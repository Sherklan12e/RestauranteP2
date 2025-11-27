import { useState } from 'react';
import { calificacionesService } from '../services/api';
import './CalificarPlato.css';

function CalificarPlato({ platoId, usuarioId, onCalificacionCreada }) {
  const [puntuacion, setPuntuacion] = useState(5);
  const [comentario, setComentario] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [exito, setExito] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await calificacionesService.create({
        idPlato: platoId,
        idUsuario: usuarioId,
        puntuacion: puntuacion,
        comentario: comentario || null,
      });

      setExito(true);
      setTimeout(() => {
        if (onCalificacionCreada) {
          onCalificacionCreada();
        }
      }, 1500);
    } catch (error) {
      console.error('Error creando calificación:', error);
      setError(error.response?.data?.message || 'Error al calificar el plato');
    } finally {
      setLoading(false);
    }
  };

  if (exito) {
    return (
      <div className="calificacion-exito">
        <p>¡Calificación enviada exitosamente! ⭐</p>
      </div>
    );
  }

  return (
    <div className="calificar-plato">
      <h3>Calificar este plato</h3>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Puntuación (1-5 estrellas)</label>
          <div className="estrellas-selector">
            {[1, 2, 3, 4, 5].map((valor) => (
              <button
                key={valor}
                type="button"
                className={`estrella-btn ${puntuacion >= valor ? 'activa' : ''}`}
                onClick={() => setPuntuacion(valor)}
              >
                ⭐
              </button>
            ))}
            <span className="puntuacion-numero">{puntuacion}/5</span>
          </div>
        </div>

        <div className="form-group">
          <label>Comentario (Opcional)</label>
          <textarea
            value={comentario}
            onChange={(e) => setComentario(e.target.value)}
            rows="4"
            placeholder="Comparte tu experiencia con este plato..."
            maxLength={300}
          />
          <span className="caracteres-restantes">
            {300 - comentario.length} caracteres restantes
          </span>
        </div>

        {error && <div className="error-message">{error}</div>}

        <button type="submit" disabled={loading} className="btn-enviar-calificacion">
          {loading ? 'Enviando...' : 'Enviar Calificación'}
        </button>
      </form>
    </div>
  );
}

export default CalificarPlato;
