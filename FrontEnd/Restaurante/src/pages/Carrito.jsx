import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useCarrito } from '../context/CarritoContext';
import { pedidosService, mesasService, metodosPagoService } from '../services/api';
import './Carrito.css';

function Carrito() {
  const navigate = useNavigate();
  const { carrito, removerDelCarrito, actualizarCantidad, limpiarCarrito, obtenerTotal } = useCarrito();
  const [usuario, setUsuario] = useState(null);
  const [mesas, setMesas] = useState([]);
  const [metodosPago, setMetodosPago] = useState([]);
  const [idMetodoPago, setIdMetodoPago] = useState(null);
  const [idMesa, setIdMesa] = useState(null);
  const [comentarios, setComentarios] = useState('');
  const [esPreOrden, setEsPreOrden] = useState(false);
  const [fechaPreOrden, setFechaPreOrden] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (!usuarioGuardado) {
      navigate('/login');
      return;
    }
    setUsuario(JSON.parse(usuarioGuardado));

    const cargarDatos = async () => {
      try {
        const [mesasData, metodosPagoData] = await Promise.all([
          mesasService.getAll(),
          metodosPagoService.getAll(),
        ]);
        setMesas(mesasData.filter(m => m.activa));
        setMetodosPago(metodosPagoData);
        if (metodosPagoData.length > 0) {
          setIdMetodoPago(metodosPagoData[0].idMetodoPago);
        }
      } catch (error) {
        console.error('Error cargando datos:', error);
      }
    };
    cargarDatos();
  }, [navigate]);

  const handlePedido = async () => {
    if (carrito.length === 0) {
      setError('El carrito está vacío');
      return;
    }

    if (!usuario) {
      navigate('/login');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const detalles = carrito.map(item => ({
        idPlato: item.idPlato,
        cantidad: item.cantidad,
        comentarios: null,
      }));

      const pedidoData = {
        idUsuario: usuario.idUsuario,
        idReserva: null,
        idMetodoPago: idMetodoPago || null,
        esPreOrden: esPreOrden,
        comentarios: comentarios || null,
        detalles: detalles,
      };

      await pedidosService.create(pedidoData);
      
      limpiarCarrito();
      navigate('/mis-pedidos');
    } catch (error) {
      console.error('Error creando pedido:', error);
      setError(error.response?.data?.message || 'Error al crear el pedido. Por favor intenta de nuevo.');
    } finally {
      setLoading(false);
    }
  };

  if (carrito.length === 0) {
    return (
      <div className="carrito-page">
        <h1>Carrito de Compras</h1>
        <div className="carrito-vacio">
          <p>Tu carrito está vacío</p>
          <button onClick={() => navigate('/menu')} className="btn-ir-menu">
            Ver Menú
          </button>
        </div>
      </div>
    );
  }

  const total = obtenerTotal();

  return (
    <div className="carrito-page">
      <h1>Carrito de Compras</h1>

      <div className="carrito-container">
        <div className="carrito-items">
          <h2>Tu Pedido</h2>
          {carrito.map((item) => (
            <div key={item.idPlato} className="carrito-item">
              <div className="item-info">
                <h3>{item.nombre}</h3>
                <p className="item-precio">${item.precio} c/u</p>
              </div>
              <div className="item-controls">
                <button
                  onClick={() => actualizarCantidad(item.idPlato, item.cantidad - 1)}
                  className="btn-cantidad"
                >
                  -
                </button>
                <span className="cantidad">{item.cantidad}</span>
                <button
                  onClick={() => actualizarCantidad(item.idPlato, item.cantidad + 1)}
                  className="btn-cantidad"
                >
                  +
                </button>
                <button
                  onClick={() => removerDelCarrito(item.idPlato)}
                  className="btn-eliminar"
                >
                  🗑️
                </button>
              </div>
              <div className="item-subtotal">
                Subtotal: ${(item.precio * item.cantidad).toFixed(2)}
              </div>
            </div>
          ))}
        </div>

        <div className="carrito-checkout">
          <h2>Finalizar Pedido</h2>

          <div className="form-group">
            <label>
              <input
                type="checkbox"
                checked={esPreOrden}
                onChange={(e) => setEsPreOrden(e.target.checked)}
              />
              Es una pre-orden
            </label>
            {esPreOrden && (
              <input
                type="datetime-local"
                value={fechaPreOrden}
                onChange={(e) => setFechaPreOrden(e.target.value)}
                className="input-fecha"
              />
            )}
          </div>

          <div className="form-group">
            <label>Método de Pago</label>
            <select
              value={idMetodoPago || ''}
              onChange={(e) => setIdMetodoPago(e.target.value ? parseInt(e.target.value) : null)}
            >
              {metodosPago.length === 0 ? (
                <option value="">Cargando métodos...</option>
              ) : (
                metodosPago.map((metodo) => (
                  <option key={metodo.idMetodoPago} value={metodo.idMetodoPago}>
                    {metodo.tipoMedioPago}
                  </option>
                ))
              )}
            </select>
          </div>

          <div className="form-group">
            <label>Mesa (Opcional)</label>
            <select
              value={idMesa || ''}
              onChange={(e) => setIdMesa(e.target.value ? parseInt(e.target.value) : null)}
            >
              <option value="">Sin mesa específica</option>
              {mesas.map((mesa) => (
                <option key={mesa.idMesa} value={mesa.idMesa}>
                  Mesa {mesa.numeroMesa} - {mesa.capacidad} personas
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Comentarios</label>
            <textarea
              value={comentarios}
              onChange={(e) => setComentarios(e.target.value)}
              rows="3"
              placeholder="Instrucciones especiales, alergias, etc."
            />
          </div>

          <div className="resumen-total">
            <div className="total-line">
              <span>Subtotal:</span>
              <span>${total.toFixed(2)}</span>
            </div>
            <div className="total-line total-final">
              <span>Total:</span>
              <span>${total.toFixed(2)}</span>
            </div>
          </div>

          {error && <div className="error-message">{error}</div>}

          <button
            onClick={handlePedido}
            disabled={loading}
            className="btn-realizar-pedido"
          >
            {loading ? 'Procesando...' : 'Realizar Pedido'}
          </button>
        </div>
      </div>
    </div>
  );
}

export default Carrito;
