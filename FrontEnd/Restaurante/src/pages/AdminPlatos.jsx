import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminService, categoriasService } from '../services/api';
import './AdminPlatos.css';

function AdminPlatos() {
  const navigate = useNavigate();
  const [usuario, setUsuario] = useState(null);
  const [platos, setPlatos] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [filtro, setFiltro] = useState('todos');
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [formData, setFormData] = useState({
    nombre: '',
    descripcion: '',
    precio: '',
    idCategoria: '',
    tiempoPreparacion: '',
    imagenUrl: '',
    disponible: true,
    esMenuDelDia: false
  });

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
    cargarCategorias();
  }, [navigate]);

  const cargarCategorias = async () => {
    try {
      const data = await categoriasService.getAll();
      setCategorias(data);
    } catch (err) {
      console.error('Error cargando categorías:', err);
    }
  };

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

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmitFormulario = async (e) => {
    e.preventDefault();
    
    if (!formData.nombre || !formData.precio || !formData.idCategoria) {
      setError('Por favor completa los campos obligatorios');
      return;
    }

    try {
      const nuevoPlato = {
        nombre: formData.nombre,
        descripcion: formData.descripcion,
        precio: parseFloat(formData.precio),
        idCategoria: parseInt(formData.idCategoria),
        tiempoPreparacion: formData.tiempoPreparacion ? parseInt(formData.tiempoPreparacion) : 0,
        imagenUrl: formData.imagenUrl,
        disponible: formData.disponible,
        esMenuDelDia: formData.esMenuDelDia
      };

      await adminService.crearPlato(usuario.idUsuario, nuevoPlato);
      setSuccess('Plato creado correctamente');
      setFormData({
        nombre: '',
        descripcion: '',
        precio: '',
        idCategoria: '',
        tiempoPreparacion: '',
        imagenUrl: '',
        disponible: true,
        esMenuDelDia: false
      });
      setMostrarFormulario(false);
      cargarPlatos(usuario.idUsuario);
      setTimeout(() => setSuccess(''), 3000);
    } catch (err) {
      console.error('Error al crear plato:', err);
      setError('Error al crear el plato');
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
        <button className="back-btn" onClick={() => navigate('/admin')}>Volver</button>
        <h1>Gestionar Platos</h1>
        <p>Total de platos: {platos.length}</p>
      </div>

      {error && <div className="admin-error">{error}</div>}
      {success && <div className="admin-success">{success}</div>}

      <div className="header-actions">
        <button 
          className="btn-crear-plato" 
          onClick={() => setMostrarFormulario(!mostrarFormulario)}
        >
          {mostrarFormulario ? 'Cancelar' : 'Crear Nuevo Plato'}
        </button>
      </div>

      {mostrarFormulario && (
        <div className="formulario-container">
          <form onSubmit={handleSubmitFormulario} className="form-crear-plato">
            <h2>Crear Nuevo Plato</h2>
            
            <div className="form-group">
              <label htmlFor="nombre">Nombre del Plato *</label>
              <input
                type="text"
                id="nombre"
                name="nombre"
                value={formData.nombre}
                onChange={handleInputChange}
                placeholder="Ej: Pasta Carbonara"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="descripcion">Descripción</label>
              <textarea
                id="descripcion"
                name="descripcion"
                value={formData.descripcion}
                onChange={handleInputChange}
                placeholder="Describe el plato"
                rows="3"
              />
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="precio">Precio *</label>
                <input
                  type="number"
                  id="precio"
                  name="precio"
                  value={formData.precio}
                  onChange={handleInputChange}
                  placeholder="0.00"
                  step="0.01"
                  min="0"
                  required
                />
              </div>

              <div className="form-group">
                <label htmlFor="idCategoria">Categoría *</label>
                <select
                  id="idCategoria"
                  name="idCategoria"
                  value={formData.idCategoria}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">Selecciona una categoría</option>
                  {categorias.map(cat => (
                    <option key={cat.idCategoria} value={cat.idCategoria}>
                      {cat.nombre}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label htmlFor="tiempoPreparacion">Tiempo de Preparación (min)</label>
                <input
                  type="number"
                  id="tiempoPreparacion"
                  name="tiempoPreparacion"
                  value={formData.tiempoPreparacion}
                  onChange={handleInputChange}
                  placeholder="30"
                  min="0"
                />
              </div>

              <div className="form-group">
                <label htmlFor="imagenUrl">URL de Imagen</label>
                <input
                  type="url"
                  id="imagenUrl"
                  name="imagenUrl"
                  value={formData.imagenUrl}
                  onChange={handleInputChange}
                  placeholder="https://ejemplo.com/imagen.jpg"
                />
              </div>
            </div>

            <div className="form-checkboxes">
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  name="disponible"
                  checked={formData.disponible}
                  onChange={handleInputChange}
                />
                Disponible
              </label>
              <label className="checkbox-label">
                <input
                  type="checkbox"
                  name="esMenuDelDia"
                  checked={formData.esMenuDelDia}
                  onChange={handleInputChange}
                />
                Menu del Día
              </label>
            </div>

            <div className="form-actions">
              <button type="submit" className="btn-submit">Crear Plato</button>
              <button 
                type="button" 
                className="btn-cancel" 
                onClick={() => setMostrarFormulario(false)}
              >
                Cancelar
              </button>
            </div>
          </form>
        </div>
      )}

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
                    {plato.disponible ? 'Disponible' : 'Agotado'}
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
                        {plato.disponible ? 'Agotar' : 'Reponer'}
                      </button>
                      <button
                        className="btn-accion eliminar"
                        onClick={() => eliminarPlato(plato.idPlato)}
                        title="Eliminar plato"
                      >
                        Eliminar
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
