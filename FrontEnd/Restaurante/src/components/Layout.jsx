import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useCarrito } from '../context/CarritoContext';
import { useUsuario } from '../context/UsuarioContext';
import './Layout.css';

function Layout({ children }) {
  const navigate = useNavigate();
  const { obtenerCantidadTotal } = useCarrito();
  const { usuario, logout } = useUsuario();
  const cantidadCarrito = obtenerCantidadTotal();
  const [menuAbierto, setMenuAbierto] = useState(false);

  const handleLogout = () => {
    logout();
    navigate('/');
    setMenuAbierto(false);
  };

  const cerrarMenu = () => {
    setMenuAbierto(false);
  };

  return (
    <div className="layout">
      <header className="header">
        <div className="header-content">
          <Link to="/" className="logo" onClick={cerrarMenu}>
            <h1>Restaurante</h1>
          </Link>
          <button 
            className="hamburguesa" 
            onClick={() => setMenuAbierto(!menuAbierto)}
            aria-label="Abrir menú"
          >
            <span></span>
            <span></span>
            <span></span>
          </button>
          <nav className={`nav ${menuAbierto ? 'abierto' : ''}`}>
            <Link to="/" onClick={cerrarMenu}>Inicio</Link>
            <Link to="/menu" onClick={cerrarMenu}>Menú</Link>
            <Link to="/reservas" onClick={cerrarMenu}>Reservar</Link>
            {usuario ? (
              <>
                <Link to="/carrito" className="carrito-link" onClick={cerrarMenu}>
                  Carrito
                  {cantidadCarrito > 0 && (
                    <span className="carrito-badge">{cantidadCarrito}</span>
                  )}
                </Link>
                <Link to="/mis-pedidos" onClick={cerrarMenu}>Mis Pedidos</Link>
                <Link to="/mis-reservas" onClick={cerrarMenu}>Mis Reservas</Link>
                <div className="user-menu">
                  <span>{usuario.nombre} {usuario.apellido}</span>
                  <button onClick={handleLogout} className="btn-logout">
                    Salir
                  </button>
                </div>
              </>
            ) : (
              <>
                <Link to="/login" onClick={cerrarMenu}>Iniciar Sesión</Link>
                <Link to="/register" onClick={cerrarMenu}>
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
