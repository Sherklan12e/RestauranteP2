-- =====================================================
-- Sistema de Disponibilidad de Platos en Tiempo Real
-- Base de Datos para Restaurante
-- =====================================================

DROP DATABASE IF EXISTS RestauranteDisponibilidad;

-- -----------------------------------------------------
-- Schema RestauranteDisponibilidad
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `RestauranteDisponibilidad`;

CREATE SCHEMA IF NOT EXISTS `RestauranteDisponibilidad`;
CREATE DATABASE IF NOT EXISTS RestauranteDisponibilidad;
USE `RestauranteDisponibilidad`;

-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Usuario`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Usuario` (
  `idUsuario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Apellido` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(60) NOT NULL,
  `Telefono` VARCHAR(20) NULL,
  `Contrasena` CHAR(64) NOT NULL,
  `FechaRegistro` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Activo` BOOLEAN NOT NULL DEFAULT TRUE,
  `Rol` VARCHAR(20) NOT NULL DEFAULT 'cliente' COMMENT 'Rol del usuario: cliente o admin',
  PRIMARY KEY (`idUsuario`),
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC),
  INDEX `idx_telefono` (`Telefono` ASC)
  
)
ENGINE = InnoDB
COMMENT = 'Tabla de usuarios/clientes del restaurante';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`CategoriaPlato`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`CategoriaPlato`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`CategoriaPlato` (
  `idCategoria` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Descripcion` VARCHAR(200) NULL,
  PRIMARY KEY (`idCategoria`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC)
)
ENGINE = InnoDB
COMMENT = 'Categorías de platos (Entradas, Principales, Postres, Menú del Día, etc.)';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Plato`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Plato`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Plato` (
  `idPlato` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idCategoria` INT UNSIGNED NOT NULL,
  `Nombre` VARCHAR(100) NOT NULL,
  `Descripcion` VARCHAR(300) NULL,
  `Precio` DECIMAL(10,2) UNSIGNED NOT NULL,
  `TiempoPreparacion` INT UNSIGNED NOT NULL COMMENT 'Tiempo en minutos',
  `ImagenURL` VARCHAR(200) NULL,
  `Disponible` BOOLEAN NOT NULL DEFAULT TRUE,
  `EsMenuDelDia` BOOLEAN NOT NULL DEFAULT FALSE,
  `Activo` BOOLEAN NOT NULL DEFAULT TRUE,
  PRIMARY KEY (`idPlato`),
  INDEX `fk_Plato_Categoria_idx` (`idCategoria` ASC),
  INDEX `idx_disponible` (`Disponible` ASC),
  INDEX `idx_menu_dia` (`EsMenuDelDia` ASC),
  CONSTRAINT `fk_Plato_Categoria`
    FOREIGN KEY (`idCategoria`)
    REFERENCES `RestauranteDisponibilidad`.`CategoriaPlato` (`idCategoria`)
)
ENGINE = InnoDB
COMMENT = 'Platos del menú del restaurante';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Ingrediente`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Ingrediente`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Ingrediente` (
  `idIngrediente` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(100) NOT NULL,
  `UnidadMedida` VARCHAR(20) NOT NULL COMMENT 'kg, litros, unidades, etc.',
  `StockActual` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `StockMinimo` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `EsCritico` BOOLEAN NOT NULL DEFAULT FALSE COMMENT 'Si es crítico para la disponibilidad',
  PRIMARY KEY (`idIngrediente`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC),
  INDEX `idx_stock_critico` (`EsCritico` ASC, `StockActual` ASC)
)
ENGINE = InnoDB
COMMENT = 'Ingredientes para control de disponibilidad';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`PlatoIngrediente`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`PlatoIngrediente`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`PlatoIngrediente` (
  `idPlato` INT UNSIGNED NOT NULL,
  `idIngrediente` INT UNSIGNED NOT NULL,
  `CantidadNecesaria` DECIMAL(10,2) UNSIGNED NOT NULL,
  PRIMARY KEY (`idPlato`, `idIngrediente`),
  INDEX `fk_PlatoIngrediente_Ingrediente_idx` (`idIngrediente` ASC),
  CONSTRAINT `fk_PlatoIngrediente_Plato`
    FOREIGN KEY (`idPlato`)
    REFERENCES `RestauranteDisponibilidad`.`Plato` (`idPlato`),
  CONSTRAINT `fk_PlatoIngrediente_Ingrediente`
    FOREIGN KEY (`idIngrediente`)
    REFERENCES `RestauranteDisponibilidad`.`Ingrediente` (`idIngrediente`)
)
ENGINE = InnoDB
COMMENT = 'Relación entre platos e ingredientes necesarios';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`HorarioRestaurante`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`HorarioRestaurante`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`HorarioRestaurante` (
  `idHorario` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `DiaSemana` TINYINT UNSIGNED NOT NULL COMMENT '0=Domingo, 1=Lunes, ..., 6=Sábado',
  `HoraApertura` TIME NOT NULL,
  `HoraCierre` TIME NOT NULL,
  `Activo` BOOLEAN NOT NULL DEFAULT TRUE,
  PRIMARY KEY (`idHorario`),
  UNIQUE INDEX `DiaSemana_UNIQUE` (`DiaSemana` ASC)
)
ENGINE = InnoDB
COMMENT = 'Horarios de operación del restaurante';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Mesa`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Mesa`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Mesa` (
  `idMesa` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `NumeroMesa` VARCHAR(10) NOT NULL,
  `Capacidad` TINYINT UNSIGNED NOT NULL,
  `Activa` BOOLEAN NOT NULL DEFAULT TRUE,
  PRIMARY KEY (`idMesa`),
  UNIQUE INDEX `NumeroMesa_UNIQUE` (`NumeroMesa` ASC)
)
ENGINE = InnoDB
COMMENT = 'Mesas del restaurante';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`MetodoPago`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`MetodoPago`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`MetodoPago` (
  `idMetodoPago` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `TipoMedioPago` VARCHAR(45) NOT NULL,
  `Activo` BOOLEAN NOT NULL DEFAULT TRUE,
  PRIMARY KEY (`idMetodoPago`),
  UNIQUE INDEX `TipoMedioPago_UNIQUE` (`TipoMedioPago` ASC)
)
ENGINE = InnoDB
COMMENT = 'Métodos de pago disponibles';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Reserva`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Reserva`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Reserva` (
  `idReserva` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idMesa` INT UNSIGNED NULL,
  `FechaHora` DATETIME NOT NULL,
  `CantidadPersonas` TINYINT UNSIGNED NOT NULL,
  `Estado` ENUM('Pendiente', 'Confirmada', 'Cancelada', 'Completada') NOT NULL DEFAULT 'Pendiente',
  `Comentarios` VARCHAR(300) NULL,
  `FechaCreacion` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idReserva`),
  INDEX `fk_Reserva_Usuario_idx` (`idUsuario` ASC),
  INDEX `fk_Reserva_Mesa_idx` (`idMesa` ASC),
  INDEX `idx_fecha_estado` (`FechaHora` ASC, `Estado` ASC),
  CONSTRAINT `fk_Reserva_Usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `RestauranteDisponibilidad`.`Usuario` (`idUsuario`),
  CONSTRAINT `fk_Reserva_Mesa`
    FOREIGN KEY (`idMesa`)
    REFERENCES `RestauranteDisponibilidad`.`Mesa` (`idMesa`)
)
ENGINE = InnoDB
COMMENT = 'Reservas de mesas';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`EstadoPedido`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`EstadoPedido`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`EstadoPedido` (
  `idEstadoPedido` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Nombre` VARCHAR(45) NOT NULL,
  `Descripcion` VARCHAR(100) NULL,
  PRIMARY KEY (`idEstadoPedido`),
  UNIQUE INDEX `Nombre_UNIQUE` (`Nombre` ASC)
)
ENGINE = InnoDB
COMMENT = 'Estados posibles de un pedido (Pendiente, En Preparación, Listo, Entregado, Cancelado)';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Pedido`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Pedido`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Pedido` (
  `idPedido` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idReserva` INT UNSIGNED NULL COMMENT 'NULL si es pedido sin reserva',
  `idEstadoPedido` INT UNSIGNED NOT NULL,
  `idMetodoPago` INT UNSIGNED NULL,
  `FechaHoraPedido` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FechaHoraEntregaEstimada` DATETIME NULL,
  `FechaHoraEntregaReal` DATETIME NULL,
  `EsPreOrden` BOOLEAN NOT NULL DEFAULT FALSE,
  `Total` DECIMAL(10,2) UNSIGNED NOT NULL DEFAULT 0,
  `Comentarios` VARCHAR(300) NULL,
  PRIMARY KEY (`idPedido`),
  INDEX `fk_Pedido_Usuario_idx` (`idUsuario` ASC),
  INDEX `fk_Pedido_Reserva_idx` (`idReserva` ASC),
  INDEX `fk_Pedido_Estado_idx` (`idEstadoPedido` ASC),
  INDEX `fk_Pedido_MetodoPago_idx` (`idMetodoPago` ASC),
  INDEX `idx_fecha_estado` (`FechaHoraPedido` ASC, `idEstadoPedido` ASC),
  INDEX `idx_preorden` (`EsPreOrden` ASC),
  CONSTRAINT `fk_Pedido_Usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `RestauranteDisponibilidad`.`Usuario` (`idUsuario`),
  CONSTRAINT `fk_Pedido_Reserva`
    FOREIGN KEY (`idReserva`)
    REFERENCES `RestauranteDisponibilidad`.`Reserva` (`idReserva`),
  CONSTRAINT `fk_Pedido_Estado`
    FOREIGN KEY (`idEstadoPedido`)
    REFERENCES `RestauranteDisponibilidad`.`EstadoPedido` (`idEstadoPedido`),
  CONSTRAINT `fk_Pedido_MetodoPago`
    FOREIGN KEY (`idMetodoPago`)
    REFERENCES `RestauranteDisponibilidad`.`MetodoPago` (`idMetodoPago`)
)
ENGINE = InnoDB
COMMENT = 'Pedidos realizados por los clientes';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`DetallePedido`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`DetallePedido`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`DetallePedido` (
  `idDetallePedido` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPedido` INT UNSIGNED NOT NULL,
  `idPlato` INT UNSIGNED NOT NULL,
  `Cantidad` TINYINT UNSIGNED NOT NULL DEFAULT 1,
  `PrecioUnitario` DECIMAL(10,2) UNSIGNED NOT NULL,
  `Subtotal` DECIMAL(10,2) UNSIGNED NOT NULL,
  `Comentarios` VARCHAR(200) NULL COMMENT 'Comentarios especiales del cliente',
  PRIMARY KEY (`idDetallePedido`),
  INDEX `fk_DetallePedido_Pedido_idx` (`idPedido` ASC),
  INDEX `fk_DetallePedido_Plato_idx` (`idPlato` ASC),
  CONSTRAINT `fk_DetallePedido_Pedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `RestauranteDisponibilidad`.`Pedido` (`idPedido`),
  CONSTRAINT `fk_DetallePedido_Plato`
    FOREIGN KEY (`idPlato`)
    REFERENCES `RestauranteDisponibilidad`.`Plato` (`idPlato`)
)
ENGINE = InnoDB
COMMENT = 'Detalle de platos en cada pedido';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`HistorialDisponibilidad`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`HistorialDisponibilidad`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`HistorialDisponibilidad` (
  `idHistorial` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPlato` INT UNSIGNED NOT NULL,
  `FechaHora` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Disponible` BOOLEAN NOT NULL,
  `Motivo` VARCHAR(200) NULL COMMENT 'Razón del cambio de disponibilidad',
  PRIMARY KEY (`idHistorial`),
  INDEX `fk_Historial_Plato_idx` (`idPlato` ASC),
  INDEX `idx_fecha` (`FechaHora` ASC),
  CONSTRAINT `fk_Historial_Plato`
    FOREIGN KEY (`idPlato`)
    REFERENCES `RestauranteDisponibilidad`.`Plato` (`idPlato`)
)
ENGINE = InnoDB
COMMENT = 'Registro histórico de cambios en disponibilidad de platos';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`Calificacion`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`Calificacion`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`Calificacion` (
  `idCalificacion` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `idPlato` INT UNSIGNED NOT NULL,
  `idUsuario` INT UNSIGNED NOT NULL,
  `idPedido` INT UNSIGNED NULL,
  `Puntuacion` TINYINT UNSIGNED NOT NULL COMMENT 'Escala 1-5',
  `Comentario` VARCHAR(300) NULL,
  `FechaHora` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`idCalificacion`),
  INDEX `fk_Calificacion_Plato_idx` (`idPlato` ASC),
  INDEX `fk_Calificacion_Usuario_idx` (`idUsuario` ASC),
  INDEX `fk_Calificacion_Pedido_idx` (`idPedido` ASC),
  INDEX `idx_puntuacion` (`Puntuacion` ASC),
  CONSTRAINT `fk_Calificacion_Plato`
    FOREIGN KEY (`idPlato`)
    REFERENCES `RestauranteDisponibilidad`.`Plato` (`idPlato`),
  CONSTRAINT `fk_Calificacion_Usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `RestauranteDisponibilidad`.`Usuario` (`idUsuario`),
  CONSTRAINT `fk_Calificacion_Pedido`
    FOREIGN KEY (`idPedido`)
    REFERENCES `RestauranteDisponibilidad`.`Pedido` (`idPedido`),
  CONSTRAINT `chk_puntuacion` CHECK (`Puntuacion` >= 1 AND `Puntuacion` <= 5)
)
ENGINE = InnoDB
COMMENT = 'Calificaciones y comentarios de platos por usuarios';


-- -----------------------------------------------------
-- Table `RestauranteDisponibilidad`.`ConfiguracionRestaurante`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RestauranteDisponibilidad`.`ConfiguracionRestaurante`;

CREATE TABLE IF NOT EXISTS `RestauranteDisponibilidad`.`ConfiguracionRestaurante` (
  `idConfiguracion` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Clave` VARCHAR(50) NOT NULL,
  `Valor` VARCHAR(200) NOT NULL,
  `Descripcion` VARCHAR(200) NULL,
  PRIMARY KEY (`idConfiguracion`),
  UNIQUE INDEX `Clave_UNIQUE` (`Clave` ASC)
)
ENGINE = InnoDB
COMMENT = 'Configuraciones generales del sistema (tiempo antes de cierre para pedidos, etc.)';
