-- =====================================================
-- Datos Iniciales para el Sistema
-- =====================================================

USE `RestauranteDisponibilidad`;

-- Insertar Categorías de Platos
INSERT INTO `categoriaplato` (`Nombre`, `Descripcion`) VALUES
('Entradas', 'Platos de entrada y aperitivos'),
('Principales', 'Platos principales'),
('Guarniciones', 'Acompañamientos'),
('Postres', 'Postres y dulces'),
('Bebidas', 'Bebidas y refrescos'),
('Menú del Día', 'Platos especiales del día');

-- Insertar Estados de Pedido
INSERT INTO `estadopedido` (`Nombre`, `Descripcion`) VALUES
('Pendiente', 'Pedido recibido, esperando confirmación'),
('Confirmado', 'Pedido confirmado, esperando preparación'),
('En Preparación', 'Pedido en proceso de preparación'),
('Listo', 'Pedido listo para entregar'),
('Entregado', 'Pedido entregado al cliente'),
('Cancelado', 'Pedido cancelado');

-- Insertar Métodos de Pago
INSERT INTO `metodopago` (`TipoMedioPago`) VALUES
('Efectivo'),
('Tarjeta de Débito'),
('Tarjeta de Crédito'),
('Transferencia'),
('Mercado Pago'),
('Billetera Virtual');

-- Insertar Horarios del Restaurante (Ejemplo: Lunes a Domingo)
INSERT INTO `horariorestaurante` (`DiaSemana`, `HoraApertura`, `HoraCierre`, `Activo`) VALUES
(0, '11:00:00', '23:00:00', TRUE), -- Domingo
(1, '11:00:00', '23:00:00', TRUE), -- Lunes
(2, '11:00:00', '23:00:00', TRUE), -- Martes
(3, '11:00:00', '23:00:00', TRUE), -- Miércoles
(4, '11:00:00', '23:00:00', TRUE), -- Jueves
(5, '11:00:00', '00:00:00', TRUE), -- Viernes
(6, '11:00:00', '00:00:00', TRUE); -- Sábado

-- Insertar Mesas
INSERT INTO `mesa` (`NumeroMesa`, `Capacidad`, `Activa`) VALUES
('1', 2, TRUE),
('2', 2, TRUE),
('3', 4, TRUE),
('4', 4, TRUE),
('5', 6, TRUE),
('6', 6, TRUE),
('7', 8, TRUE);

-- Insertar Platos (Basados en tu ejemplo)
INSERT INTO `plato` (`idCategoria`, `Nombre`, `Descripcion`, `Precio`, `TiempoPreparacion`, `Disponible`, `EsMenuDelDia`) VALUES
(2, 'Papas a la española', 'Papas cortadas en cubos con cebolla, pimientos y especias', 850.00, 10, TRUE, FALSE),
(2, 'Ravioles', 'Ravioles caseros con salsa a elección', 1200.00, 7, TRUE, FALSE),
(2, 'Pollo al verdeo con crema', 'Pechuga de pollo con salsa de verdeo y crema', 1450.00, 12, TRUE, FALSE),
(1, 'Empanadas', 'Empanadas de carne, pollo o jamón y queso (por unidad)', 250.00, 3, TRUE, FALSE),
(3, 'Ensalada césar', 'Lechuga, pollo, crutones, parmesano y aderezo césar', 950.00, 5, FALSE, FALSE);

-- Insertar Ingredientes Críticos
INSERT INTO `ingrediente` (`Nombre`, `UnidadMedida`, `StockActual`, `StockMinimo`, `EsCritico`) VALUES
('Papas', 'kg', 50.00, 10.00, TRUE),
('Cebolla', 'kg', 20.00, 5.00, TRUE),
('Masa de ravioles', 'kg', 15.00, 3.00, TRUE),
('Pechuga de pollo', 'kg', 25.00, 5.00, TRUE),
('Crema de leche', 'litros', 10.00, 2.00, TRUE),
('Verdeo', 'kg', 5.00, 1.00, TRUE),
('Masa de empanadas', 'unidades', 100.00, 20.00, TRUE),
('Lechuga', 'kg', 8.00, 2.00, TRUE),
('Parmesano', 'kg', 3.00, 0.50, TRUE),
('Crutones', 'kg', 2.00, 0.50, TRUE);

-- Relacionar Platos con Ingredientes
-- Papas a la española
INSERT INTO `platoingrediente` (`idPlato`, `idIngrediente`, `CantidadNecesaria`) VALUES
(1, 1, 0.30), -- 300g de papas
(1, 2, 0.10); -- 100g de cebolla

-- Ravioles
INSERT INTO `platoingrediente` (`idPlato`, `idIngrediente`, `CantidadNecesaria`) VALUES
(2, 3, 0.25); -- 250g de masa de ravioles

-- Pollo al verdeo con crema
INSERT INTO `platoingrediente` (`idPlato`, `idIngrediente`, `CantidadNecesaria`) VALUES
(3, 4, 0.25), -- 250g de pechuga de pollo
(3, 5, 0.15), -- 150ml de crema
(3, 6, 0.05); -- 50g de verdeo

-- Empanadas
INSERT INTO `platoingrediente` (`idPlato`, `idIngrediente`, `CantidadNecesaria`) VALUES
(4, 7, 1.00); -- 1 unidad de masa

-- Ensalada césar
INSERT INTO `platoingrediente` (`idPlato`, `idIngrediente`, `CantidadNecesaria`) VALUES
(5, 8, 0.15), -- 150g de lechuga
(5, 4, 0.10), -- 100g de pollo
(5, 9, 0.03), -- 30g de parmesano
(5, 10, 0.02); -- 20g de crutones

-- Configuraciones del Sistema
INSERT INTO `configuracionrestaurante` (`Clave`, `Valor`, `Descripcion`) VALUES
('MINUTOS_ANTES_CIERRE_NO_PEDIDOS', '30', 'Minutos antes del cierre en los que no se aceptan pedidos'),
('TIEMPO_MAXIMO_PREORDEN_DIAS', '7', 'Días máximos de anticipación para pre-órdenes'),
('CAPACIDAD_MAXIMA_COCINA', '20', 'Cantidad máxima de platos en preparación simultánea'),
('PORCENTAJE_SENA_RESERVA', '20', 'Porcentaje de seña requerido para reservas');

-- Usuario de prueba (contraseña: "test123" hasheada con SHA-256)
INSERT INTO `usuario` (`Nombre`, `Apellido`, `Email`, `Telefono`, `Contrasena`) VALUES
('Juan', 'Pérez', 'juan.perez@example.com', '+54 9 11 1234-5678', 'ecd71870d1963316a97e3ac3408c9835ad8cf0f3c1bc703527c30265534f75ae');
