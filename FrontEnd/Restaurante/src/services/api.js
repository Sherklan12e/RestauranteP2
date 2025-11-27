import axios from 'axios';

const API_BASE_URL = 'http://localhost:5142/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Servicios de Platos
export const platosService = {
  getAll: async () => {
    const response = await api.get('/platos');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/platos/${id}`);
    return response.data;
  },
};

// Servicios de Categorías
export const categoriasService = {
  getAll: async () => {
    const response = await api.get('/categoriaplato');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/categoriaplato/${id}`);
    return response.data;
  },
};

// Servicios de Reservas
export const reservasService = {
  getAll: async () => {
    const response = await api.get('/reservas');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/reservas/${id}`);
    return response.data;
  },
  getByUsuario: async (usuarioId) => {
    const response = await api.get(`/reservas/usuario/${usuarioId}`);
    return response.data;
  },
  create: async (reserva) => {
    const response = await api.post('/reservas', reserva);
    return response.data;
  },
  updateEstado: async (id, estado) => {
    // El backend espera un string como JSON
    const response = await api.put(`/reservas/${id}/estado`, `"${estado}"`, {
      headers: { 'Content-Type': 'application/json' },
    });
    return response.data;
  },
  delete: async (id) => {
    await api.delete(`/reservas/${id}`);
  },
};

// Servicios de Usuarios
export const usuariosService = {
  getAll: async () => {
    const response = await api.get('/usuarios');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/usuarios/${id}`);
    return response.data;
  },
  create: async (usuario) => {
    const response = await api.post('/usuarios', usuario);
    return response.data;
  },
  update: async (id, usuario) => {
    await api.put(`/usuarios/${id}`, usuario);
  },
};

// Servicios de Mesas
export const mesasService = {
  getAll: async () => {
    const response = await api.get('/mesas');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/mesas/${id}`);
    return response.data;
  },
};

// Servicios de Pedidos
export const pedidosService = {
  getAll: async () => {
    const response = await api.get('/pedidos');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/pedidos/${id}`);
    return response.data;
  },
  getByUsuario: async (usuarioId) => {
    const response = await api.get(`/pedidos/usuario/${usuarioId}`);
    return response.data;
  },
  create: async (pedido) => {
    const response = await api.post('/pedidos', pedido);
    return response.data;
  },
  updateEstado: async (id, estado) => {
    const response = await api.put(`/pedidos/${id}/estado`, `"${estado}"`, {
      headers: { 'Content-Type': 'application/json' },
    });
    return response.data;
  },
  delete: async (id) => {
    await api.delete(`/pedidos/${id}`);
  },
};

// Servicios de Calificaciones
export const calificacionesService = {
  getAll: async () => {
    const response = await api.get('/calificaciones');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/calificaciones/${id}`);
    return response.data;
  },
  getByPlato: async (platoId) => {
    const response = await api.get(`/calificaciones/plato/${platoId}`);
    return response.data;
  },
  getByUsuario: async (usuarioId) => {
    const response = await api.get(`/calificaciones/usuario/${usuarioId}`);
    return response.data;
  },
  create: async (calificacion) => {
    const response = await api.post('/calificaciones', calificacion);
    return response.data;
  },
  update: async (id, calificacion) => {
    await api.put(`/calificaciones/${id}`, calificacion);
  },
  delete: async (id) => {
    await api.delete(`/calificaciones/${id}`);
  },
};

// Servicios de Métodos de Pago
export const metodosPagoService = {
  getAll: async () => {
    const response = await api.get('/metodospago');
    return response.data;
  },
  getById: async (id) => {
    const response = await api.get(`/metodospago/${id}`);
    return response.data;
  },
};

// Servicios de Admin
export const adminService = {
  // Pedidos
  getTodosPedidos: async (usuarioId) => {
    const response = await api.get('/admin/pedidos', { params: { usuarioId } });
    return response.data;
  },
  actualizarEstadoPedido: async (pedidoId, usuarioId, nuevoEstado) => {
    await api.put(`/admin/pedidos/${pedidoId}/estado`, { nuevoEstado }, { params: { usuarioId } });
  },

  // Reservas
  getTodasReservas: async (usuarioId) => {
    const response = await api.get('/admin/reservas', { params: { usuarioId } });
    return response.data;
  },
  actualizarEstadoReserva: async (reservaId, usuarioId, nuevoEstado) => {
    await api.put(`/admin/reservas/${reservaId}/estado`, { nuevoEstado }, { params: { usuarioId } });
  },

  // Usuarios
  getTodosUsuarios: async (usuarioId) => {
    const response = await api.get('/admin/usuarios', { params: { usuarioId } });
    return response.data;
  },
  cambiarRolUsuario: async (usuarioTargetId, usuarioId, nuevoRol) => {
    await api.put(`/admin/usuarios/${usuarioTargetId}/rol`, { nuevoRol }, { params: { usuarioId } });
  },
  cambiarEstadoUsuario: async (usuarioTargetId, usuarioId, activo) => {
    await api.put(`/admin/usuarios/${usuarioTargetId}/estado`, { activo }, { params: { usuarioId } });
  },

  // Estadísticas
  getEstadisticas: async (usuarioId) => {
    const response = await api.get('/admin/estadisticas', { params: { usuarioId } });
    return response.data;
  },

  // Mesas
  getTodasMesas: async (usuarioId) => {
    const response = await api.get('/admin/mesas', { params: { usuarioId } });
    return response.data;
  },
  cambiarEstadoMesa: async (mesaId, usuarioId, activa) => {
    await api.put(`/admin/mesas/${mesaId}/estado`, { activo: activa }, { params: { usuarioId } });
  },

  // Platos
  getTodosPlatos: async (usuarioId) => {
    const response = await api.get('/admin/platos', { params: { usuarioId } });
    return response.data;
  },
  cambiarDisponibilidadPlato: async (platoId, usuarioId, disponible) => {
    await api.put(`/admin/platos/${platoId}/disponibilidad`, { activo: disponible }, { params: { usuarioId } });
  },
  eliminarPlato: async (platoId, usuarioId) => {
    await api.delete(`/admin/platos/${platoId}`, { params: { usuarioId } });
  },
};

export default api;
