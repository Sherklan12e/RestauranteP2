import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { pedidosService } from '../services/api';
import './MisPedidos.css';

function MisPedidos() {
  const navigate = useNavigate();
  const [pedidos, setPedidos] = useState([]);
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

    const cargarPedidos = async () => {
      try {
        const pedidosData = await pedidosService.getByUsuario(usuarioObj.idUsuario);
        setPedidos(pedidosData);
      } catch (error) {
        console.error('Error cargando pedidos:', error);
      } finally {
        setLoading(false);
      }
    };

    cargarPedidos();
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
    const estadoLower = estado?.toLowerCase();
    if (estadoLower?.includes('pendiente')) return 'pendiente';
    if (estadoLower?.includes('confirmado')) return 'confirmada';
    if (estadoLower?.includes('preparación')) return 'preparacion';
    if (estadoLower?.includes('listo')) return 'lista';
    if (estadoLower?.includes('entregado')) return 'entregada';
    if (estadoLower?.includes('cancelado')) return 'cancelada';
    return 'pendiente';
  };

  if (loading) {
    return <div className="loading-container">Cargando tus pedidos...</div>;
  }

  return (
    <div className="mis-pedidos-page">
      <h1>Mis Pedidos</h1>

      {pedidos.length === 0 ? (
        <div className="no-pedidos">
          <p>No tienes pedidos aún.</p>
          <button onClick={() => navigate('/menu')} className="btn-hacer-pedido">
            Hacer un Pedido
          </button>
        </div>
      ) : (
        <div className="pedidos-list">
          {pedidos.map((pedido) => (
            <div key={pedido.idPedido} className="pedido-card">
              <div className="pedido-header">
                <div>
                  <h3>Pedido #{pedido.idPedido}</h3>
                  <p className="fecha-pedido">
                    📅 {formatearFecha(pedido.fechaHoraPedido)}
                  </p>
                </div>
                <span className={`estado-badge ${getEstadoColor(pedido.estadoNombre)}`}>
                  {pedido.estadoNombre}
                </span>
              </div>

              <div className="pedido-detalles">
                {pedido.detalles && pedido.detalles.length > 0 && (
                  <div className="detalles-items">
                    <h4>Platos:</h4>
                    <ul>
                      {pedido.detalles.map((detalle) => (
                        <li key={detalle.idDetallePedido}>
                          <span className="item-nombre">{detalle.nombrePlato}</span>
                          <span className="item-cantidad">x{detalle.cantidad}</span>
                          <span className="item-precio">${detalle.subtotal.toFixed(2)}</span>
                        </li>
                      ))}
                    </ul>
                  </div>
                )}

                <div className="pedido-info">
                  <div className="info-item">
                    <span className="info-label">Total:</span>
                    <span className="info-value precio-total">${pedido.total.toFixed(2)}</span>
                  </div>
                  {pedido.metodoPagoNombre && (
                    <div className="info-item">
                      <span className="info-label">Método de pago:</span>
                      <span className="info-value">{pedido.metodoPagoNombre}</span>
                    </div>
                  )}
                  {pedido.fechaHoraEntregaEstimada && (
                    <div className="info-item">
                      <span className="info-label">Entrega estimada:</span>
                      <span className="info-value">
                        {formatearFecha(pedido.fechaHoraEntregaEstimada)}
                      </span>
                    </div>
                  )}
                  {pedido.comentarios && (
                    <div className="info-item">
                      <span className="info-label">Comentarios:</span>
                      <span className="info-value">{pedido.comentarios}</span>
                    </div>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default MisPedidos;
