import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { platosService, categoriasService } from '../services/api';
import { useCarrito } from '../context/CarritoContext';
import './Menu.css';

function Menu() {
  const { agregarAlCarrito } = useCarrito();
  const [platos, setPlatos] = useState([]);
  const [categorias, setCategorias] = useState([]);
  const [categoriaSeleccionada, setCategoriaSeleccionada] = useState(null);
  const [loading, setLoading] = useState(true);
  const [busqueda, setBusqueda] = useState('');

  useEffect(() => {
    const cargarDatos = async () => {
      try {
        const [platosData, categoriasData] = await Promise.all([
          platosService.getAll(),
          categoriasService.getAll(),
        ]);
        setPlatos(platosData);
        setCategorias(categoriasData);
      } catch (error) {
        console.error('Error cargando datos:', error);
      } finally {
        setLoading(false);
      }
    };
    cargarDatos();
  }, []);

  const platosFiltrados = platos.filter((plato) => {
    const coincideCategoria = !categoriaSeleccionada || plato.idCategoria === categoriaSeleccionada;
    const coincideBusqueda = plato.nombre.toLowerCase().includes(busqueda.toLowerCase()) ||
                            (plato.descripcion && plato.descripcion.toLowerCase().includes(busqueda.toLowerCase()));
    return coincideCategoria && coincideBusqueda && plato.disponible;
  });

  if (loading) {
    return <div className="loading-container">Cargando menú...</div>;
  }

  return (
    <div className="menu-page">
      <h1>Nuestro Menú</h1>

      <div className="menu-filters">
        <div className="search-box">
          <input
            type="text"
            placeholder="Buscar plato..."
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
            className="search-input"
          />
        </div>

        <div className="categorias-filter">
          <button
            className={`filter-btn ${!categoriaSeleccionada ? 'active' : ''}`}
            onClick={() => setCategoriaSeleccionada(null)}
          >
            Todas
          </button>
          {categorias.map((categoria) => (
            <button
              key={categoria.idCategoria}
              className={`filter-btn ${categoriaSeleccionada === categoria.idCategoria ? 'active' : ''}`}
              onClick={() => setCategoriaSeleccionada(categoria.idCategoria)}
            >
              {categoria.nombre}
            </button>
          ))}
        </div>
      </div>

      {platosFiltrados.length === 0 ? (
        <div className="no-results">
          <p>No se encontraron platos con los filtros seleccionados.</p>
        </div>
      ) : (
        <div className="platos-container">
          {platosFiltrados.map((plato) => (
            <div key={plato.idPlato} className="menu-plato-card">
              <div className="plato-imagen">
                {plato.imagenUrl ? (
                  <img src={plato.imagenUrl} alt={plato.nombre} />
                ) : (
                  <div className="placeholder-imagen">🍽️</div>
                )}
                {plato.esMenuDelDia && (
                  <span className="menu-del-dia-badge">Menú del Día</span>
                )}
              </div>
              <div className="plato-detalles">
                <Link to={`/plato/${plato.idPlato}`} className="plato-header-link">
                  <div className="plato-header">
                    <h3>{plato.nombre}</h3>
                    <span className="plato-precio">${plato.precio}</span>
                  </div>
                </Link>
                <p className="plato-categoria">{plato.categoriaNombre}</p>
                {plato.descripcion && (
                  <p className="plato-descripcion">{plato.descripcion}</p>
                )}
                <div className="plato-metadata">
                  <span>⏱️ {plato.tiempoPreparacion} min</span>
                  <span className={`disponibilidad ${plato.disponible ? 'disponible' : 'no-disponible'}`}>
                    {plato.disponible ? '✓ Disponible' : '✗ No disponible'}
                  </span>
                </div>
                <div className="plato-acciones">
                  <Link to={`/plato/${plato.idPlato}`} className="btn-ver-detalle">
                    Ver Detalle
                  </Link>
                  {plato.disponible && (
                    <button
                      onClick={() => {
                        agregarAlCarrito(plato, 1);
                        alert('¡Agregado al carrito!');
                      }}
                      className="btn-agregar-menu"
                    >
                      🛒 Agregar
                    </button>
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

export default Menu;
