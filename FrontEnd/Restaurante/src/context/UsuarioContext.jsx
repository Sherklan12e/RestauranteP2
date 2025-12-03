import { createContext, useContext, useEffect, useState } from 'react';

const UsuarioContext = createContext(null);

export const useUsuario = () => {
  const ctx = useContext(UsuarioContext);
  if (!ctx) {
    throw new Error('useUsuario debe usarse dentro de UsuarioProvider');
  }
  return ctx;
};

export const UsuarioProvider = ({ children }) => {
  const [usuario, setUsuario] = useState(null);

  useEffect(() => {
    const usuarioGuardado = localStorage.getItem('usuario');
    if (usuarioGuardado) {
      try {
        setUsuario(JSON.parse(usuarioGuardado));
      } catch (error) {
        console.error('Error cargando usuario:', error);
      }
    }
  }, []);

  const login = (usuarioData) => {
    setUsuario(usuarioData);
    localStorage.setItem('usuario', JSON.stringify(usuarioData));
  };

  const logout = () => {
    setUsuario(null);
    localStorage.removeItem('usuario');
  };

  const value = {
    usuario,
    login,
    logout,
    setUsuario,
  };

  return (
    <UsuarioContext.Provider value={value}>
      {children}
    </UsuarioContext.Provider>
  );
};


