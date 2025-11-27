import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { usuariosService } from '../services/api';
import './Auth.css';

function Login() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    email: '',
    contrasena: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      // Buscar usuario por email
      const usuarios = await usuariosService.getAll();
      const usuario = usuarios.find(u => u.email === formData.email);

      if (!usuario) {
        setError('Email o contraseña incorrectos');
        setLoading(false);
        return;
      }

      // En una app real, esto se haría con autenticación JWT
      // Por ahora, guardamos el usuario en localStorage
      const usuarioLogin = {
        idUsuario: usuario.idUsuario,
        nombre: usuario.nombre,
        apellido: usuario.apellido,
        email: usuario.email,
        rol: usuario.rol,
      };
      
      console.log(usuarioLogin)  
      localStorage.setItem('usuario', JSON.stringify(usuarioLogin));
      console.log(usuarioLogin)
      // Redirigir según el rol
      if (usuario.rol === 'admin') {
        navigate('/admin');
      } else {
        navigate('/');
      }
      
    } catch (err) {
      console.error('Error en login:', err);
      if (err.response?.status === 500) {
        setError('Error del servidor. Por favor verifica que la base de datos esté conectada y vuelve a intentar.');
      } else if (err.response?.status === 404 || !err.response) {
        setError('No se puede conectar con el servidor. Verifica que el backend esté corriendo.');
      } else {
        setError(err.response?.data?.message || 'Error al iniciar sesión. Por favor intenta de nuevo.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-container">
        <h1>Iniciar Sesión</h1>
        <p className="auth-subtitle">Ingresa a tu cuenta</p>

        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              placeholder="tu@email.com"
            />
          </div>

          <div className="form-group">
            <label htmlFor="contrasena">Contraseña</label>
            <input
              type="password"
              id="contrasena"
              name="contrasena"
              value={formData.contrasena}
              onChange={handleChange}
              required
              placeholder="••••••••"
            />
          </div>

          {error && (
            <div className="error-message">{error}</div>
          )}

          <button 
            type="submit" 
            className="btn-auth"
            disabled={loading}
          >
            {loading ? 'Iniciando sesión...' : 'Iniciar Sesión'}
          </button>
        </form>

        <p className="auth-footer">
          ¿No tienes una cuenta? <Link to="/register">Regístrate aquí</Link>
        </p>
      </div>
    </div>
  );
}

export default Login;
