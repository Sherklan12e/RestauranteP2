import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { reservasService, mesasService, usuariosService } from '../services/api';
import './Reservas.css';

function Reservas() {
  const navigate = useNavigate();
  const [mesas, setMesas] = useState([]);
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(true);
  const [enviando, setEnviando] = useState(false);
  const [mensaje, setMensaje] = useState({ tipo: '', texto: '' });

  const [formData, setFormData] = useState({
    idMesa: '',
    fechaHora: '',
    cantidadPersonas: 1,
    comentarios: '',
  });

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (!usuarioGuardado) {
      navigate('/login');
      return;
    }

    const usuarioObj = JSON.parse(usuarioGuardado);
    setUsuario(usuarioObj);

    const cargarMesas = async () => {
      try {
        const mesasData = await mesasService.getAll();
        const mesasDisponibles = mesasData.filter(m => m.activa);
        setMesas(mesasDisponibles);
      } catch (error) {
        console.error('Error cargando mesas:', error);
        setMensaje({ tipo: 'error', texto: 'Error al cargar las mesas disponibles' });
      } finally {
        setLoading(false);
      }
    };

    cargarMesas();
  }, [navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'cantidadPersonas' ? parseInt(value) || 1 :
              name === 'idMesa' ? (value ? parseInt(value) : null) : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    if (!usuario) {
      navigate('/login');
      return;
    }

    if (!formData.fechaHora) {
      setMensaje({ tipo: 'error', texto: 'Por favor selecciona una fecha y hora' });
      return;
    }

    setEnviando(true);
    setMensaje({ tipo: '', texto: '' });

    try {
      const fechaHoraDate = new Date(formData.fechaHora);
      
      const reservaData = {
        idUsuario: usuario.idUsuario,
        idMesa: formData.idMesa || null,
        fechaHora: fechaHoraDate.toISOString(),
        cantidadPersonas: formData.cantidadPersonas,
        comentarios: formData.comentarios || null,
      };

      await reservasService.create(reservaData);
      
      setMensaje({ 
        tipo: 'exito', 
        texto: '¡Reserva creada exitosamente! Redirigiendo...' 
      });

      setTimeout(() => {
        navigate('/mis-reservas');
      }, 2000);

    } catch (error) {
      console.error('Error creando reserva:', error);
      setMensaje({ 
        tipo: 'error', 
        texto: error.response?.data?.message || 'Error al crear la reserva. Por favor intenta de nuevo.' 
      });
    } finally {
      setEnviando(false);
    }
  };

  const obtenerFechaMinima = () => {
    const fecha = new Date();
    fecha.setHours(fecha.getHours() + 1);
    return fecha.toISOString().slice(0, 16);
  };

  if (loading) {
    return <div className="loading-container">Cargando...</div>;
  }

  return (
    <div className="reservas-page">
      <h1>Hacer una Reserva</h1>
      
      <div className="reserva-container">
        <div className="reserva-form-wrapper">
          <form onSubmit={handleSubmit} className="reserva-form">
            <div className="form-group">
              <label htmlFor="fechaHora">Fecha y Hora *</label>
              <input
                type="datetime-local"
                id="fechaHora"
                name="fechaHora"
                value={formData.fechaHora}
                onChange={handleChange}
                min={obtenerFechaMinima()}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="cantidadPersonas">Cantidad de Personas *</label>
              <input
                type="number"
                id="cantidadPersonas"
                name="cantidadPersonas"
                value={formData.cantidadPersonas}
                onChange={handleChange}
                min="1"
                max="20"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="idMesa">Mesa (Opcional)</label>
              <select
                id="idMesa"
                name="idMesa"
                value={formData.idMesa || ''}
                onChange={handleChange}
              >
                <option value="">Sin mesa específica</option>
                {mesas.map((mesa) => (
                  <option key={mesa.idMesa} value={mesa.idMesa}>
                    Mesa {mesa.numeroMesa} - {mesa.capacidad} personas
                  </option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="comentarios">Comentarios o Solicitudes Especiales</label>
              <textarea
                id="comentarios"
                name="comentarios"
                value={formData.comentarios}
                onChange={handleChange}
                rows="4"
                placeholder="Ej: Mesa cerca de la ventana, alergias alimentarias, etc."
              />
            </div>

            {mensaje.texto && (
              <div className={`mensaje ${mensaje.tipo}`}>
                {mensaje.texto}
              </div>
            )}

            <button 
              type="submit" 
              className="btn-submit"
              disabled={enviando}
            >
              {enviando ? 'Procesando...' : 'Confirmar Reserva'}
            </button>
          </form>
        </div>

        <div className="reserva-info">
          <h2>Información de Reserva</h2>
          <div className="info-card">
            <p>📍 Dirección del Restaurante</p>
            <p>📞 Teléfono: (123) 456-7890</p>
            <p>🕐 Horario: Lunes a Domingo 12:00 - 23:00</p>
          </div>
          <div className="info-tips">
            <h3>Consejos para tu reserva:</h3>
            <ul>
              <li>Reserva con al menos 1 hora de anticipación</li>
              <li>Si tienes alergias, menciónalo en los comentarios</li>
              <li>Puedes seleccionar una mesa específica o dejarnos elegir</li>
              <li>Recibirás confirmación por correo electrónico</li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Reservas;
