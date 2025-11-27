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

export default api;
