import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminReservas.css';

function AdminReservas() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [reservas, setReservas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
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
    cargarReservas(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarReservas = async (usuarioId) => {
    try {
      setLoading(true);
      const data = await adminService.getTodasReservas(usuarioId);
      setReservas(data);
      setError('');
    } catch (err) {
      console.error('Error cargando reservas:', err);
      setError('Error al cargar las reservas');
    } finally {
      setLoading(false);
    }
  };

  const handleCambiarEstado = async (reservaId, nuevoEstado) => {
    try {
      await adminService.actualizarEstadoReserva(reservaId, usuario.idUsuario, nuevoEstado);
      setSuccess('Estado actualizado correctamente');
      cargarReservas(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error actualizando estado:', err);
      setError('Error al actualizar el estado de la reserva');
    }
  };

  const reservasFiltradas = reservas.filter(r =>
    r.nombreUsuario.toLowerCase().includes(filtro.toLowerCase()) ||
    r.emailUsuario.toLowerCase().includes(filtro.toLowerCase()) ||
    r.idReserva.toString().includes(filtro) ||
    r.numeroMesa.toLowerCase().includes(filtro.toLowerCase())
  );

  if (loading) return <div className="admin-loading">Cargando reservas...</div>;

  return (
    <div className="admin-reservas">
      <div className="admin-header">
        <button className="back-btn" onClick={() => navigate('/admin')}>Volver</button>
        <h1>Gestionar Reservas</h1>
        <p>Total de reservas: {reservas.length}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}
      {success && <div className="admin-success">{success}</div>}

      <div className="filtro-container">
        <input
          type="text"
          placeholder="Buscar por nombre, email, mesa o ID..."
          value={filtro}
          onChange={(e) => setFiltro(e.target.value)}
          className="filtro-input"
        />
      </div>

      <div className="reservas-table">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Cliente</th>
              <th>Email</th>
              <th>Mesa</th>
              <th>Personas</th>
              <th>Fecha y Hora</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {reservasFiltradas.length > 0 ? (
              reservasFiltradas.map((reserva) => (
                <tr key={reserva.idReserva}>
                  <td>#{reserva.idReserva}</td>
                  <td className="nombre">{reserva.nombreUsuario}</td>
                  <td>{reserva.emailUsuario}</td>
                  <td className="mesa">{reserva.numeroMesa}</td>
                  <td className="numero">{reserva.cantidadPersonas}</td>
                  <td>{new Date(reserva.fechaHora).toLocaleString()}</td>
                  <td>
                    <span className={`estado-badge estado-${reserva.estado.toLowerCase()}`}>
                      {reserva.estado}
                    </span>
                  </td>
                  <td>
                    <select
                      value={reserva.estado}
                      onChange={(e) => handleCambiarEstado(reserva.idReserva, e.target.value)}
                      className="estado-select"
                    >
                      <option value="Pendiente">Pendiente</option>
                      <option value="Confirmada">Confirmada</option>
                      <option value="Completada">Completada</option>
                      <option value="Cancelada">Cancelada</option>
                    </select>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="8" className="sin-resultados">No hay reservas para mostrar</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default AdminReservas;
