import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminDashboard.css';

function AdminDashboard() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [estadisticas, setEstadisticas] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (!usuarioGuardado) {
      navigate('/login');
      return;
    }

    const usuarioObj = JSON.parse(usuarioGuardado);
    
    // Verificar si es admin
    if (usuarioObj.rol !== 'admin') {
      navigate('/');
      return;
    }

    setUsuario(usuarioObj);
    cargarEstadisticas(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarEstadisticas = async (usuarioId) => {
    try {
      const data = await adminService.getEstadisticas(usuarioId);
      setEstadisticas(data);
    } catch (err) {
      console.error('Error cargando estadísticas:', err);
      setError('Error al cargar las estadísticas');
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="admin-loading">Cargando...</div>;

  return (
    <div className="admin-dashboard">
      <div className="admin-header">
        <h1>Panel de Administración</h1>
        <p>Bienvenido, {usuario?.nombre}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}

      {estadisticas && (
        <div className="estadisticas-grid">
          <div className="estadistica-card">
            <h3>Pedidos</h3>
            <p className="numero">{estadisticas.totalPedidos}</p>
            <button onClick={() => navigate('/admin/pedidos')}>Ver Pedidos</button>
          </div>

          <div className="estadistica-card">
            <h3>Reservas</h3>
            <p className="numero">{estadisticas.totalReservas}</p>
            <button onClick={() => navigate('/admin/reservas')}>Ver Reservas</button>
          </div>

          <div className="estadistica-card">
            <h3>Usuarios</h3>
            <p className="numero">{estadisticas.totalUsuarios}</p>
            <button onClick={() => navigate('/admin/usuarios')}>Gestionar Usuarios</button>
          </div>

          <div className="estadistica-card">
            <h3>Platos</h3>
            <p className="numero">{estadisticas.totalPlatos}</p>
            <button onClick={() => navigate('/admin/platos')}>Gestionar Platos</button>
          </div>

          <div className="estadistica-card">
            <h3>Mesas</h3>
            <p className="numero">-</p>
            <button onClick={() => navigate('/admin/mesas')}>Gestionar Mesas</button>
          </div>

          <div className="estadistica-card ingresos">
            <h3>Ingresos Totales</h3>
            <p className="numero">${estadisticas.ingresosTotales.toFixed(2)}</p>
          </div>
        </div>
      )}

      <div className="admin-menu">
        <h2>Opciones de Administración</h2>
        <div className="menu-buttons">
          <button className="menu-btn" onClick={() => navigate('/admin/pedidos')}>
            📦 Gestionar Pedidos
          </button>
          <button className="menu-btn" onClick={() => navigate('/admin/reservas')}>
            📅 Gestionar Reservas
          </button>
          <button className="menu-btn" onClick={() => navigate('/admin/usuarios')}>
            👥 Gestionar Usuarios
          </button>
          <button className="menu-btn" onClick={() => navigate('/admin/platos')}>
            🍽️ Gestionar Platos
          </button>
          <button className="menu-btn" onClick={() => navigate('/admin/mesas')}>
            🪑 Gestionar Mesas
          </button>
        </div>
      </div>
    </div>
  );
}

export default AdminDashboard;
