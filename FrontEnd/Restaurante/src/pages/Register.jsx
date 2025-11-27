import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { usuariosService } from '../services/api';
import './Auth.css';

function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    nombre: '',
    apellido: '',
    email: '',
    telefono: '',
    contrasena: '',
    confirmarContrasena: '',
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

    if (formData.contrasena !== formData.confirmarContrasena) {
      setError('Las contraseñas no coinciden');
      return;
    }

    if (formData.contrasena.length < 6) {
      setError('La contraseña debe tener al menos 6 caracteres');
      return;
    }

    setLoading(true);

    try {
      // Verificar si el email ya existe
      const usuarios = await usuariosService.getAll();
      const emailExiste = usuarios.some(u => u.email === formData.email);

      if (emailExiste) {
        setError('Este email ya está registrado');
        setLoading(false);
        return;
      }

      // Crear usuario
      const nuevoUsuario = {
        nombre: formData.nombre,
        apellido: formData.apellido,
        email: formData.email,
        telefono: formData.telefono || null,
        contrasena: formData.contrasena,
      };

      const usuarioCreado = await usuariosService.create(nuevoUsuario);

      // Guardar usuario en localStorage
      const usuarioLogin = {
        idUsuario: usuarioCreado.idUsuario,
        nombre: usuarioCreado.nombre,
        apellido: usuarioCreado.apellido,
        email: usuarioCreado.email,
      };

      localStorage.setItem('usuario', JSON.stringify(usuarioLogin));
      
      // Redirigir
      navigate('/');
      
    } catch (err) {
      console.error('Error en registro:', err);
      if (err.response?.status === 500) {
        setError('Error del servidor. Por favor verifica que la base de datos esté conectada y vuelve a intentar.');
      } else if (err.response?.status === 404 || !err.response) {
        setError('No se puede conectar con el servidor. Verifica que el backend esté corriendo.');
      } else {
        setError(err.response?.data?.message || 'Error al registrar. Por favor intenta de nuevo.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-container">
        <h1>Crear Cuenta</h1>
        <p className="auth-subtitle">Únete a nuestro restaurante</p>

        <form onSubmit={handleSubmit} className="auth-form">
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="nombre">Nombre</label>
              <input
                type="text"
                id="nombre"
                name="nombre"
                value={formData.nombre}
                onChange={handleChange}
                required
                placeholder="Juan"
              />
            </div>

            <div className="form-group">
              <label htmlFor="apellido">Apellido</label>
              <input
                type="text"
                id="apellido"
                name="apellido"
                value={formData.apellido}
                onChange={handleChange}
                required
                placeholder="Pérez"
              />
            </div>
          </div>

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
            <label htmlFor="telefono">Teléfono (Opcional)</label>
            <input
              type="tel"
              id="telefono"
              name="telefono"
              value={formData.telefono}
              onChange={handleChange}
              placeholder="+1234567890"
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
              minLength="6"
              placeholder="Mínimo 6 caracteres"
            />
          </div>

          <div className="form-group">
            <label htmlFor="confirmarContrasena">Confirmar Contraseña</label>
            <input
              type="password"
              id="confirmarContrasena"
              name="confirmarContrasena"
              value={formData.confirmarContrasena}
              onChange={handleChange}
              required
              placeholder="Repite tu contraseña"
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
            {loading ? 'Creando cuenta...' : 'Registrarse'}
          </button>
        </form>

        <p className="auth-footer">
          ¿Ya tienes una cuenta? <Link to="/login">Inicia sesión aquí</Link>
        </p>
      </div>
    </div>
  );
}

export default Register;
