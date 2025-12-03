import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminPedidos.css';

function AdminPedidos() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [pedidos, setPedidos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [filtro, setFiltro] = useState('');

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (!usuarioGuardado) {
      navigate('/login');
      return;
    }

    const usuarioObj = JSON.parse(usuarioGuardado);
    
    if (usuarioObj.rol !== 'admin') {
      navigate('/');
      return;
    }

    setUsuario(usuarioObj);
    cargarPedidos(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarPedidos = async (usuarioId) => {
    try {
      const data = await adminService.getTodosPedidos(usuarioId);
      setPedidos(data);
    } catch (err) {
      console.error('Error cargando pedidos:', err);
      setError('Error al cargar los pedidos');
    } finally {
      setLoading(false);
    }
  };

  const handleCambiarEstado = async (pedidoId, nuevoEstado) => {
    try {
      await adminService.actualizarEstadoPedido(pedidoId, usuario.idUsuario, nuevoEstado);
      cargarPedidos(usuario.idUsuario);
      setError('');
    } catch (err) {
      console.error('Error actualizando estado:', err);
      setError('Error al actualizar el estado del pedido');
    }
  };

  const pedidosFiltrados = pedidos.filter(p =>
    p.nombreUsuario.toLowerCase().includes(filtro.toLowerCase()) ||
    p.emailUsuario.toLowerCase().includes(filtro.toLowerCase()) ||
    p.idPedido.toString().includes(filtro)
  );

  if (loading) return <div className="admin-loading">Cargando pedidos...</div>;

  return (
    <div className="admin-pedidos">
      <div className="admin-header-page">
        <button className="btn-volver" onClick={() => navigate('/admin')}>← Volver</button>
        <h1>Gestión de Pedidos</h1>
      </div>

      {error && <div className="admin-error">{error}</div>}

      <div className="filtro-container">
        <input
          type="text"
          placeholder="Buscar por nombre, email o ID de pedido..."
          value={filtro}
          onChange={(e) => setFiltro(e.target.value)}
          className="filtro-input"
        />
      </div>

      <div className="pedidos-table-container">
        <table className="pedidos-table">
          <thead>
            <tr>
              <th>ID Pedido</th>
              <th>Cliente</th>
              <th>Email</th>
              <th>Fecha</th>
              <th>Total</th>
              <th>Platos</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {pedidosFiltrados.length > 0 ? (
              pedidosFiltrados.map(pedido => (
                <tr key={pedido.idPedido}>
                  <td>#{pedido.idPedido}</td>
                  <td>{pedido.nombreUsuario}</td>
                  <td>{pedido.emailUsuario}</td>
                  <td>{new Date(pedido.fechaHoraPedido).toLocaleDateString()}</td>
                  <td>${pedido.total.toFixed(2)}</td>
                  <td>
                    <div className="platos-list">
                      {pedido.detalles && pedido.detalles.length > 0 ? (
                        <ul>
                          {pedido.detalles.map((detalle) => (
                            <li key={detalle.idDetallePedido}>
                              {detalle.nombrePlato} x{detalle.cantidad}
                            </li>
                          ))}
                        </ul>
                      ) : (
                        <span>-</span>
                      )}
                    </div>
                  </td>
                  <td>
                    <span className={`estado-badge estado-${pedido.estado.toLowerCase()}`}>
                      {pedido.estado}
                    </span>
                  </td>
                  <td>
                    <select
                      value={pedido.estado}
                      onChange={(e) => handleCambiarEstado(pedido.idPedido, e.target.value)}
                      className="estado-select"
                    >
                      <option value="Pendiente">Pendiente</option>
                      <option value="En Preparación">En Preparación</option>
                      <option value="Listo">Listo</option>
                      <option value="Entregado">Entregado</option>
                      <option value="Cancelado">Cancelado</option>
                    </select>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="8" className="no-data">No hay pedidos</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default AdminPedidos;
