--creo la base de datos de ventas--------------------------------------------------------------------------
if exists(Select * FROM SysDataBases WHERE name='Ventas')
BEGIN
	DROP DATABASE Ventas
END
go

CREATE DATABASE Ventas

go

--pone en uso la bd----------------------------------------------------------------------------------------
USE Ventas
go

--creo las tablas------------------------------------------------------------------------------------------
CREATE TABLE Usuarios(
	Usulog varchar(15) not null PRIMARY KEY,
	PassLog varchar(5) not null
)
GO

CREATE TABLE Articulos(
	CodArt int NOT NULL PRIMARY KEY ,
	NomArt varchar(25) NOT NULL,
	PreArt int NOT NULL check (PreArt > 0)
) 
go

CREATE TABLE Facturas(
	NumFact int NOT NULL PRIMARY KEY ,
	FechaFact DATETIME NOT NULL DEFAULT getdate(),
	UsuLog varchar(15) NOT NULL FOREIGN KEY REFERENCES Usuarios (UsuLog)
)
go	

CREATE TABLE LINEAS (
	NumFact int NOT NULL FOREIGN KEY REFERENCES Facturas (NumFact),
	CodArt int NOT NULL FOREIGN KEY REFERENCES Articulos(CodArt),
	Cant int NOT NULL,
	Primary Key(NumFact, CodArt)
)
go



--creacion de usuario IIS para que el sitio pueda acceder a la bd-------------------------------------------
USE master
GO

CREATE LOGIN [IIS APPPOOL\DefaultAppPool] FROM WINDOWS 
GO

USE Ventas
GO

CREATE USER [IIS APPPOOL\DefaultAppPool] FOR LOGIN [IIS APPPOOL\DefaultAppPool]
GO

GRANT Execute to [IIS APPPOOL\DefaultAppPool]
go


----------------------------------------------------------------------------------------------------------
-- Sp de logueo
Create Procedure Logueo @UsuL varchar(15), @PassL varchar(5) AS
Begin
	Select * from Usuarios Where Usulog = @UsuL AND PassLog = @PassL
End
go 

Create Procedure BuscoUsuario @Usu varchar(15) As
Begin
	Select * from Usuarios Where Usulog = @Usu
End
go

--creo SP de alta
Create Procedure AltaArticulo
 @cod int, 
 @nom varchar(20), 
 @pre money AS
 
Begin
	if (exists(select * from Articulos where codArt = @cod))
		return -1

	Insert Articulos(CodARt, NomArt, PreArt) Values (@cod, @nom, @pre)
	return 1
end
go

--creo SP de Baja
Create Procedure BajaArticulo @cod int AS
Begin
	if (not exists(select * from Articulos where codArt = @cod))
		return -1
	
	If (exists(select * from LINEAS where CodArt = @cod))
		return -2
		
	Delete From Articulos where codArt = @cod
	return 1
end
go

--creo SP de modificacion
Create Procedure ModArticulo @cod int, @nom varchar(20), @pre money AS
Begin
	if (not exists(select * from Articulos where codArt = @cod))
		return -1

	Update Articulos Set NomArt=@nom, PreArt=@pre where codArt = @cod
	return 1
end
go

--creo SP de busqueda
Create Procedure BuscoArticulo @cod int AS
Begin
	Select *
	From Articulos
	where codArt = @cod
end
go

--creo SP de listado
Create Procedure ListoArticulo AS
Begin
	Select *
	From Articulos
end
go


--CREO SP ALTA FACTURA
CREATE PROC AltaFactura 
@numFact INT,
 @NomUsu varchar(15)
AS
BEGIN
	if not exists(select * from Usuarios where Usulog = @NomUsu)
		return -1
	if exists(select * from Facturas where NumFact = @numFact)
		return -2
	
		
	INSERT Facturas (NumFact, UsuLog)
	VALUES(@numFact, @NomUsu)
	
	IF @@ERROR <> 0
		RETURN -3
	ELSE
		RETURN 1

END
GO 


CREATE PROC AltaLinea 
@numFact INT,
 @codArt INT,
  @cantidad INT
AS
BEGIN
	if not exists(select * from Articulos where codArt = @codArt)
		return -1
	if not exists(select * from Facturas where NumFact = @numFact)
		return -2
	
		
	INSERT Lineas (NumFact, CodArt, Cant)
	VALUES(@numFact, @codArt, @cantidad)
	
	IF @@ERROR <> 0
		RETURN -3
	ELSE
		RETURN 1

END
GO 

--creo SP de listado
Create Procedure ListoFacturas AS
Begin
	Select *
	From Facturas
end
go

Create Procedure ListoLineas @numFact INT AS
Begin
	Select *
	From LINEAS
	Where LINEAS.NumFact = @numFact
End
go


create Proc CambioPass @usu varchar(15), @pass varchar(5) AS
Begin
	if Not exists(Select * from Usuarios where Usulog = @usu)
		return -1
	
	update Usuarios
	Set PassLog = @pass
	where Usulog = @usu
	
	return 1

End
go

Create Proc ListoUsuarios AS
Begin
	Select * From Usuarios
End
go


Create Proc AltaUsuario @usu varchar(15), @pass varchar(5) AS
Begin
	if exists (select * from Usuarios Where Usulog = @usu)
		return -1 
		
	insert into Usuarios values(@usu, @pass)
	return 1
End
go

Create Proc EliminarUsuario @usu varchar(15) AS
Begin
	if NOT exists (select * from Usuarios Where Usulog = @usu)
		return -1 -- no existe
    
    if exists (select * from Facturas where UsuLog = @usu)
		return -2 -- hay facturas y no tengo baja logica - no se borra
		
	delete Usuarios
	where Usulog = @usu
	return 1
End
go



--ingreso datos de prueba ----------------------------------------------------------------------------
INSERT Articulos Values(23, 'Licuadora', 2500)
INSERT Articulos Values(48, 'Maquina de Coser', 6700)
INSERT Articulos Values(514, 'Ventilador de Techo', 250)
go

INSERT Usuarios(Usulog, PassLog) Values('Usu1', '12345')
INSERT Usuarios(Usulog, PassLog) Values('Usu2', '54321')
go

exec AltaFactura 1, 'Usu1'
exec AltaLinea 1, 48, 10
exec AltaLinea 1, 514, 3

exec AltaFactura 2, 'Usu2'
exec AltaLinea 2, 514, 52
go
