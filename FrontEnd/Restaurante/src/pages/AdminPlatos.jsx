import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService } from '../services/api';
import './AdminPlatos.css';

function AdminPlatos() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [platos, setPlatos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [filtro, setFiltro] = useState('todos');

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
    cargarPlatos(usuarioObj.idUsuario);
  }, [navigate]);

  const cargarPlatos = async (usuarioId) => {
    try {
      setLoading(true);
      const data = await adminService.getTodosPlatos(usuarioId);
      setPlatos(data);
      setError('');
    } catch (err) {
      console.error('Error cargando platos:', err);
      setError('Error al cargar los platos');
    } finally {
      setLoading(false);
    }
  };

  const toggleDisponibilidad = async (platoId, disponibleActual) => {
    try {
      await adminService.cambiarDisponibilidadPlato(platoId, usuario.idUsuario, !disponibleActual);
      setSuccess(`Plato ${disponibleActual ? 'marcado como agotado' : 'marcado como disponible'}`);
      cargarPlatos(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error al cambiar disponibilidad:', err);
      setError('Error al cambiar la disponibilidad del plato');
    }
  };

  const eliminarPlato = async (platoId) => {
    if (window.confirm('¿Estás seguro de que deseas eliminar este plato?')) {
      try {
        await adminService.eliminarPlato(platoId, usuario.idUsuario);
        setSuccess('Plato eliminado correctamente');
        cargarPlatos(usuario.idUsuario);
        setTimeout(() => setSuccess(''), 3000);
      } catch (err) {
        console.error('Error al eliminar plato:', err);
        setError('Error al eliminar el plato');
      }
    }
  };

  const platosFiltrados = platos.filter((plato) => {
    if (filtro === 'disponibles') return plato.disponible && plato.activo;
    if (filtro === 'agotados') return !plato.disponible && plato.activo;
    if (filtro === 'eliminados') return !plato.activo;
    return true;
  });

  if (loading) return <div className="admin-loading">Cargando platos...</div>;

  return (
    <div className="admin-platos">
      <div className="admin-header">
        <button className="back-btn" onClick={() => navigate('/admin')}>← Volver</button>
        <h1>Gestionar Platos</h1>
        <p>Total de platos: {platos.length}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}
      {success && <div className="admin-success">{success}</div>}

      <div className="filtros">
        <button
          className={`filtro-btn ${filtro === 'todos' ? 'activo' : ''}`}
          onClick={() => setFiltro('todos')}
        >
          Todos ({platos.length})
        </button>
        <button
          className={`filtro-btn ${filtro === 'disponibles' ? 'activo' : ''}`}
          onClick={() => setFiltro('disponibles')}
        >
          Disponibles ({platos.filter(p => p.disponible && p.activo).length})
        </button>
        <button
          className={`filtro-btn ${filtro === 'agotados' ? 'activo' : ''}`}
          onClick={() => setFiltro('agotados')}
        >
          Agotados ({platos.filter(p => !p.disponible && p.activo).length})
        </button>
        <button
          className={`filtro-btn ${filtro === 'eliminados' ? 'activo' : ''}`}
          onClick={() => setFiltro('eliminados')}
        >
          Eliminados ({platos.filter(p => !p.activo).length})
        </button>
      </div>

      <div className="platos-table">
        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Categoría</th>
              <th>Precio</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {platosFiltrados.map((plato) => (
              <tr key={plato.idPlato} className={`plato-row ${!plato.activo ? 'eliminado' : ''}`}>
                <td className="nombre">{plato.nombre}</td>
                <td>{plato.categoria}</td>
                <td className="precio">${plato.precio.toFixed(2)}</td>
                <td>
                  <span className={`estado-badge ${plato.disponible ? 'disponible' : 'agotado'}`}>
                    {plato.disponible ? '✓ Disponible' : '✗ Agotado'}
                  </span>
                </td>
                <td className="acciones">
                  {plato.activo && (
                    <>
                      <button
                        className={`btn-accion ${plato.disponible ? 'agotar' : 'reponer'}`}
                        onClick={() => toggleDisponibilidad(plato.idPlato, plato.disponible)}
                        title={plato.disponible ? 'Marcar como agotado' : 'Marcar como disponible'}
                      >
                        {plato.disponible ? '🚫' : '✓'}
                      </button>
                      <button
                        className="btn-accion eliminar"
                        onClick={() => eliminarPlato(plato.idPlato)}
                        title="Eliminar plato"
                      >
                        🗑️
                      </button>
                    </>
                  )}
                  {!plato.activo && (
                    <span className="eliminado-label">Eliminado</span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {platosFiltrados.length === 0 && (
        <div className="sin-resultados">
          <p>No hay platos para mostrar con este filtro</p>
        </div>
      )}
    </div>
  );
}

export default AdminPlatos;
