-- #----------------------------#
-- #   CREAR LA BASE DE DATOS   #
-- #----------------------------#

CREATE DATABASE gestorapp
GO

-- #-------------------------------#
-- #  SELECCIONA LA BASE DE DATOS  #
-- #-------------------------------#
USE gestorapp
GO

-- #--------------------------------#
-- #        CREAR LAS TABLAS        #
-- #--------------------------------#

-- # --------------- TABLAS BASE --------------- #
CREATE TABLE Empleado (
	cedula varchar(11) NOT NULL PRIMARY KEY,
	nombre varchar(15) NOT NULL,
	primerApellido varchar(10) NOT NULL,
	segundoApellido varchar(10) NOT NULL
)

CREATE TABLE Departamento (
	codigo varchar(10) NOT NULL PRIMARY KEY,
	nombre varchar(12) NOT NULL,
	cedulaJefe varchar(11) NOT NULL FOREIGN KEY REFERENCES Empleado(cedula) UNIQUE
)

CREATE TABLE Aplicacion (
	codigo varchar(10) NOT NULL PRIMARY KEY,
	numPatente varchar(15),
	nombre varchar(10) NOT NULL,
	descripcion varchar(80) NOT NULL,
	tipo varchar(12) NOT NULL,
	fechaProduccion date,
	fechaExpiraLicencia date,
	codigoDepartamento varchar(10) NOT NULL FOREIGN KEY REFERENCES Departamento(codigo)
)

CREATE TABLE Servidor (
	serie varchar(12) NOT NULL PRIMARY KEY,
	marca varchar(10) NOT NULL,
	modelo varchar(10) NOT NULL,
	fechaCompra date NOT NULL,
	capacidadProcesamiento varchar(6) NOT NULL,
	capacidadAlmacenamiento varchar(6) NOT NULL,
	memoria smallint NOT NULL
)

CREATE TABLE Proyecto (
	id varchar(8) NOT NULL PRIMARY KEY,
	nombre varchar(10) NOT NULL,
	descripcion varchar(100) NOT NULL,
	fechaInicio date NOT NULL,
	fechaFinalizacion date NOT NULL,
	esfuerzoEstimado varchar(8) NOT NULL,
	esfuerzoReal varchar(8) NOT NULL
)

CREATE TABLE Error (
	id varchar(8) NOT NULL PRIMARY KEY,
	descripcion varchar(80) NOT NULL,
	fecha date NOT NULL,
	hora time NOT NULL,
	impacto varchar(11) NOT NULL,
	serieServidor varchar(12) NOT NULL FOREIGN KEY REFERENCES Servidor(serie),
	codigoAplicacion varchar(10) NOT NULL FOREIGN KEY REFERENCES Aplicacion(codigo),
	idProyecto varchar(8) FOREIGN KEY REFERENCES Proyecto(id)
)

-- # --------------- TABLAS POR RELACIONES --------------- #
CREATE TABLE ServidorXAplicacion (
	codigoAplicacion varchar(10)  NOT NULL FOREIGN KEY REFERENCES Aplicacion(codigo),
	serieServidor varchar(12) NOT NULL FOREIGN KEY REFERENCES Servidor(serie),
	rol varchar(10) NOT NULL,
	PRIMARY KEY (codigoAplicacion, serieServidor)
)

CREATE TABLE EmpleadoXProyecto (
	cedulaEmpleado varchar(11)  NOT NULL FOREIGN KEY REFERENCES Empleado(cedula),
	idProyecto varchar(8)  NOT NULL FOREIGN KEY REFERENCES Proyecto(id),
	rol varchar(10)  NOT NULL,
	PRIMARY KEY (cedulaEmpleado, idProyecto)
)

-- #---------------------------------#
-- #          AGREGAR DATOS          #
-- #---------------------------------#

-- Agregar empleados
INSERT INTO Empleado (cedula, nombre, primerApellido, segundoApellido) 
VALUES 
('116590185', 'Kristin', 'Alvarado', 'Gonzalez'),
('116590186', 'Ana', 'Gonzalez', 'Aguilera'),
('106590187', 'Leda', 'Gonzalez', 'Aguilera'),
('106560190', 'Jocelyn', 'Roman', 'Rojas'),
('119590265', 'Tatiana', 'Rojas', 'Gutierrez'),
('119590269', 'Jake', 'Hernandez', 'Herrera'),
('119590268', 'Audra', 'Rodriguez', 'Rojas'),
('119590260', 'Daniela', 'Quiroz', 'Gutierrez')

-- Agregar departamentos
INSERT INTO Departamento (codigo, nombre, cedulaJefe) 
VALUES 
('CB465JK78A', 'SafetyAccess', '116590185'),
('26LWS5873P', 'Renewed', '116590186'),
('58WER24P89', 'SPS', '119590265'),
('PA517W2TGB', 'Kindel', '119590269'),
('084LTBH94L', 'GoodReads', '119590260'),
('WS789PBT0U', 'STAR', '106590187')

-- Agregar aplicaciones
INSERT INTO Aplicacion (codigo, numPatente, nombre, descripcion, tipo, codigoDepartamento) 
VALUES 
('APP123923', 'KU12584LW6751', 'ClickUpTo', 'Esta aplicacion es para manejar el timepo', 'Negocio', 'CB465JK78A'),
('APP123339', 'KU123759KW4KY8', 'Ferrer', 'Esta aplicacion para manejar planilla', 'Otra', '26LWS5873P'),
('APP915759', 'WBN9467PO12DWS', 'PlantBase', 'Esta aplicacion es encontrar restaurantes veganos', 'Utilitaria', 'WS789PBT0U')

-- Actualizar Aplicacion
UPDATE Aplicacion SET fechaProduccion = '2016-12-22' WHERE codigo = 'APP123923'
UPDATE Aplicacion SET fechaExpiraLicencia = '2016-12-22' WHERE codigo = 'APP123923'
UPDATE Aplicacion SET fechaProduccion = '2015-08-17', fechaExpiraLicencia = '2016-07-17' WHERE codigo = 'APP123339'
UPDATE Aplicacion SET fechaProduccion = '2019-04-17', fechaExpiraLicencia = '2020-03-17' WHERE codigo = 'APP915759'

-- Agregar servidores
INSERT INTO Servidor (serie, marca, modelo, fechaCompra, capacidadProcesamiento, capacidadAlmacenamiento, memoria) 
VALUES 
('SER123923', 'MARCA0001', 'MODELO001', '2015-12-22', '64', '64', 64),
('SER123924', 'MARCA0002', 'MODELO002', '2015-07-17', '32', '64', 64),
('SER123925', 'MARCA0003', 'MODELO003', '2018-10-25', '180', '128', 128),
('SER123926', 'MARCA0004', 'MODELO004', '2019-03-17', '256', '256', 128),
('SER123927', 'MARCA0005', 'MODELO005', '2022-12-17', '25', '32', 128)

-- Agregar servidor por aplicacion
INSERT INTO ServidorXAplicacion (codigoAplicacion, serieServidor, rol)
VALUES
('APP123923', 'SER123923', 'Stagging'),
('APP123339', 'SER123923', 'Testing'),
('APP915759', 'SER123924', 'Stagging')

-- Agregar proyectos
INSERT INTO Proyecto (id, nombre, descripcion, fechaInicio, fechaFinalizacion, esfuerzoEstimado, esfuerzoReal) 
VALUES 
('PRO123', 'PROYECTO01', 'PROYECTO01', '2022-03-17', '2022-05-17', 'Cod 50%', 'Cod 30%'),
('PRO124', 'PROYECTO02', 'PROYECTO02', '2022-03-17', '2022-05-17', 'Cod 70%', 'Cod 56%'),
('PRO125', 'PROYECTO03', 'PROYECTO03', '2022-03-17', '2022-05-17', 'Cod 10%', 'Cod 30%'),
('PRO126', 'PROYECTO04', 'PROYECTO04', '2022-03-17', '2022-05-17', 'Cod 60%', 'Cod 60%')

-- Agregar errores
INSERT INTO Error (id, descripcion, fecha, hora, impacto, serieServidor, codigoAplicacion, idProyecto) 
VALUES 
('ERR0001', 'ERR0001', '2022-03-17', '11:20:30', 'Bajo', 'SER123923', 'APP123923', 'PRO123'),
('ERR0002', 'ERR0002', '2022-03-17', '11:20:30', 'Medio', 'SER123924', 'APP123339', 'PRO124')

-- Agregar empleados por proyecto
INSERT INTO EmpleadoXProyecto (cedulaEmpleado, idProyecto, rol) 
VALUES 
('116590185', 'PRO123', 'Supervisor'),
('106560190', 'PRO123', 'Coder'),
('119590265', 'PRO123', 'Tester'),
('116590186', 'PRO124', 'Supervisor'),
('106590187', 'PRO125', 'Supervisor')


-- #---------------------------#
-- #          SELECTS          #
-- #---------------------------#

-- Aplicacion
SELECT * FROM Aplicacion

-- Departamento
SELECT * FROM Departamento

-- Errores
SELECT * FROM Error

-- Servidores
SELECT * FROM Servidor

-- Proyectos
SELECT 
Proyecto.*,
Error.id AS errorId,
Error.descripcion AS errorDesc,
Error.fecha,
Error.hora,
Error.impacto,
Error.serieServidor,
Error.codigoAplicacion,
EmpleadoXProyecto.cedulaEmpleado,
Empleado.nombre + ' ' + Empleado.primerApellido + ' ' + Empleado.segundoApellido AS nombreEmpleado,
EmpleadoXProyecto.rol
FROM Proyecto
LEFT JOIN Error ON Proyecto.id = Error.idProyecto
LEFT JOIN EmpleadoXProyecto ON Proyecto.id = EmpleadoXProyecto.idProyecto
LEFT JOIN Empleado ON Empleado.cedula = EmpleadoXProyecto.cedulaEmpleado

-- Servidor x Aplicacion
SELECT * 
FROM ServidorXAplicacion

-- ID de Proyectos
SELECT id
FROM Proyecto

-- ID de Errores
SELECT id FROM Error

-- Empleado x Proyecto
SELECT * 
FROM EmpleadoXProyecto