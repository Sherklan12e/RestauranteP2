import { createContext, useContext, useState, useEffect } from 'react';

const CarritoContext = createContext();

export const useCarrito = () => {
  const context = useContext(CarritoContext);
  if (!context) {
    throw new Error('useCarrito debe usarse dentro de CarritoProvider');
  }
  return context;
};

export const CarritoProvider = ({ children }) => {
  const [carrito, setCarrito] = useState([]);

  // Cargar carrito del localStorage al iniciar
  useEffect(() => {
    const carritoGuardado = localStorage.getItem('carrito');
    if (carritoGuardado) {
      try {
        setCarrito(JSON.parse(carritoGuardado));
      } catch (error) {
        console.error('Error cargando carrito:', error);
      }
    }
  }, []);

  // Guardar carrito en localStorage cuando cambia
  useEffect(() => {
    localStorage.setItem('carrito', JSON.stringify(carrito));
  }, [carrito]);

  const agregarAlCarrito = (plato, cantidad = 1) => {
    setCarrito(prev => {
      const existe = prev.find(item => item.idPlato === plato.idPlato);
      if (existe) {
        return prev.map(item =>
          item.idPlato === plato.idPlato
            ? { ...item, cantidad: item.cantidad + cantidad }
            : item
        );
      }
      return [...prev, { ...plato, cantidad }];
    });
  };

  const removerDelCarrito = (idPlato) => {
    setCarrito(prev => prev.filter(item => item.idPlato !== idPlato));
  };

  const actualizarCantidad = (idPlato, cantidad) => {
    if (cantidad <= 0) {
      removerDelCarrito(idPlato);
      return;
    }
    setCarrito(prev =>
      prev.map(item =>
        item.idPlato === idPlato ? { ...item, cantidad } : item
      )
    );
  };

  const limpiarCarrito = () => {
    setCarrito([]);
    localStorage.removeItem('carrito');
  };

  const obtenerTotal = () => {
    return carrito.reduce((total, item) => {
      return total + (item.precio * item.cantidad);
    }, 0);
  };

  const obtenerCantidadTotal = () => {
    return carrito.reduce((total, item) => total + item.cantidad, 0);
  };

  const value = {
    carrito,
    agregarAlCarrito,
    removerDelCarrito,
    actualizarCantidad,
    limpiarCarrito,
    obtenerTotal,
    obtenerCantidadTotal,
  };

  return (
    <CarritoContext.Provider value={value}>
      {children}
    </CarritoContext.Provider>
  );
};
