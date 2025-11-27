import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CarritoProvider } from './context/CarritoContext';
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
import './App.css';

function App() {
  return (
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
          </Routes>
        </Layout>
      </Router>
    </CarritoProvider>
  );
}

export default App;