-- Insertar usuario admin de prueba
INSERT INTO `usuario` (`Nombre`, `Apellido`, `Email`, `Telefono`, `Contrasena`, `FechaRegistro`, `Activo`, `Rol`)
VALUES ('Admin', 'Sistema', 'admin@restaurante.com', '1234567890', SHA2('admin123', 256), NOW(), TRUE, 'admin');

-- Insertar estados de pedido si no existen
INSERT IGNORE INTO `estadopedido` (`Nombre`, `Descripcion`)
VALUES 
('Pendiente', 'Pedido pendiente de preparación'),
('En Preparación', 'Pedido en proceso de preparación'),
('Listo', 'Pedido listo para entregar'),
('Entregado', 'Pedido entregado al cliente'),
('Cancelado', 'Pedido cancelado');

-- Insertar método de pago si no existe
INSERT IGNORE INTO `Metodopago` (`TipoMedioPago`, `Activo`)
VALUES ('Efectivo', TRUE),
       ('Tarjeta', TRUE),
       ('Transferencia', TRUE);
