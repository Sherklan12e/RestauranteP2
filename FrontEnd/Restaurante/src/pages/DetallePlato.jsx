import { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { platosService, calificacionesService } from '../services/api';
import { useCarrito } from '../context/CarritoContext';
import CalificarPlato from '../components/CalificarPlato';
import './DetallePlato.css';

function DetallePlato() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { agregarAlCarrito } = useCarrito();
  const [plato, setPlato] = useState(null);
  const [calificaciones, setCalificaciones] = useState([]);
  const [loading, setLoading] = useState(true);
  const [cantidad, setCantidad] = useState(1);
  const [usuario, setUsuario] = useState(null);
  const [mostrarCalificar, setMostrarCalificar] = useState(false);

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      setUsuario(JSON.parse(usuarioGuardado));
    }

    const cargarDatos = async () => {
      try {
        const [platoData, calificacionesData] = await Promise.all([
          platosService.getById(id),
          calificacionesService.getByPlato(id),
        ]);
        setPlato(platoData);
        setCalificaciones(calificacionesData);
      } catch (error) {
        console.error('Error cargando datos:', error);
        if (error.response?.status === 404) {
          navigate('/menu');
        }
      } finally {
        setLoading(false);
      }
    };

    cargarDatos();
  }, [id, navigate]);

  const handleAgregarAlCarrito = () => {
    if (!plato) return;
    agregarAlCarrito(plato, cantidad);
    alert('Plato agregado al carrito!');
  };

  const calcularPromedio = () => {
    if (calificaciones.length === 0) return 0;
    const suma = calificaciones.reduce((acc, cal) => acc + cal.puntuacion, 0);
    return (suma / calificaciones.length).toFixed(1);
  };

  const handleCalificacionCreada = async () => {
    const nuevasCalificaciones = await calificacionesService.getByPlato(id);
    setCalificaciones(nuevasCalificaciones);
    setMostrarCalificar(false);
  };

  if (loading) {
    return <div className="loading-container">Cargando...</div>;
  }

  if (!plato) {
    return (
      <div className="error-container">
        <p>Plato no encontrado</p>
        <Link to="/menu">Volver al menú</Link>
      </div>
    );
  }

  const promedio = calcularPromedio();

  return (
    <div className="detalle-plato-page">
      <div className="plato-main">
        <div className="plato-imagen-section">
          {plato.imagenUrl ? (
            <img src={plato.imagenUrl} alt={plato.nombre} className="plato-imagen-grande" />
          ) : (
            <div className="plato-imagen-placeholder">🍽️</div>
          )}
        </div>

        <div className="plato-info-section">
          <div className="plato-header">
            <h1>{plato.nombre}</h1>
            {plato.esMenuDelDia && <span className="badge-menu-dia">Menú del Día</span>}
          </div>

          <p className="plato-categoria">{plato.categoriaNombre}</p>

          <div className="plato-rating">
            <div className="rating-stars">
              {'⭐'.repeat(Math.round(promedio))}
              <span className="rating-number">({promedio})</span>
            </div>
            <span className="rating-count">
              {calificaciones.length} calificaciones
            </span>
          </div>

          <p className="plato-descripcion">{plato.descripcion}</p>

          <div className="plato-detalles">
            <div className="detalle-item">
              <span className="detalle-label">⏱️ Tiempo de preparación:</span>
              <span className="detalle-value">{plato.tiempoPreparacion} minutos</span>
            </div>
            <div className="detalle-item">
              <span className="detalle-label">💰 Precio:</span>
              <span className="detalle-value precio">${plato.precio}</span>
            </div>
            <div className="detalle-item">
              <span className="detalle-label">✅ Disponibilidad:</span>
              <span className={`detalle-value ${plato.disponible ? 'disponible' : 'no-disponible'}`}>
                {plato.disponible ? 'Disponible' : 'No disponible'}
              </span>
            </div>
          </div>

          {plato.disponible && (
            <div className="plato-acciones">
              <div className="cantidad-selector">
                <button
                  onClick={() => setCantidad(Math.max(1, cantidad - 1))}
                  className="btn-cantidad"
                >
                  -
                </button>
                <span className="cantidad-value">{cantidad}</span>
                <button
                  onClick={() => setCantidad(cantidad + 1)}
                  className="btn-cantidad"
                >
                  +
                </button>
              </div>
              <button onClick={handleAgregarAlCarrito} className="btn-agregar-carrito">
                🛒 Agregar al Carrito
              </button>
            </div>
          )}

          {usuario && (
            <button
              onClick={() => setMostrarCalificar(!mostrarCalificar)}
              className="btn-calificar"
            >
              {mostrarCalificar ? 'Cancelar' : '⭐ Calificar este plato'}
            </button>
          )}

          {mostrarCalificar && usuario && (
            <CalificarPlato
              platoId={plato.idPlato}
              usuarioId={usuario.idUsuario}
              onCalificacionCreada={handleCalificacionCreada}
            />
          )}
        </div>
      </div>

      <div className="calificaciones-section">
        <h2>Calificaciones y Comentarios</h2>
        
        {calificaciones.length === 0 ? (
          <p className="sin-calificaciones">
            Aún no hay calificaciones para este plato. ¡Sé el primero en calificarlo!
          </p>
        ) : (
          <div className="calificaciones-list">
            {calificaciones.map((cal) => (
              <div key={cal.idCalificacion} className="calificacion-card">
                <div className="calificacion-header">
                  <div>
                    <strong>{cal.nombreUsuario || 'Usuario'}</strong>
                    <div className="calificacion-estrellas">
                      {'⭐'.repeat(cal.puntuacion)}
                    </div>
                  </div>
                  <span className="calificacion-fecha">
                    {new Date(cal.fechaHora).toLocaleDateString('es-ES')}
                  </span>
                </div>
                {cal.comentario && (
                  <p className="calificacion-comentario">{cal.comentario}</p>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

export default DetallePlato;
