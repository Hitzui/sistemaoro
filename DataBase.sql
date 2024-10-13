USE [master]
GO
/****** Object:  Database [Sistema]    Script Date: 8/10/2024 1:28:30 PM ******/
CREATE DATABASE [Sistema]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Sistema', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Sistema.mdf' , SIZE = 21504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Sistema_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Sistema_log.ldf' , SIZE = 76736KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Sistema] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Sistema].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Sistema] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Sistema] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Sistema] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Sistema] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Sistema] SET ARITHABORT OFF 
GO
ALTER DATABASE [Sistema] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Sistema] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Sistema] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Sistema] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Sistema] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Sistema] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Sistema] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Sistema] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Sistema] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Sistema] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Sistema] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Sistema] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Sistema] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Sistema] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Sistema] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Sistema] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Sistema] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Sistema] SET RECOVERY FULL 
GO
ALTER DATABASE [Sistema] SET  MULTI_USER 
GO
ALTER DATABASE [Sistema] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Sistema] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Sistema] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Sistema] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Sistema] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Sistema] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Sistema] SET QUERY_STORE = OFF
GO
USE [Sistema]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Codoperador] [nvarchar](20) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[Clave] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Pregunta] [nvarchar](50) NOT NULL,
	[Respuesta] [nvarchar](50) NOT NULL,
	[Fcreau] [datetime2](7) NOT NULL,
	[Nivel] [int] NOT NULL,
	[Estado] [nvarchar](50) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Codoperador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[compras_operador]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[compras_operador]
AS
SELECT        TOP (100) PERCENT u.nombre, c.numcompra, c.fecha, c.peso AS peso_total, c.total, c.codcaja, c.hora, dt.kilate, dt.peso, dt.importe, dt.preciok, c.codcliente, c.codagencia
FROM            dbo.compras AS c INNER JOIN
                         dbo.det_compra AS dt ON c.numcompra = dt.numcompra AND c.codagencia = dt.codagencia INNER JOIN
                         dbo.usuario AS u ON c.usuario = u.usuario
WHERE        (c.codestado <> 3)

GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[Codcliente] [nvarchar](20) NOT NULL,
	[Nombres] [nvarchar](35) NOT NULL,
	[Apellidos] [nvarchar](50) NULL,
	[Numcedula] [nvarchar](50) NOT NULL,
	[FEmision] [datetime2](7) NULL,
	[FVencimiento] [datetime2](7) NULL,
	[Direccion] [varchar](max) NOT NULL,
	[FNacimiento] [datetime2](7) NULL,
	[Estadocivil] [nvarchar](20) NULL,
	[Ciudad] [nvarchar](50) NULL,
	[Telefono] [nvarchar](50) NOT NULL,
	[Celular] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[FIngreso] [datetime2](7) NULL,
	[Ocupacion] [nvarchar](50) NULL,
	[DireccionNegocio] [varchar](max) NULL,
	[TiempoNeg] [nvarchar](50) NULL,
	[OtraAe] [nvarchar](50) NULL,
	[DescOtra] [varchar](max) NULL,
	[NomCuenta] [nvarchar](50) NULL,
	[NumCuenta] [nvarchar](50) NULL,
	[NomBanco] [nvarchar](50) NULL,
	[MontoMensual] [decimal](19, 4) NULL,
	[TotalOperaciones] [decimal](12, 3) NULL,
	[ActuaPor] [nvarchar](50) NULL,
	[NombreTercero] [nvarchar](100) NULL,
	[DireccionTercero] [varchar](max) NULL,
	[Pica] [int] NULL,
	[Ocupacion2] [nvarchar](50) NULL,
	[Idtipodocumento] [int] NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Codcliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[descargueByCompra]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[descargueByCompra]
AS
SELECT        d.dgnumdes, d.dgfecdes, c.numcompra, c.fecha, c.codcliente, cli.nombres, cli.apellidos, c.peso, c.total, d.dgcancom AS Cantcompra
FROM            dbo.descargues AS d INNER JOIN
                         dbo.compras AS c ON d.dgnumdes = c.dgnumdes INNER JOIN
                         dbo.Cliente AS cli ON c.codcliente = cli.codcliente


GO
/****** Object:  Table [dbo].[Detacaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detacaja](
	[Fecha] [datetime2](7) NOT NULL,
	[Idcaja] [int] NOT NULL,
	[Idmov] [int] NOT NULL,
	[Hora] [nvarchar](50) NOT NULL,
	[Concepto] [nvarchar](250) NULL,
	[Efectivo] [decimal](18, 2) NULL,
	[Referencia] [nvarchar](250) NULL,
	[Cheque] [decimal](18, 2) NULL,
	[Transferencia] [decimal](18, 2) NULL,
	[Codcaja] [nvarchar](10) NULL,
	[Tipocambio] [decimal](12, 4) NULL,
 CONSTRAINT [PK_Detacaja] PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mcaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mcaja](
	[Idcaja] [int] IDENTITY(1,1) NOT NULL,
	[Codcaja] [nvarchar](10) NOT NULL,
	[Codagencia] [nvarchar](10) NULL,
	[Fecha] [datetime2](7) NULL,
	[Sinicial] [decimal](18, 2) NULL,
	[Entrada] [decimal](18, 2) NULL,
	[Salida] [decimal](18, 2) NULL,
	[Sfinal] [decimal](18, 2) NULL,
	[Estado] [int] NULL,
 CONSTRAINT [PK_Mcaja] PRIMARY KEY CLUSTERED 
(
	[Idcaja] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MovimientosCajaSelect]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create view [dbo].[MovimientosCajaSelect]
as
select mc.estado,mc.codcaja,dc.hora,dc.fecha,dc.concepto,dc.efectivo,dc.cheque,dc.transferencia
 from detacaja dc join mcaja mc on dc.idcaja = mc.idcaja 



GO
/****** Object:  Table [dbo].[Movcaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movcaja](
	[Idmov] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
	[Codrubro] [int] NULL,
 CONSTRAINT [PK_Movcaja] PRIMARY KEY CLUSTERED 
(
	[Idmov] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rubro]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rubro](
	[Descrubro] [nvarchar](250) NOT NULL,
	[Naturaleza] [int] NULL,
	[Codrubro] [int] NOT NULL,
 CONSTRAINT [PK_Rubro] PRIMARY KEY CLUSTERED 
(
	[Descrubro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[rptMovimientosCaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[rptMovimientosCaja]
as
select r.descrubro,m.descripcion,sum(dc.efectivo) as Efectivo,sum(dc.cheque) as cheque,
sum(dc.transferencia) as transferencia,dc.fecha 
from detacaja dc
join movcaja m on dc.idmov = m.idmov
join Rubro r on m.codrubro = r.codrubro
group by m.descripcion,dc.fecha,r.descrubro

GO
/****** Object:  View [dbo].[TransaccionEfectivo]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[TransaccionEfectivo]
AS
select c.numcompra,cli.codcliente, cli.nombres,cli.apellidos,c.fecha,
iif(c.efectivo>0,'Efectivo',iif(c.cheque>0,'Cheque','Transferencia')) as FormaPago,c.total
from compras c join Cliente cli on c.codcliente = cli.codcliente
where c.efectivo >0 and c.transferencia=0 and c.adelantos=0 and c.cheque=0


GO
/****** Object:  View [dbo].[vdetacaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vdetacaja]
as
select dc.idcaja,dc.idmov,dc.hora,dc.fecha,dc.concepto,
iif(r.naturaleza=1,dc.efectivo,-1*dc.efectivo) as efectivo,IIF(r.naturaleza=1,dc.cheque,-1*dc.cheque) as cheque,
iif(r.naturaleza=1,dc.transferencia,-1*dc.transferencia) as transferencia, dc.codcaja,dc.referencia,
mc.sinicial,mc.sfinal
from detacaja dc
join mcaja mc on dc.idcaja = mc.idcaja
join movcaja m on dc.idmov = m.idmov
join rubro r on m.codrubro = r.codrubro
where dc.codcaja = mc.codcaja and mc.estado = 1



GO
/****** Object:  Table [dbo].[TipoCambio]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCambio](
	[Fecha] [datetime2](7) NOT NULL,
	[Tipocambio] [decimal](12, 4) NOT NULL,
	[Hora] [nvarchar](50) NULL,
	[PrecioOro] [decimal](18, 4) NULL,
 CONSTRAINT [PK_TipoCambio] PRIMARY KEY CLUSTERED 
(
	[Fecha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vVariacionesCliente]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vVariacionesCliente]
AS
select cli.codcliente,cli.nombres, cli.apellidos, DATENAME(MONTH,c.fecha) as mes,
(cli.monto_mensual *tc.tipocambio) as Monto,SUM(c.total) as 'Monto Mensual',
(SUM(c.total)/(cli.monto_mensual *tc.tipocambio))*100 as Variacion,c.fecha
from compras c join det_compra dt on c.numcompra = dt.numcompra
join Cliente cli on c.codcliente = cli.codcliente join TipoCambio tc on c.fecha = tc.fecha
group by cli.codcliente, cli.nombres,cli.apellidos, DATENAME(month,c.fecha),cli.monto_mensual,tc.tipocambio,c.fecha




GO
/****** Object:  Table [dbo].[Adelanto]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Adelanto](
	[Idsalida] [nvarchar](20) NOT NULL,
	[Codcliente] [nvarchar](20) NOT NULL,
	[Numcompra] [nvarchar](4000) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Monto] [decimal](18, 2) NULL,
	[Saldo] [decimal](18, 2) NOT NULL,
	[Efectivo] [decimal](18, 2) NULL,
	[Cheque] [decimal](18, 2) NULL,
	[Transferencia] [decimal](18, 2) NULL,
	[Codcaja] [nvarchar](10) NULL,
	[Usuario] [nvarchar](50) NULL,
	[MontoLetras] [nvarchar](4000) NULL,
	[Hora] [nvarchar](50) NULL,
	[Codmoneda] [int] NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Adelanto] PRIMARY KEY CLUSTERED 
(
	[Idsalida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Agencia]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agencia](
	[Codagencia] [nvarchar](10) NOT NULL,
	[Nomagencia] [nvarchar](150) NOT NULL,
	[Diragencia] [nvarchar](4000) NOT NULL,
	[Disagencia] [nvarchar](100) NOT NULL,
	[Telagencia] [nvarchar](20) NOT NULL,
	[Numcompra] [int] NULL,
	[Logo] [varbinary](max) NULL,
	[Ruc] [varchar](50) NULL,
 CONSTRAINT [PK_Agencia] PRIMARY KEY CLUSTERED 
(
	[Codagencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Caja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Caja](
	[Codcaja] [nvarchar](10) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[Codagencia] [nvarchar](10) NULL,
 CONSTRAINT [PK_Caja] PRIMARY KEY CLUSTERED 
(
	[Codcaja] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CierrePrecio]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CierrePrecio](
	[Codcierre] [int] IDENTITY(1,1) NOT NULL,
	[Codcliente] [nvarchar](20) NOT NULL,
	[Status] [bit] NOT NULL,
	[OnzasFinas] [decimal](12, 3) NOT NULL,
	[GramosFinos] [decimal](12, 2) NOT NULL,
	[PrecioOro] [decimal](12, 2) NOT NULL,
	[PrecioBase] [decimal](12, 2) NOT NULL,
	[SaldoOnzas] [decimal](12, 3) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Margen] [decimal](12, 2) NOT NULL,
 CONSTRAINT [PK_CierrePrecio] PRIMARY KEY CLUSTERED 
(
	[Codcierre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Compra]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compra](
	[Numcompra] [nvarchar](20) NOT NULL,
	[Codagencia] [nvarchar](10) NOT NULL,
	[Codcliente] [nvarchar](20) NOT NULL,
	[Codcaja] [nvarchar](10) NOT NULL,
	[Peso] [decimal](11, 2) NOT NULL,
	[Codmoneda] [int] NOT NULL,
	[Total] [decimal](15, 2) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Codestado] [int] NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
	[Hora] [nvarchar](50) NOT NULL,
	[FormaPago] [nvarchar](50) NOT NULL,
	[Dgnumdes] [int] NOT NULL,
	[Efectivo] [decimal](18, 2) NOT NULL,
	[Cheque] [decimal](18, 2) NOT NULL,
	[Transferencia] [decimal](18, 2) NOT NULL,
	[PorCobrar] [decimal](18, 2) NOT NULL,
	[PorPagar] [decimal](18, 2) NOT NULL,
	[Adelantos] [decimal](18, 2) NOT NULL,
	[SaldoPorPagar] [decimal](18, 2) NOT NULL,
	[SaldoAdelanto] [decimal](12, 3) NULL,
	[SaldoAdelantoDolares] [decimal](12, 3) NULL,
	[Nocontrato] [int] NULL,
	[subtotal] [decimal](18, 2) NULL,
	[descuento] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Compra] PRIMARY KEY CLUSTERED 
(
	[Numcompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComprasAdelanto]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComprasAdelanto](
	[IdcomprasAdelantos] [int] IDENTITY(1,1) NOT NULL,
	[Numcompra] [nvarchar](20) NOT NULL,
	[Idadelanto] [nvarchar](20) NOT NULL,
	[Codcliente] [nvarchar](20) NULL,
	[Sinicial] [decimal](18, 2) NOT NULL,
	[Monto] [decimal](18, 3) NOT NULL,
	[Sfinal] [decimal](18, 2) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Codcaja] [nvarchar](10) NOT NULL,
	[Usuario] [nvarchar](20) NOT NULL,
	[Hora] [time](0) NOT NULL,
	[Codagencia] [nvarchar](10) NULL,
	[Codmoneda] [int] NULL,
 CONSTRAINT [PK_ComprasAdelanto] PRIMARY KEY CLUSTERED 
(
	[IdcomprasAdelantos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComprasOperador]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComprasOperador](
	[Nombre] [nvarchar](100) NOT NULL,
	[Numcompra] [nvarchar](20) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[PesoTotal] [decimal](11, 2) NOT NULL,
	[Total] [decimal](15, 2) NOT NULL,
	[Codcaja] [nvarchar](10) NOT NULL,
	[Hora] [nvarchar](50) NOT NULL,
	[Kilate] [nvarchar](20) NOT NULL,
	[Peso] [decimal](18, 2) NOT NULL,
	[Importe] [decimal](18, 2) NULL,
	[Preciok] [decimal](18, 2) NOT NULL,
	[Codcliente] [nvarchar](20) NOT NULL,
	[Codagencia] [nvarchar](10) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Descargue]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Descargue](
	[Dgnumdes] [int] NOT NULL,
	[Dgcodage] [nchar](4) NOT NULL,
	[Dgcodcaj] [nchar](7) NOT NULL,
	[Dgusuari] [nchar](10) NOT NULL,
	[Dgcancom] [int] NOT NULL,
	[Dgpesbrt] [decimal](11, 2) NOT NULL,
	[Dgpesntt] [decimal](11, 2) NOT NULL,
	[Dgimptcom] [decimal](11, 2) NOT NULL,
	[Dgfecdes] [datetime2](7) NOT NULL,
	[Dgfecgen] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Descargue] PRIMARY KEY CLUSTERED 
(
	[Dgnumdes] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Detacierre]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detacierre](
	[Codcierre] [int] NOT NULL,
	[Numcompra] [nvarchar](20) NOT NULL,
	[Onzas] [decimal](12, 3) NOT NULL,
	[Saldo] [decimal](12, 3) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[Cantidad] [decimal](12, 3) NOT NULL,
	[Codagencia] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Detacierre] PRIMARY KEY CLUSTERED 
(
	[Codcierre] ASC,
	[Numcompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetCompra]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetCompra](
	[Numcompra] [nvarchar](20) NOT NULL,
	[Linea] [int] NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
	[Kilate] [nvarchar](20) NOT NULL,
	[Preciok] [decimal](18, 2) NOT NULL,
	[Peso] [decimal](18, 2) NOT NULL,
	[Importe] [decimal](18, 2) NULL,
	[Kilshowdoc] [nvarchar](20) NULL,
	[Numdescargue] [int] NULL,
	[Codagencia] [nvarchar](20) NULL,
	[Fecha] [datetime2](7) NULL,
 CONSTRAINT [PK_DetCompra] PRIMARY KEY CLUSTERED 
(
	[Numcompra] ASC,
	[Linea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Estado]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estado](
	[Codestado] [int] NOT NULL,
	[DescEstado] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Estado] PRIMARY KEY CLUSTERED 
(
	[Codestado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FormaPago]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FormaPago](
	[Idmov] [nvarchar](20) NOT NULL,
	[Descripcion] [decimal](12, 2) NULL,
	[Naturaleza] [decimal](12, 2) NULL,
 CONSTRAINT [PK_FormaPago] PRIMARY KEY CLUSTERED 
(
	[Idmov] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Id]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Id](
	[Codcliente] [int] NOT NULL,
	[Codagencia] [int] NULL,
	[Numcompra] [int] NULL,
	[Idadelanto] [int] NULL,
	[Idcompras] [int] NULL,
	[IdAdelantos] [int] NULL,
	[SaldoAnterior] [int] NULL,
	[CierreCompra] [int] NULL,
	[PrestamoEgreso] [int] NULL,
	[PrestamoIngreso] [int] NULL,
	[AnularCompra] [int] NULL,
	[AnularAdelanto] [int] NULL,
	[VariasCompras] [bit] NULL,
	[Recibe] [nvarchar](50) NULL,
	[PagoAdelanto] [int] NULL,
	[Idreserva] [int] NULL,
	[Backup] [datetime2](7) NULL,
	[Cordobas] [int] NULL,
	[Dolares] [int] NULL,
	[Nocontrato] [int] NULL,
	[Logo] [varbinary](max) NULL,
 CONSTRAINT [PK_Id] PRIMARY KEY CLUSTERED 
(
	[Codcliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Liquidacion2015]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Liquidacion2015](
	[Numcompra] [nvarchar](255) NOT NULL,
	[Codcliente] [nvarchar](255) NULL,
	[Codcaja] [nvarchar](255) NULL,
	[Peso] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[Fecha] [datetime2](7) NULL,
	[Usuario] [nvarchar](255) NULL,
	[Hora] [nvarchar](255) NULL,
	[Cliente] [nvarchar](255) NULL,
 CONSTRAINT [PK_Liquidacion2015] PRIMARY KEY CLUSTERED 
(
	[Numcompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listado]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listado](
	[Solicitud] [int] NULL,
	[Ranking] [int] NULL,
	[Cliente] [nvarchar](255) NULL,
	[Fecha] [datetime2](7) NULL,
	[Recibo] [nvarchar](255) NULL,
	[Total] [decimal](18, 2) NULL,
	[Codigo] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Moneda]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Moneda](
	[Codmoneda] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](250) NULL,
	[Simbolo] [nvarchar](50) NULL,
	[Fecha] [datetime2](7) NULL,
	[Default] [bit] NULL,
 CONSTRAINT [PK_Moneda] PRIMARY KEY CLUSTERED 
(
	[Codmoneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PagosAdelantado]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PagosAdelantado](
	[IdPagoefec] [int] NOT NULL,
	[Idingreso] [nchar](14) NOT NULL,
	[Codagencia] [nchar](4) NULL,
	[FechaopParcial] [datetime2](7) NOT NULL,
	[ValorParcialpagado] [decimal](11, 2) NOT NULL,
	[Usuario] [nchar](10) NOT NULL,
	[HoraOp] [datetime2](7) NOT NULL,
	[EstadoOp] [nchar](3) NOT NULL,
	[CajaRegadel] [nvarchar](8) NULL,
 CONSTRAINT [PK_PagosAdelantado] PRIMARY KEY CLUSTERED 
(
	[IdPagoefec] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pica]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pica](
	[Codcliente] [nvarchar](20) NOT NULL,
	[NombreEntidad] [nvarchar](50) NOT NULL,
	[TipoRelacion] [nvarchar](50) NULL,
	[TiempoMantener] [nvarchar](50) NULL,
	[IngresoMensual] [decimal](19, 4) NULL,
 CONSTRAINT [PK_Pica] PRIMARY KEY CLUSTERED 
(
	[Codcliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Precio]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Precio](
	[Codcliente] [nvarchar](20) NOT NULL,
	[Kilate] [decimal](12, 2) NOT NULL,
	[Precio1] [decimal](12, 2) NOT NULL,
	[Gramos] [decimal](12, 3) NOT NULL,
	[PrecioOro] [decimal](12, 3) NOT NULL,
 CONSTRAINT [PK_Precio] PRIMARY KEY CLUSTERED 
(
	[Codcliente] ASC,
	[Kilate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrecioKilate]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrecioKilate](
	[DescKilate] [nvarchar](50) NOT NULL,
	[KilatePeso] [decimal](18, 2) NOT NULL,
	[PrecioKilate1] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_PrecioKilate] PRIMARY KEY CLUSTERED 
(
	[DescKilate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReporteCaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReporteCaja](
	[Codagencia] [nvarchar](10) NOT NULL,
	[Codrubro] [nvarchar](10) NOT NULL,
	[Codoperador] [nvarchar](10) NOT NULL,
	[Monto] [decimal](19, 4) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RubrosCaja]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RubrosCaja](
	[Rucodrub] [nchar](2) NOT NULL,
	[Rucodope] [nchar](2) NOT NULL,
	[Rudescri] [nchar](60) NOT NULL,
	[Rudeha] [nchar](1) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoDocumento]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoDocumento](
	[Idtipodocumento] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Simbolo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TipoDocumento] PRIMARY KEY CLUSTERED 
(
	[Idtipodocumento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPrecio]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPrecio](
	[idtipoprecio] [int] IDENTITY(1,1) NOT NULL,
	[descripcion] [varchar](50) NULL,
	[precio] [decimal](18, 2) NULL,
 CONSTRAINT [PK_TipoPrecio_idtipoprecio] PRIMARY KEY CLUSTERED 
(
	[idtipoprecio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPreciosPagado]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPreciosPagado](
	[Codprecio] [nvarchar](10) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Pesoinicial] [nvarchar](20) NOT NULL,
	[Pesofinal] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TipoPreciosPagado] PRIMARY KEY CLUSTERED 
(
	[Codprecio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tmpprecio]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tmpprecio](
	[Codcierre] [int] NOT NULL,
	[Linea] [tinyint] NOT NULL,
	[Codcliente] [nvarchar](20) NOT NULL,
	[Cantidad] [decimal](12, 3) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Tmpprecio] PRIMARY KEY CLUSTERED 
(
	[Codcierre] ASC,
	[Linea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Adelanto] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ('Pequeño Minero') FOR [Ocupacion2]
GO
ALTER TABLE [dbo].[Cliente] ADD  DEFAULT ((0)) FOR [Idtipodocumento]
GO
ALTER TABLE [dbo].[Compra] ADD  DEFAULT ((0)) FOR [SaldoAdelanto]
GO
ALTER TABLE [dbo].[Detacaja] ADD  DEFAULT ((1)) FOR [Tipocambio]
GO
ALTER TABLE [dbo].[DetCompra] ADD  DEFAULT ((0)) FOR [Numdescargue]
GO
ALTER TABLE [dbo].[Moneda] ADD  DEFAULT ((0)) FOR [Default]
GO
/****** Object:  StoredProcedure [dbo].[anularCompra]    Script Date: 8/10/2024 1:28:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[anularCompra]
@numeroCompra varchar(20),@agencia varchar(20)
as
update compras set peso = 0, total = 0, codestado = 0, efectivo = 0,cheque = 0,transferencia = 0,por_cobrar= 0,
 por_pagar = 0, adelantos = 0, saldo_por_pagar = 0 where numcompra = @numeroCompra and codagencia = @agencia
 update det_compra set preciok = 0, peso = 0, importe = 0, kilshowdoc = ''
 where numcompra = @numeroCompra and codagencia = @agencia
 delete from compras_adelantos where numcompra = @numeroCompra
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "d"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 259
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 0
               Left = 276
               Bottom = 313
               Right = 485
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "cli"
            Begin Extent = 
               Top = 1
               Left = 585
               Bottom = 130
               Right = 794
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 11
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'descargueByCompra'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'descargueByCompra'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Cliente"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 29
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TransaccionEfectivo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TransaccionEfectivo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "dt"
            Begin Extent = 
               Top = 6
               Left = 301
               Bottom = 135
               Right = 526
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "cli"
            Begin Extent = 
               Top = 6
               Left = 564
               Bottom = 135
               Right = 789
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "tc"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 250
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2370
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vVariacionesCliente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vVariacionesCliente'
GO
USE [master]
GO
ALTER DATABASE [Sistema] SET  READ_WRITE 
GO
