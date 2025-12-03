import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminMesas.css';

function AdminMesas() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [mesas, setMesas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

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
    cargarMesas(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarMesas = async (usuarioId) => {
    try {
      setLoading(true);
      const data = await adminService.getTodasMesas(usuarioId);
      setMesas(data);
      setError('');
    } catch (err) {
      console.error('Error cargando mesas:', err);
      setError('Error al cargar las mesas');
    } finally {
      setLoading(false);
    }
  };

  const toggleEstadoMesa = async (mesaId, estadoActual) => {
    try {
      await adminService.cambiarEstadoMesa(mesaId, usuario.idUsuario, !estadoActual);
      setSuccess(`Mesa ${estadoActual ? 'deshabilitada' : 'habilitada'} correctamente`);
      cargarMesas(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error al cambiar estado de mesa:', err);
      setError('Error al cambiar el estado de la mesa');
    }
  };

  if (loading) return <div className="admin-loading">Cargando mesas...</div>;

  return (
    <div className="admin-mesas">
      <div className="admin-header">
        <button className="back-btn" onClick={() => navigate('/admin')}>← Volver</button>
        <h1>Gestionar Mesas</h1>
        <p>Total de mesas: {mesas.length}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}
      {success && <div className="admin-success">{success}</div>}

      <div className="mesas-grid">
        {mesas.map((mesa) => (
          <div key={mesa.idMesa} className={`mesa-card ${mesa.activa ? 'activa' : 'inactiva'}`}>
            <div className="mesa-info">
              <h3>Mesa {mesa.numeroMesa}</h3>
              <p className="capacidad">Capacidad: {mesa.capacidad} personas</p>
              <p className={`estado ${mesa.activa ? 'disponible' : 'ocupada'}`}>
                {mesa.activa ? 'Disponible' : 'Ocupada'}
              </p>
              {mesa.tieneReservaConfirmada && (
                <p className="reserva-confirmada">Reserva confirmada</p>
              )}
            </div>
            <button
              className={`toggle-btn ${mesa.activa ? 'desactivar' : 'activar'}`}
              onClick={() => toggleEstadoMesa(mesa.idMesa, mesa.activa)}
            >
              {mesa.activa ? 'Ocupar' : 'Liberar'}
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}

export default AdminMesas;
