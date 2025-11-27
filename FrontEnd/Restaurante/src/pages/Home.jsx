import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { platosService } from '../services/api';
import { useCarrito } from '../context/CarritoContext';
import './Home.css';

function Home() {
  const { agregarAlCarrito } = useCarrito();
  const [platosDestacados, setPlatosDestacados] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const cargarPlatosDestacados = async () => {
      try {
        const platos = await platosService.getAll();
        const destacados = platos
          .filter(p => p.esMenuDelDia || p.disponible)
          .slice(0, 6);
        setPlatosDestacados(destacados);
      } catch (error) {
        console.error('Error cargando platos:', error);
        // Mostrar mensaje de error al usuario
        if (error.response?.status === 500) {
          console.error('Error 500: El servidor no puede conectarse a la base de datos.');
        }
      } finally {
        setLoading(false);
      }
    };
    cargarPlatosDestacados();
  }, []);

  return (
    <div className="home">
      <section className="hero">
        <div className="hero-content">
          <h1>Bienvenido a nuestro Restaurante</h1>
          <p>Disfruta de los mejores platos con ingredientes frescos y de calidad</p>
          <div className="hero-buttons">
            <Link to="/menu" className="btn btn-primary">
              Ver Menú
            </Link>
            <Link to="/reservas" className="btn btn-secondary">
              Hacer Reserva
            </Link>
          </div>
        </div>
      </section>

      <section className="destacados">
        <h2>Platos Destacados</h2>
        {loading ? (
          <div className="loading">Cargando platos...</div>
        ) : platosDestacados.length > 0 ? (
          <div className="platos-grid">
            {platosDestacados.map((plato) => (
              <div key={plato.idPlato} className="plato-card">
                {plato.imagenUrl && (
                  <img src={plato.imagenUrl} alt={plato.nombre} />
                )}
                <div className="plato-info">
                  <Link to={`/plato/${plato.idPlato}`} className="plato-title-link">
                    <h3>{plato.nombre}</h3>
                  </Link>
                  <p className="categoria">{plato.categoriaNombre}</p>
                  <p className="descripcion">{plato.descripcion}</p>
                  <div className="plato-footer">
                    <span className="precio">${plato.precio}</span>
                    {plato.esMenuDelDia && (
                      <span className="badge">Menú del Día</span>
                    )}
                  </div>
                  <div className="plato-acciones-home">
                    <Link to={`/plato/${plato.idPlato}`} className="btn-ver-mas">
                      Ver Más
                    </Link>
                    {plato.disponible && (
                      <button
                        onClick={() => {
                          agregarAlCarrito(plato, 1);
                          alert('¡Agregado al carrito!');
                        }}
                        className="btn-agregar-home"
                      >
                        🛒
                      </button>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <p>No hay platos disponibles en este momento.</p>
        )}
      </section>

      <section className="features">
        <div className="feature">
          <div className="feature-icon">🍴</div>
          <h3>Menú Variado</h3>
          <p>Platos exquisitos para todos los gustos</p>
        </div>
        <div className="feature">
          <div className="feature-icon">⏰</div>
          <h3>Horario Flexible</h3>
          <p>Reserva cuando mejor te convenga</p>
        </div>
        <div className="feature">
          <div className="feature-icon">👨‍🍳</div>
          <h3>Chefs Profesionales</h3>
          <p>Cocina de alta calidad</p>
        </div>
      </section>
    </div>
  );
}

export default Home;
