import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { reservasService } from '../services/api';
import './MisReservas.css';

function MisReservas() {
  const navigate = useNavigate();
  const [reservas, setReservas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [usuario, setUsuario] = useState(null);

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (!usuarioGuardado) {
      navigate('/login');
      return;
    }

    const usuarioObj = JSON.parse(usuarioGuardado);
    setUsuario(usuarioObj);

    const cargarReservas = async () => {
      try {
        const reservasData = await reservasService.getByUsuario(usuarioObj.idUsuario);
        setReservas(reservasData);
      } catch (error) {
        console.error('Error cargando reservas:', error);
      } finally {
        setLoading(false);
      }
    };

    cargarReservas();
  }, [navigate]);

  const formatearFecha = (fecha) => {
    const fechaObj = new Date(fecha);
    return fechaObj.toLocaleString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  const getEstadoColor = (estado) => {
    switch (estado?.toLowerCase()) {
      case 'confirmada':
        return 'confirmada';
      case 'pendiente':
        return 'pendiente';
      case 'cancelada':
        return 'cancelada';
      default:
        return 'pendiente';
    }
  };

  const handleCancelar = async (id) => {
    if (!window.confirm('¿Estás seguro de que deseas cancelar esta reserva?')) {
      return;
    }

    try {
      await reservasService.updateEstado(id, 'Cancelada');
      setReservas(prev => 
        prev.map(r => r.idReserva === id ? { ...r, estado: 'Cancelada' } : r)
      );
    } catch (error) {
      console.error('Error cancelando reserva:', error);
      alert('Error al cancelar la reserva');
    }
  };

  if (loading) {
    return <div className="loading-container">Cargando tus reservas...</div>;
  }

  return (
    <div className="mis-reservas-page">
      <h1>Mis Reservas</h1>

      {reservas.length === 0 ? (
        <div className="no-reservas">
          <p>No tienes reservas aún.</p>
          <button 
            onClick={() => navigate('/reservas')}
            className="btn-crear-reserva"
          >
            Hacer una Reserva
          </button>
        </div>
      ) : (
        <div className="reservas-list">
          {reservas.map((reserva) => (
            <div key={reserva.idReserva} className="reserva-card">
              <div className="reserva-header">
                <div>
                  <h3>Reserva #{reserva.idReserva}</h3>
                  <p className="fecha-reserva">
                     {formatearFecha(reserva.fechaHora)}
                  </p>
                </div>
                <span className={`estado-badge ${getEstadoColor(reserva.estado)}`}>
                  {reserva.estado}
                </span>
              </div>

              <div className="reserva-details">
                <div className="detail-item">
                  <span className="detail-label">👥 Personas:</span>
                  <span className="detail-value">{reserva.cantidadPersonas}</span>
                </div>
                {reserva.numeroMesa && (
                  <div className="detail-item">
                    <span className="detail-label">🪑 Mesa:</span>
                    <span className="detail-value">
                      Mesa {reserva.numeroMesa}
                    </span>
                  </div>
                )}
                {reserva.comentarios && (
                  <div className="detail-item full-width">
                    <span className="detail-label">Comentarios:</span>
                    <span className="detail-value">{reserva.comentarios}</span>
                  </div>
                )}
                <div className="detail-item full-width">
                  <span className="detail-label"> Fecha de creación:</span>
                  <span className="detail-value">
                    {formatearFecha(reserva.fechaCreacion)}
                  </span>
                </div>
              </div>

              {reserva.estado?.toLowerCase() !== 'cancelada' && 
               reserva.estado?.toLowerCase() !== 'completada' && (
                <div className="reserva-actions">
                  <button
                    onClick={() => handleCancelar(reserva.idReserva)}
                    className="btn-cancelar"
                  >
                    Cancelar Reserva
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default MisReservas;
