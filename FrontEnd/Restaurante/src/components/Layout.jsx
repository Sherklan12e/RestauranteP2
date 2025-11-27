import { Link, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useCarrito } from '../context/CarritoContext';
import './Layout.css';

function Layout({ children }) {
  const [usuario, setUsuario] = useState(null);
  const navigate = useNavigate();
  const { obtenerCantidadTotal } = useCarrito();
  const cantidadCarrito = obtenerCantidadTotal();

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      setUsuario(JSON.parse(usuarioGuardado));
    }
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('usuario');
    setUsuario(null);
    navigate('/');
  };

  return (
    <div className="layout">
      <header className="header">
        <div className="container">
          <Link to="/" className="logo">
            <h1>🍽️ Restaurante</h1>
          </Link>
          <nav className="nav">
            <Link to="/">Inicio</Link>
            <Link to="/menu">Menú</Link>
            <Link to="/reservas">Reservar</Link>
            {usuario ? (
              <>
                <Link to="/carrito" className="carrito-link">
                  🛒 Carrito
                  {cantidadCarrito > 0 && (
                    <span className="carrito-badge">{cantidadCarrito}</span>
                  )}
                </Link>
                <Link to="/mis-pedidos">Mis Pedidos</Link>
                <Link to="/mis-reservas">Mis Reservas</Link>
                <div className="user-menu">
                  <span>👤 {usuario.nombre} {usuario.apellido}</span>
                  <button onClick={handleLogout} className="btn-logout">
                    Salir
                  </button>
                </div>
              </>
            ) : (
              <>
                <Link to="/login">Iniciar Sesión</Link>
                <Link to="/register" className="btn-register">
                  Registrarse
                </Link>
              </>
            )}
          </nav>
        </div>
      </header>
      <main className="main-content">
        {children}
      </main>
      <footer className="footer">
        <div className="container">
          <p>&copy; 2025 Restaurante. Todos los derechos reservados.</p>
        </div>
      </footer>
    </div>
  );
}

export default Layout;
