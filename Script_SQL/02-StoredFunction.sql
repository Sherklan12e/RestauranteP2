-- =============================================
-- Stored Functions para Sistema de Disponibilidad
-- =============================================

USE `RestauranteDisponibilidad`;
DELIMITER ;;

-- =============================================
-- Función: Verificar si el restaurante está abierto
-- =============================================
DROP FUNCTION IF EXISTS `fn_RestauranteAbierto`;;
CREATE FUNCTION `fn_RestauranteAbierto`()
RETURNS BOOLEAN
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_EstaAbierto BOOLEAN DEFAULT FALSE;
    DECLARE v_DiaSemana TINYINT;
    DECLARE v_HoraActual TIME;
    
    -- Obtener día de la semana (0=Domingo, 1=Lunes, ..., 6=Sábado)
    SET v_DiaSemana = DAYOFWEEK(NOW()) - 1;
    IF v_DiaSemana = 0 THEN
        SET v_DiaSemana = 6; -- Domingo = 0 en nuestra estructura
    ELSE
        SET v_DiaSemana = v_DiaSemana - 1; -- Lunes = 1
    END IF;
    
    SET v_HoraActual = TIME(NOW());
    
    IF EXISTS (
        SELECT 1 
        FROM `Horariorestaurante` 
        WHERE `DiaSemana` = v_DiaSemana 
        AND `Activo` = TRUE
        AND v_HoraActual BETWEEN `HoraApertura` AND `HoraCierre`
    ) THEN
        SET v_EstaAbierto = TRUE;
    END IF;
    
    RETURN v_EstaAbierto;
END;;

-- =============================================
-- Función: Verificar disponibilidad de plato
-- =============================================
DROP FUNCTION IF EXISTS `fn_VerificarDisponibilidadPlato`;;
CREATE FUNCTION `fn_VerificarDisponibilidadPlato`(p_PlatoID INT UNSIGNED)
RETURNS BOOLEAN
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_Disponible BOOLEAN DEFAULT FALSE;
    
    -- Verificar si el plato está activo
    IF NOT EXISTS (
        SELECT 1 FROM `Plato` 
        WHERE `idPlato` = p_PlatoID 
        AND `Disponible` = TRUE 
        AND `Activo` = TRUE
    ) THEN
        RETURN FALSE;
    END IF;
    
    -- Verificar si el restaurante está abierto
    IF fn_RestauranteAbierto() = FALSE THEN
        RETURN FALSE;
    END IF;
    
    -- Verificar ingredientes críticos
    IF EXISTS (
        SELECT 1
        FROM `Platoingrediente` pi
        JOIN `Ingrediente` i ON pi.`idIngrediente` = i.`idIngrediente`
        WHERE pi.`idPlato` = p_PlatoID
        AND i.`EsCritico` = TRUE
        AND i.`StockActual` < pi.`CantidadNecesaria`
    ) THEN
        RETURN FALSE;
    END IF;
    
    SET v_Disponible = TRUE;
    RETURN v_Disponible;
END;;

-- =============================================
-- Función: Calcular tiempo de preparación de pedido
-- =============================================
DROP FUNCTION IF EXISTS `fn_CalcularTiempoPreparacionPedido`;;
CREATE FUNCTION `fn_CalcularTiempoPreparacionPedido`(p_PedidoID INT UNSIGNED)
RETURNS INT UNSIGNED
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_TiempoTotal INT UNSIGNED DEFAULT 0;
    
    -- Obtener el tiempo máximo de preparación (platos se preparan en paralelo)
    SELECT COALESCE(MAX(p.`TiempoPreparacion`), 0) INTO v_TiempoTotal
    FROM `Detallepedido` dp
    JOIN `Plato` p ON dp.`idPlato` = p.`idPlato`
    WHERE dp.`idPedido` = p_PedidoID;
    
    RETURN v_TiempoTotal;
END;;

-- =============================================
-- Función: Calcular tiempo estimado de entrega
-- =============================================
DROP FUNCTION IF EXISTS `fn_CalcularTiempoEstimadoEntrega`;;
CREATE FUNCTION `fn_CalcularTiempoEstimadoEntrega`(p_PedidoID INT UNSIGNED)
RETURNS DATETIME
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_TiempoPreparacion INT UNSIGNED;
    DECLARE v_TiempoEspera INT UNSIGNED DEFAULT 5; -- 5 minutos de tiempo base de espera
    DECLARE v_TiempoEstimado DATETIME;
    DECLARE v_PedidosPendientes INT UNSIGNED DEFAULT 0;
    
    -- Calcular tiempo de preparación
    SET v_TiempoPreparacion = fn_CalcularTiempoPreparacionPedido(p_PedidoID);
    
    -- Agregar tiempo de espera por pedidos pendientes
    SELECT COUNT(*) INTO v_PedidosPendientes
    FROM `Pedido` p
    INNER JOIN `Estadopedido` ep ON p.`idEstadoPedido` = ep.`idEstadoPedido`
    WHERE ep.`Nombre` IN ('Pendiente', 'En Preparación')
    AND p.`idPedido` < p_PedidoID;
    
    SET v_TiempoEspera = v_TiempoEspera + (v_PedidosPendientes * 2);
    
    SET v_TiempoEstimado = DATE_ADD(NOW(), INTERVAL (v_TiempoPreparacion + v_TiempoEspera) MINUTE);
    
    RETURN v_TiempoEstimado;
END;;

-- =============================================
-- Función: Obtener stock disponible de ingrediente
-- =============================================
DROP FUNCTION IF EXISTS `fn_ObtenerStockDisponible`;;
CREATE FUNCTION `fn_ObtenerStockDisponible`(p_IngredienteID INT UNSIGNED)
RETURNS DECIMAL(10,2)
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_Stock DECIMAL(10,2) DEFAULT 0;
    
    SELECT COALESCE(`StockActual`, 0) INTO v_Stock
    FROM `Ingrediente`
    WHERE `idIngrediente` = p_IngredienteID;
    
    RETURN v_Stock;
END;;

-- =============================================
-- Función: Verificar si es hora cercana al cierre
-- =============================================
DROP FUNCTION IF EXISTS `fn_EsHoraCercanaAlCierre`;;
CREATE FUNCTION `fn_EsHoraCercanaAlCierre`(p_MinutosAntes INT UNSIGNED)
RETURNS BOOLEAN
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_EsCercano BOOLEAN DEFAULT FALSE;
    DECLARE v_DiaSemana TINYINT;
    DECLARE v_HoraActual TIME;
    DECLARE v_HoraCierre TIME;
    DECLARE v_HoraLimite TIME;
    
    -- Obtener día de la semana
    SET v_DiaSemana = DAYOFWEEK(NOW()) - 1;
    IF v_DiaSemana = 0 THEN
        SET v_DiaSemana = 6;
    ELSE
        SET v_DiaSemana = v_DiaSemana - 1;
    END IF;
    
    SET v_HoraActual = TIME(NOW());
    
    SELECT `HoraCierre` INTO v_HoraCierre
    FROM `Horariorestaurante`
    WHERE `DiaSemana` = v_DiaSemana AND `Activo` = TRUE
    LIMIT 1;
    
    IF v_HoraCierre IS NOT NULL THEN
        SET v_HoraLimite = TIME(DATE_SUB(CONCAT(CURDATE(), ' ', v_HoraCierre), INTERVAL p_MinutosAntes MINUTE));
        
        IF v_HoraActual >= v_HoraLimite THEN
            SET v_EsCercano = TRUE;
        END IF;
    END IF;
    
    RETURN v_EsCercano;
END;;

-- =============================================
-- Función: Calcular cantidad de platos disponibles
-- =============================================
DROP FUNCTION IF EXISTS `fn_CantidadPlatosDisponibles`;;
CREATE FUNCTION `fn_CantidadPlatosDisponibles`(p_PlatoID INT UNSIGNED)
RETURNS INT UNSIGNED
READS SQL DATA
DETERMINISTIC
BEGIN
    DECLARE v_CantidadMaxima INT UNSIGNED DEFAULT 999;
    DECLARE v_CantidadTemp DECIMAL(10,2);
    
    -- Calcular cuántos platos se pueden hacer con los ingredientes disponibles
    SELECT COALESCE(MIN(FLOOR(i.`StockActual` / pi.`CantidadNecesaria`)), 0) INTO v_CantidadTemp
    FROM `Platoingrediente` pi
    INNER JOIN `Ingrediente` i ON pi.`idIngrediente` = i.`idIngrediente`
    WHERE pi.`idPlato` = p_PlatoID
    AND i.`EsCritico` = TRUE;
    
    SET v_CantidadMaxima = CAST(v_CantidadTemp AS UNSIGNED);
    
    RETURN COALESCE(v_CantidadMaxima, 0);
END;;

DELIMITER ;
