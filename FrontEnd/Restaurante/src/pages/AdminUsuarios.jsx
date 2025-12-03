import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminUsuarios.css';

function AdminUsuarios() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [usuarios, setUsuarios] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [filtro, setFiltro] = useState('');
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
    cargarUsuarios(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarUsuarios = async (usuarioId) => {
    try {
      setLoading(true);
      const data = await adminService.getTodosUsuarios(usuarioId);
      setUsuarios(data);
      setError('');
    } catch (err) {
      console.error('Error cargando usuarios:', err);
      setError('Error al cargar los usuarios');
    } finally {
      setLoading(false);
    }
  };

  const handleCambiarRol = async (usuarioTargetId, nuevoRol) => {
    try {
      await adminService.cambiarRolUsuario(usuarioTargetId, usuario.idUsuario, nuevoRol);
      setSuccess('Rol actualizado correctamente');
      cargarUsuarios(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error al cambiar rol:', err);
      setError('Error al cambiar el rol del usuario');
    }
  };

  const handleCambiarEstado = async (usuarioTargetId, nuevoEstado) => {
    try {
      await adminService.cambiarEstadoUsuario(usuarioTargetId, usuario.idUsuario, nuevoEstado);
      setSuccess('Estado actualizado correctamente');
      cargarUsuarios(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error al cambiar estado:', err);
      setError('Error al cambiar el estado del usuario');
    }
  };

  const usuariosFiltrados = usuarios.filter(u =>
    u.nombre.toLowerCase().includes(filtro.toLowerCase()) ||
    u.apellido.toLowerCase().includes(filtro.toLowerCase()) ||
    u.email.toLowerCase().includes(filtro.toLowerCase()) ||
    u.idUsuario.toString().includes(filtro)
  );

  if (loading) return <div className="admin-loading">Cargando usuarios...</div>;

  return (
    <div className="admin-usuarios">
      <div className="admin-header">
        <button className="back-btn" onClick={() => navigate('/admin')}>Volver</button>
        <h1>Gestionar Usuarios</h1>
        <p>Total de usuarios: {usuarios.length}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}
      {success && <div className="admin-success">{success}</div>}

      <div className="filtro-container">
        <input
          type="text"
          placeholder="Buscar por nombre, email o ID..."
          value={filtro}
          onChange={(e) => setFiltro(e.target.value)}
          className="filtro-input"
        />
      </div>

      <div className="usuarios-table">
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Email</th>
              <th>Teléfono</th>
              <th>Pedidos</th>
              <th>Reservas</th>
              <th>Rol</th>
              <th>Estado</th>
              <th>Fecha Registro</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {usuariosFiltrados.length > 0 ? (
              usuariosFiltrados.map((usr) => (
                <tr key={usr.idUsuario}>
                  <td>#{usr.idUsuario}</td>
                  <td className="nombre">{usr.nombre} {usr.apellido}</td>
                  <td>{usr.email}</td>
                  <td>{usr.telefono || '-'}</td>
                  <td className="numero">{usr.totalPedidos}</td>
                  <td className="numero">{usr.totalReservas}</td>
                  <td>
                    <select
                      value={usr.rol}
                      onChange={(e) => handleCambiarRol(usr.idUsuario, e.target.value)}
                      className="rol-select"
                    >
                      <option value="cliente">Cliente</option>
                      <option value="admin">Admin</option>
                    </select>
                  </td>
                  <td>
                    <span className={`estado-badge ${usr.activo ? 'activo' : 'inactivo'}`}>
                      {usr.activo ? 'Activo' : 'Inactivo'}
                    </span>
                  </td>
                  <td>{new Date(usr.fechaRegistro).toLocaleDateString()}</td>
                  <td className="acciones">
                    <button
                      className={`btn-accion ${usr.activo ? 'desactivar' : 'activar'}`}
                      onClick={() => handleCambiarEstado(usr.idUsuario, !usr.activo)}
                      title={usr.activo ? 'Desactivar usuario' : 'Activar usuario'}
                    >
                      {usr.activo ? 'Desactivar' : 'Activar'}
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="10" className="sin-resultados">No hay usuarios para mostrar</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default AdminUsuarios;
