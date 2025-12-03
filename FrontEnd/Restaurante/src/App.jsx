import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CarritoProvider } from './context/CarritoContext';
import { UsuarioProvider } from './context/UsuarioContext';
import Layout from './components/Layout';
import Home from './pages/Home';
import Menu from './pages/Menu';
import DetallePlato from './pages/DetallePlato';
import Reservas from './pages/Reservas';
import MisReservas from './pages/MisReservas';
import Carrito from './pages/Carrito';
import MisPedidos from './pages/MisPedidos';
import Login from './pages/Login';
import Register from './pages/Register';
import AdminDashboard from './pages/AdminDashboard';
import AdminPedidos from './pages/AdminPedidos';
import AdminMesas from './pages/AdminMesas';
import AdminPlatos from './pages/AdminPlatos';
import './App.css';

function App() {
  return (
    <UsuarioProvider>
      <CarritoProvider>
        <Router>
          <Layout>
            <Routes>
              <Route path="/" element={<Home />} />
              <Route path="/menu" element={<Menu />} />
              <Route path="/plato/:id" element={<DetallePlato />} />
              <Route path="/reservas" element={<Reservas />} />
              <Route path="/mis-reservas" element={<MisReservas />} />
              <Route path="/carrito" element={<Carrito />} />
              <Route path="/mis-pedidos" element={<MisPedidos />} />
              <Route path="/login" element={<Login />} />
              <Route path="/register" element={<Register />} />
              <Route path="/admin" element={<AdminDashboard />} />
              <Route path="/admin/pedidos" element={<AdminPedidos />} />
              <Route path="/admin/mesas" element={<AdminMesas />} />
              <Route path="/admin/platos" element={<AdminPlatos />} />
            </Routes>
          </Layout>
        </Router>
      </CarritoProvider>
    </UsuarioProvider>
  );
}

export default App;