-- =============================================
-- Triggers para Sistema de Disponibilidad
-- =============================================

USE `RestauranteDisponibilidad`;
DELIMITER ;;

-- =============================================
-- Trigger: Registrar historial de disponibilidad
-- =============================================
DROP TRIGGER IF EXISTS `trg_RegistrarHistorialDisponibilidad`;;
CREATE TRIGGER `trg_RegistrarHistorialDisponibilidad`
AFTER UPDATE ON `Plato`
FOR EACH ROW
BEGIN
    -- Solo registrar si cambió la disponibilidad
    IF OLD.`Disponible` <> NEW.`Disponible` THEN
        INSERT INTO `Historialdisponibilidad` (`idPlato`, `FechaHora`, `Disponible`, `Motivo`)
        VALUES (
            NEW.`idPlato`,
            NOW(),
            NEW.`Disponible`,
            CASE 
                WHEN NEW.`Disponible` = FALSE THEN 'Plato marcado como no disponible'
                ELSE 'Plato marcado como disponible'
            END
        );
    END IF;
END;;

-- =============================================
-- Trigger: Descontar ingredientes al confirmar pedido
-- =============================================
DROP TRIGGER IF EXISTS `trg_DescontarIngredientes`;;
CREATE TRIGGER `trg_DescontarIngredientes`
AFTER UPDATE ON `Pedido`
FOR EACH ROW
BEGIN
    DECLARE v_EstadoAnterior VARCHAR(45);
    DECLARE v_EstadoNuevo VARCHAR(45);
    
    -- Obtener nombres de estados
    SELECT `Nombre` INTO v_EstadoAnterior FROM `Estadopedido` WHERE `idEstadoPedido` = OLD.`idEstadoPedido`;
    SELECT `Nombre` INTO v_EstadoNuevo FROM `Estadopedido` WHERE `idEstadoPedido` = NEW.`idEstadoPedido`;
    
    -- Solo ejecutar cuando el estado cambia a 'Confirmado'
    IF v_EstadoAnterior = 'Pendiente' AND v_EstadoNuevo = 'Confirmado' THEN
        -- Descontar ingredientes
        UPDATE `Ingrediente` ing
        INNER JOIN `Platoingrediente` pi ON ing.`idIngrediente` = pi.`idIngrediente`
        INNER JOIN `Detallepedido` dp ON pi.`idPlato` = dp.`idPlato`
        SET ing.`StockActual` = ing.`StockActual` - (pi.`CantidadNecesaria` * dp.`Cantidad`)
        WHERE dp.`idPedido` = NEW.`idPedido`;
    END IF;
END;;

-- =============================================
-- Trigger: Actualizar disponibilidad por stock bajo
-- =============================================
DROP TRIGGER IF EXISTS `trg_ActualizarDisponibilidadPorStock`;;
CREATE TRIGGER `trg_ActualizarDisponibilidadPorStock`
AFTER UPDATE ON `Ingrediente`
FOR EACH ROW
BEGIN
    -- Solo ejecutar si cambió el stock
    IF OLD.`StockActual` <> NEW.`StockActual` THEN
        -- Marcar platos como no disponibles si falta algún ingrediente crítico
        UPDATE `Plato` p
        SET p.`Disponible` = FALSE
        WHERE p.`Activo` = TRUE
        AND EXISTS (
            SELECT 1
            FROM `Platoingrediente` pi
            WHERE pi.`idPlato` = p.`idPlato`
            AND pi.`idIngrediente` = NEW.`idIngrediente`
            AND NEW.`EsCritico` = TRUE
            AND NEW.`StockActual` < pi.`CantidadNecesaria`
        );
        
        -- Marcar platos como disponibles si todos los ingredientes críticos están disponibles
        UPDATE `Plato` p
        SET p.`Disponible` = TRUE
        WHERE p.`Activo` = TRUE
        AND p.`Disponible` = FALSE
        AND NOT EXISTS (
            SELECT 1
            FROM `Platoingrediente` pi
            JOIN `Ingrediente` ing ON pi.`idIngrediente` = ing.`idIngrediente`
            WHERE pi.`idPlato` = p.`idPlato`
            AND ing.`EsCritico` = TRUE
            AND ing.`StockActual` < pi.`CantidadNecesaria`
        );
    END IF;
END;;

-- =============================================
-- Trigger: Actualizar tiempo estimado al cambiar estado
-- =============================================
DROP TRIGGER IF EXISTS `trg_ActualizarTiempoEstimado`;;
CREATE TRIGGER `trg_ActualizarTiempoEstimado`
AFTER UPDATE ON `Pedido`
FOR EACH ROW
BEGIN
    DECLARE v_EstadoNuevo VARCHAR(45);
    
    -- Obtener nombre del nuevo estado
    SELECT `Nombre` INTO v_EstadoNuevo 
    FROM `Estadopedido` 
    WHERE `idEstadoPedido` = NEW.`idEstadoPedido`;
    
    -- Cuando el pedido pasa a 'En Preparación', recalcular tiempo estimado
    IF v_EstadoNuevo = 'En Preparación' THEN
        UPDATE `Pedido`
        SET `FechaHoraEntregaEstimada` = fn_CalcularTiempoEstimadoEntrega(NEW.`idPedido`)
        WHERE `idPedido` = NEW.`idPedido`;
    END IF;
END;;

-- =============================================
-- Trigger: Validar cantidad en detalle de pedido
-- =============================================
DROP TRIGGER IF EXISTS `trg_ValidarCantidadDetallePedido`;;
CREATE TRIGGER `trg_ValidarCantidadDetallePedido`
BEFORE INSERT ON `Detallepedido`
FOR EACH ROW
BEGIN
    DECLARE v_CantidadDisponible INT UNSIGNED DEFAULT 0;
    
    SET v_CantidadDisponible = fn_CantidadPlatosDisponibles(NEW.`idPlato`);
    
    IF NEW.`Cantidad` > v_CantidadDisponible THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = CONCAT('Solo hay ', v_CantidadDisponible, ' unidades disponibles de este plato');
    END IF;
END;;

DELIMITER ;