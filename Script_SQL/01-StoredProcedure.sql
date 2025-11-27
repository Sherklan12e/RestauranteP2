-- =============================================
-- Stored Procedures para Sistema de Disponibilidad
-- =============================================

USE `RestauranteDisponibilidad`;
DELIMITER ;;

-- =============================================
-- Procedimiento: Registrar nuevo pedido
-- =============================================
DROP PROCEDURE IF EXISTS `sp_RegistrarPedido`;;
CREATE PROCEDURE `sp_RegistrarPedido`(
    IN p_UsuarioID INT UNSIGNED,
    IN p_TipoOrden VARCHAR(20),
    IN p_FechaPreorden DATETIME,
    IN p_MetodoPagoID INT UNSIGNED,
    IN p_Observaciones VARCHAR(500),
    OUT p_PedidoID INT UNSIGNED,
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    DECLARE v_RestauranteAbierto BOOLEAN DEFAULT FALSE;
    
    -- Verificar si el restaurante está abierto (solo para pedidos inmediatos)
    IF p_TipoOrden = 'Inmediato' THEN
        SET v_RestauranteAbierto = fn_RestauranteAbierto();
        IF v_RestauranteAbierto = FALSE THEN
            SET p_Mensaje = 'El restaurante está cerrado en este momento';
            SET p_PedidoID = 0;
            LEAVE proc_label;
        END IF;
    END IF;
    
    proc_label: BEGIN
        -- Crear el pedido
        INSERT INTO `Pedido` (`idUsuario`, `FechaHoraPedido`, `EsPreOrden`, `idEstadoPedido`, `idMetodoPago`, `Comentarios`)
        VALUES (p_UsuarioID, NOW(), (p_TipoOrden = 'PreOrden'), 1, p_MetodoPagoID, p_Observaciones);
        
        SET p_PedidoID = LAST_INSERT_ID();
        SET p_Mensaje = 'Pedido registrado exitosamente';
    END proc_label;
END;;

-- =============================================
-- Procedimiento: Agregar detalle a pedido
-- =============================================
DROP PROCEDURE IF EXISTS `sp_AgregarDetallePedido`;;
CREATE PROCEDURE `sp_AgregarDetallePedido`(
    IN p_PedidoID INT UNSIGNED,
    IN p_PlatoID INT UNSIGNED,
    IN p_Cantidad TINYINT UNSIGNED,
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    DECLARE v_PrecioUnitario DECIMAL(10,2) DEFAULT 0;
    DECLARE v_Disponible BOOLEAN DEFAULT FALSE;
    DECLARE v_CantidadDisponible INT DEFAULT 0;
    
    -- Verificar disponibilidad del plato
    SET v_Disponible = fn_VerificarDisponibilidadPlato(p_PlatoID);
    
    IF v_Disponible = FALSE THEN
        SET p_Mensaje = 'El plato no está disponible en este momento';
        LEAVE proc_label;
    END IF;
    
    -- Verificar cantidad disponible
    SET v_CantidadDisponible = fn_CantidadPlatosDisponibles(p_PlatoID);
    
    IF p_Cantidad > v_CantidadDisponible THEN
        SET p_Mensaje = CONCAT('Solo hay ', v_CantidadDisponible, ' unidades disponibles');
        LEAVE proc_label;
    END IF;
    
    proc_label: BEGIN
        -- Obtener precio del plato
        SELECT `Precio` INTO v_PrecioUnitario
        FROM `Plato`
        WHERE `idPlato` = p_PlatoID;
        
        -- Insertar detalle
        INSERT INTO `DetallePedido` (`idPedido`, `idPlato`, `Cantidad`, `PrecioUnitario`, `Subtotal`)
        VALUES (p_PedidoID, p_PlatoID, p_Cantidad, v_PrecioUnitario, v_PrecioUnitario * p_Cantidad);
        
        -- Actualizar total del pedido
        UPDATE `Pedido`
        SET `Total` = (
            SELECT COALESCE(SUM(`Subtotal`), 0)
            FROM `DetallePedido`
            WHERE `idPedido` = p_PedidoID
        )
        WHERE `idPedido` = p_PedidoID;
        
        SET p_Mensaje = 'Plato agregado al pedido exitosamente';
    END proc_label;
END;;

-- =============================================
-- Procedimiento: Confirmar pedido
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ConfirmarPedido`;;
CREATE PROCEDURE `sp_ConfirmarPedido`(
    IN p_PedidoID INT UNSIGNED,
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    DECLARE v_TiempoEstimado DATETIME;
    
    -- Verificar que el pedido existe y está pendiente
    IF NOT EXISTS (
        SELECT 1         FROM `pedido` p
        INNER JOIN `estadopedido` ep ON p.`idEstadoPedido` = ep.`idEstadoPedido`
        WHERE p.`idPedido` = p_PedidoID AND ep.`Nombre` = 'Pendiente'
    ) THEN
        SET p_Mensaje = 'El pedido no existe o ya fue procesado';
        LEAVE proc_label;
    END IF;
    
    proc_label: BEGIN
        -- Calcular tiempo estimado de entrega
        SET v_TiempoEstimado = fn_CalcularTiempoEstimadoEntrega(p_PedidoID);
        
        -- Actualizar estado del pedido (buscar idEstadoPedido para 'Confirmado')
        UPDATE `Pedido`
        SET `idEstadoPedido` = (
            SELECT `idEstadoPedido` FROM `Estadopedido` WHERE `Nombre` = 'Confirmado' LIMIT 1
        ),
        `FechaHoraEntregaEstimada` = v_TiempoEstimado
        WHERE `idPedido` = p_PedidoID;
        
        SET p_Mensaje = CONCAT('Pedido confirmado. Tiempo estimado: ', TIME_FORMAT(v_TiempoEstimado, '%H:%i'));
    END proc_label;
END;;

-- =============================================
-- Procedimiento: Actualizar estado de pedido
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ActualizarEstadoPedido`;;
CREATE PROCEDURE `sp_ActualizarEstadoPedido`(
    IN p_PedidoID INT UNSIGNED,
    IN p_NuevoEstado VARCHAR(45),
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    DECLARE v_EstadoID INT UNSIGNED;
    
    -- Validar estado
    IF p_NuevoEstado NOT IN ('Pendiente', 'Confirmado', 'En Preparación', 'Listo', 'Entregado', 'Cancelado') THEN
        SET p_Mensaje = 'Estado no válido';
        LEAVE proc_label;
    END IF;
    
    proc_label: BEGIN
        -- Obtener ID del estado
        SELECT `idEstadoPedido` INTO v_EstadoID
        FROM `Estadopedido`
        WHERE `Nombre` = p_NuevoEstado
        LIMIT 1;
        
        IF v_EstadoID IS NULL THEN
            SET p_Mensaje = 'Estado no encontrado';
            LEAVE proc_label;
        END IF;
        
        -- Actualizar estado
        UPDATE `Pedido`
        SET `idEstadoPedido` = v_EstadoID
        WHERE `idPedido` = p_PedidoID;
        
        -- Si el pedido está listo, actualizar tiempo real de entrega
        IF p_NuevoEstado = 'Listo' THEN
            UPDATE `Pedido`
            SET `FechaHoraEntregaReal` = NOW()
            WHERE `idPedido` = p_PedidoID;
        END IF;
        
        SET p_Mensaje = CONCAT('Estado actualizado a: ', p_NuevoEstado);
    END proc_label;
END;;

-- =============================================
-- Procedimiento: Actualizar disponibilidad de plato
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ActualizarDisponibilidadPlato`;;
CREATE PROCEDURE `sp_ActualizarDisponibilidadPlato`(
    IN p_PlatoID INT UNSIGNED,
    IN p_Disponible BOOLEAN,
    IN p_Motivo VARCHAR(200),
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    -- Actualizar disponibilidad
    UPDATE `Plato`
    SET `Disponible` = p_Disponible
    WHERE `idPlato` = p_PlatoID;
    
    -- Registrar en historial si se proporciona motivo
    IF p_Motivo IS NOT NULL THEN
        INSERT INTO `Historialdisponibilidad` (`idPlato`, `Disponible`, `Motivo`)
        VALUES (p_PlatoID, p_Disponible, p_Motivo);
    END IF;
    
    SET p_Mensaje = 'Disponibilidad actualizada exitosamente';
END;;

-- =============================================
-- Procedimiento: Actualizar stock de ingrediente
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ActualizarStockIngrediente`;;
CREATE PROCEDURE `sp_ActualizarStockIngrediente`(
    IN p_IngredienteID INT UNSIGNED,
    IN p_NuevoStock DECIMAL(10,2),
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    -- Actualizar stock
    UPDATE `Ingrediente`
    SET `StockActual` = p_NuevoStock
    WHERE `idIngrediente` = p_IngredienteID;
    
    SET p_Mensaje = 'Stock actualizado exitosamente';
END;;

-- =============================================
-- Procedimiento: Registrar reserva
-- =============================================
DROP PROCEDURE IF EXISTS `sp_RegistrarReserva`;;
CREATE PROCEDURE `sp_RegistrarReserva`(
    IN p_UsuarioID INT UNSIGNED,
    IN p_FechaReserva DATETIME,
    IN p_NumeroPersonas TINYINT UNSIGNED,
    IN p_Observaciones VARCHAR(500),
    OUT p_ReservaID INT UNSIGNED,
    OUT p_Mensaje VARCHAR(200)
)
BEGIN
    -- Validar que la fecha sea futura
    IF p_FechaReserva <= NOW() THEN
        SET p_Mensaje = 'La fecha de reserva debe ser futura';
        SET p_ReservaID = 0;
        LEAVE proc_label;
    END IF;
    
    proc_label: BEGIN
        -- Crear reserva
        INSERT INTO `Reserva` (`idUsuario`, `FechaHora`, `CantidadPersonas`, `Estado`, `Comentarios`, `FechaCreacion`)
        VALUES (p_UsuarioID, p_FechaReserva, p_NumeroPersonas, 'Pendiente', p_Observaciones, NOW());
        
        SET p_ReservaID = LAST_INSERT_ID();
        SET p_Mensaje = 'Reserva registrada exitosamente';
    END proc_label;
END;;

-- =============================================
-- Procedimiento: Obtener menú disponible
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ObtenerMenuDisponible`;;
CREATE PROCEDURE `sp_ObtenerMenuDisponible`()
BEGIN
    SELECT 
        p.`idPlato`,
        p.`Nombre`,
        p.`Descripcion`,
        p.`Precio`,
        p.`TiempoPreparacion`,
        cp.`Nombre` AS `Categoria`,
        p.`EsMenuDelDia`,
        p.`Disponible`,
        fn_VerificarDisponibilidadPlato(p.`idPlato`) AS `DisponibleAhora`,
        fn_CantidadPlatosDisponibles(p.`idPlato`) AS `CantidadDisponible`,
        p.`ImagenUrl`
    FROM `plato` p
    INNER JOIN `categoriaplato` cp ON p.`idCategoria` = cp.`idCategoria`
    WHERE p.`Activo` = TRUE
    ORDER BY cp.`Nombre`, p.`Nombre`;
END;;

-- =============================================
-- Procedimiento: Obtener historial de pedidos de usuario
-- =============================================
DROP PROCEDURE IF EXISTS `sp_ObtenerHistorialPedidos`;;
CREATE PROCEDURE `sp_ObtenerHistorialPedidos`(
    IN p_UsuarioID INT UNSIGNED
)
BEGIN
    SELECT 
        p.`idPedido`,
        p.`FechaHoraPedido`,
        p.`EsPreOrden`,
        ep.`Nombre` AS `Estado`,
        p.`Total`,
        p.`FechaHoraEntregaEstimada`,
        mp.`TipoMedioPago` AS `MetodoPago`
    FROM `Pedido` p
    INNER JOIN `Estadopedido` ep ON p.`idEstadoPedido` = ep.`idEstadoPedido`
    LEFT JOIN `Metodopago` mp ON p.`idMetodoPago` = mp.`idMetodoPago`
    WHERE p.`idUsuario` = p_UsuarioID
    ORDER BY p.`FechaHoraPedido` DESC;
END;;

DELIMITER ;