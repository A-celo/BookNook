USE [master]
GO
/****** Object:  Database [BookNook]    Script Date: 11/28/2024 12:08:17 AM ******/
CREATE DATABASE [BookNook]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BookNook', FILENAME = N'/var/opt/mssql/data/BookNook.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BookNook_log', FILENAME = N'/var/opt/mssql/data/BookNook_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BookNook] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BookNook].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BookNook] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BookNook] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BookNook] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BookNook] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BookNook] SET ARITHABORT OFF 
GO
ALTER DATABASE [BookNook] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BookNook] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BookNook] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BookNook] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BookNook] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BookNook] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BookNook] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BookNook] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BookNook] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BookNook] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BookNook] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BookNook] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BookNook] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BookNook] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BookNook] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BookNook] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BookNook] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BookNook] SET RECOVERY FULL 
GO
ALTER DATABASE [BookNook] SET  MULTI_USER 
GO
ALTER DATABASE [BookNook] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BookNook] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BookNook] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BookNook] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BookNook] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BookNook] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BookNook', N'ON'
GO
ALTER DATABASE [BookNook] SET QUERY_STORE = ON
GO
ALTER DATABASE [BookNook] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BookNook]
GO
/****** Object:  Table [dbo].[estado_lectura]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[estado_lectura](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[descripcion] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[etiquetas]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[etiquetas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[creado_en] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[etiquetas_libro]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[etiquetas_libro](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[libro_id] [int] NULL,
	[etiqueta_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[formatos_libro]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[formatos_libro](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[descripcion] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[lecturas]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lecturas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NOT NULL,
	[libro_id] [int] NOT NULL,
	[estado_id] [int] NOT NULL,
	[fecha_inicio] [datetime] NULL,
	[fecha_fin] [datetime] NULL,
	[pagina_actual] [int] NULL,
	[calificacion] [decimal](2, 1) NULL,
	[notas] [text] NULL,
	[creado_en] [datetime] NULL,
	[actualizado_en] [datetime] NULL,
 CONSTRAINT [PK__lecturas__3213E83FADFCFA5E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[libros]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[libros](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [varchar](255) NOT NULL,
	[autor] [varchar](255) NOT NULL,
	[ano_publicacion] [int] NULL,
	[idioma] [varchar](50) NULL,
	[genero] [varchar](255) NULL,
	[subgenero] [varchar](255) NULL,
	[numero_paginas] [int] NULL,
	[imagen_portada] [varchar](255) NULL,
	[creado_en] [datetime] NULL,
	[actualizado_en] [datetime] NULL,
 CONSTRAINT [PK__libros__3213E83FF7C6515C] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[libros_en_lista]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[libros_en_lista](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[lista_id] [int] NULL,
	[libro_id] [int] NULL,
	[formato_id] [int] NULL,
	[orden] [int] NULL,
	[nota] [text] NULL,
	[creado_en] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[listas_libros]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[listas_libros](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NULL,
	[nombre_lista] [varchar](255) NOT NULL,
	[descripcion] [text] NULL,
	[creado_en] [datetime] NULL,
	[actualizado_en] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[objetivos_lectura]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[objetivos_lectura](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[usuario_id] [int] NULL,
	[año] [int] NULL,
	[objetivo_anual] [int] NULL,
	[progreso_anual] [int] NULL,
	[creado_en] [datetime] NULL,
	[actualizado_en] [datetime] NULL,
	[libros_leidos] [int] NULL,
	[libros_restantes] [int] NULL,
 CONSTRAINT [PK__objetivo__3213E83FF080A55D] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[usuarios]    Script Date: 11/28/2024 12:08:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[usuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](255) NOT NULL,
	[apellido] [varchar](255) NOT NULL,
	[correo] [varchar](255) NOT NULL,
	[contraseña] [varchar](255) NOT NULL,
 CONSTRAINT [PK__usuarios__3213E83FCB301E93] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[estado_lectura] ON 

INSERT [dbo].[estado_lectura] ([id], [nombre], [descripcion]) VALUES (1, N'Leído', NULL)
INSERT [dbo].[estado_lectura] ([id], [nombre], [descripcion]) VALUES (2, N'Por Leer', NULL)
INSERT [dbo].[estado_lectura] ([id], [nombre], [descripcion]) VALUES (3, N'Leyendo', NULL)
INSERT [dbo].[estado_lectura] ([id], [nombre], [descripcion]) VALUES (4, N'DNF', NULL)
SET IDENTITY_INSERT [dbo].[estado_lectura] OFF
GO
SET IDENTITY_INSERT [dbo].[formatos_libro] ON 

INSERT [dbo].[formatos_libro] ([id], [nombre], [descripcion]) VALUES (1, N'Físico', NULL)
INSERT [dbo].[formatos_libro] ([id], [nombre], [descripcion]) VALUES (2, N'Dígital', NULL)
INSERT [dbo].[formatos_libro] ([id], [nombre], [descripcion]) VALUES (3, N'Audiolibro', NULL)
SET IDENTITY_INSERT [dbo].[formatos_libro] OFF
GO
SET IDENTITY_INSERT [dbo].[lecturas] ON 

INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (14, 2, 17, 1, CAST(N'2024-08-10T00:00:00.000' AS DateTime), CAST(N'2024-08-18T00:00:00.000' AS DateTime), NULL, CAST(5.0 AS Decimal(2, 1)), NULL, NULL, NULL)
INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (15, 2, 20, 2, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-11-26T20:54:35.190' AS DateTime), CAST(N'2024-11-26T20:54:35.190' AS DateTime))
INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (16, 2, 22, 1, CAST(N'2024-11-05T00:00:00.000' AS DateTime), CAST(N'2024-11-08T00:00:00.000' AS DateTime), NULL, CAST(5.0 AS Decimal(2, 1)), N'notas', CAST(N'2024-11-26T21:30:45.540' AS DateTime), CAST(N'2024-11-26T21:30:45.540' AS DateTime))
INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (17, 2, 24, 1, CAST(N'2024-06-12T00:00:00.000' AS DateTime), CAST(N'2024-06-20T00:00:00.000' AS DateTime), NULL, CAST(4.0 AS Decimal(2, 1)), NULL, CAST(N'2024-11-27T12:11:52.967' AS DateTime), CAST(N'2024-11-27T12:11:52.967' AS DateTime))
INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (18, 2, 25, 1, CAST(N'2024-09-03T00:00:00.000' AS DateTime), CAST(N'2024-09-21T00:00:00.000' AS DateTime), NULL, CAST(2.0 AS Decimal(2, 1)), NULL, CAST(N'2024-11-27T12:14:10.967' AS DateTime), CAST(N'2024-11-27T12:14:10.967' AS DateTime))
INSERT [dbo].[lecturas] ([id], [usuario_id], [libro_id], [estado_id], [fecha_inicio], [fecha_fin], [pagina_actual], [calificacion], [notas], [creado_en], [actualizado_en]) VALUES (19, 2, 20, 3, CAST(N'2024-11-27T00:00:00.000' AS DateTime), NULL, 80, NULL, NULL, CAST(N'2024-11-27T12:14:37.777' AS DateTime), CAST(N'2024-11-27T12:14:37.777' AS DateTime))
SET IDENTITY_INSERT [dbo].[lecturas] OFF
GO
SET IDENTITY_INSERT [dbo].[libros] ON 

INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (17, N'Normal People', N'Sally Rooney', 2018, N'Inglés', N'Ficción', N'Novela', 273, N'/imagen_libro/4b6629e0-0d1b-400f-a6b7-a87c8fc2cfba_normal_people.jpg', CAST(N'2024-11-25T19:04:39.333' AS DateTime), CAST(N'2024-11-27T22:46:56.237' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (18, N'Los Juegos Del Hambre', N'Suzanne Collins', 2008, N'Español', N'Ciencia Ficción', N'Juvenil', 374, N'/imagen_libro/dbff460a-14c4-4907-8264-a5a4c9e82286_juegos_hambre.jpg', CAST(N'2024-11-25T19:05:49.973' AS DateTime), CAST(N'2024-11-25T19:05:49.973' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (19, N'The Life Impossible', N'Matt Haig', 2024, N'Inglés', N'Ficción', N'Fantasía', 324, N'/imagen_libro/d4628889-d72f-41a6-a438-04b66b644f4f_life_impo.jpg', CAST(N'2024-11-25T19:07:03.597' AS DateTime), CAST(N'2024-11-25T19:07:03.597' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (20, N'Midnight Library', N'Matt Haig', 2020, N'Inglés', N'Ficción', N'Adulto', 288, N'/imagen_libro/909bf9a6-ecdf-423a-abe7-95555b1d60d3_mdn.jpg', CAST(N'2024-11-25T19:07:51.693' AS DateTime), CAST(N'2024-11-25T19:07:51.693' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (21, N'Pride And Prejudice', N'Jane Austen', 1813, N'Inglés', N'Literatura', N'Novela', 279, N'/imagen_libro/b43bdff1-db5e-42b7-b2b2-07efa83be242_ppp.jpg', CAST(N'2024-11-25T19:08:52.000' AS DateTime), CAST(N'2024-11-27T23:38:07.350' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (22, N'Divine Rivals', N'Rebecca Ross', 2023, N'Inglés', N'Romance', N'Juvenil', 357, N'/imagen_libro/ab4d66b0-ee82-4639-b950-0ba3cd6a272d_divine_rivals.jpg', CAST(N'2024-11-25T19:09:36.523' AS DateTime), CAST(N'2024-11-25T19:09:36.523' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (23, N'Crown of Midnight', N'Sarah J. Maas', 2013, N'Inglés', N'Fantasía', N'Ficción', 420, N'/imagen_libro/a611de7a-c5ab-4aee-98ec-b795e1f1918b_crownm.jpg', CAST(N'2024-11-26T16:23:29.917' AS DateTime), CAST(N'2024-11-26T16:23:29.917' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (24, N'Intermezzo', N'Sally Rooney', 2024, N'Inglés', N'Ficción', N'Novela', 454, N'/imagen_libro/ad61df08-6e88-4d86-a025-d9e6e6698dea_Intermezzo.jpg', CAST(N'2024-11-27T12:11:14.697' AS DateTime), CAST(N'2024-11-27T12:11:14.697' AS DateTime))
INSERT [dbo].[libros] ([id], [titulo], [autor], [ano_publicacion], [idioma], [genero], [subgenero], [numero_paginas], [imagen_portada], [creado_en], [actualizado_en]) VALUES (25, N'The Wedding People', N'Alison Espach', 2024, N'Inglés', N'Ficción', N'Romance', 384, N'/imagen_libro/1407a3f6-74cd-468e-b7c1-8180b7e87a80_wp.jpg', CAST(N'2024-11-27T12:13:16.863' AS DateTime), CAST(N'2024-11-27T12:13:16.863' AS DateTime))
SET IDENTITY_INSERT [dbo].[libros] OFF
GO
SET IDENTITY_INSERT [dbo].[objetivos_lectura] ON 

INSERT [dbo].[objetivos_lectura] ([id], [usuario_id], [año], [objetivo_anual], [progreso_anual], [creado_en], [actualizado_en], [libros_leidos], [libros_restantes]) VALUES (2, 2, 2024, 20, 4, NULL, CAST(N'2024-11-27T23:58:47.180' AS DateTime), 4, 16)
SET IDENTITY_INSERT [dbo].[objetivos_lectura] OFF
GO
SET IDENTITY_INSERT [dbo].[usuarios] ON 

INSERT [dbo].[usuarios] ([id], [nombre], [apellido], [correo], [contraseña]) VALUES (1, N'Angelica', N'Lopez', N'acerecer5@gmail.com', N'8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92')
INSERT [dbo].[usuarios] ([id], [nombre], [apellido], [correo], [contraseña]) VALUES (2, N'Iris', N'Smith', N'iriss@email.com', N'049ec1af7c1332193d602986f2fdad5b4d1c2ff90e5cdc65388c794c1f10226b')
SET IDENTITY_INSERT [dbo].[usuarios] OFF
GO
ALTER TABLE [dbo].[etiquetas_libro]  WITH CHECK ADD FOREIGN KEY([etiqueta_id])
REFERENCES [dbo].[etiquetas] ([id])
GO
ALTER TABLE [dbo].[etiquetas_libro]  WITH CHECK ADD  CONSTRAINT [FK__etiquetas__libro__5165187F] FOREIGN KEY([libro_id])
REFERENCES [dbo].[libros] ([id])
GO
ALTER TABLE [dbo].[etiquetas_libro] CHECK CONSTRAINT [FK__etiquetas__libro__5165187F]
GO
ALTER TABLE [dbo].[lecturas]  WITH CHECK ADD  CONSTRAINT [FK__lecturas__estado__4F7CD00D] FOREIGN KEY([estado_id])
REFERENCES [dbo].[estado_lectura] ([id])
GO
ALTER TABLE [dbo].[lecturas] CHECK CONSTRAINT [FK__lecturas__estado__4F7CD00D]
GO
ALTER TABLE [dbo].[lecturas]  WITH CHECK ADD  CONSTRAINT [FK__lecturas__libro___4D94879B] FOREIGN KEY([libro_id])
REFERENCES [dbo].[libros] ([id])
GO
ALTER TABLE [dbo].[lecturas] CHECK CONSTRAINT [FK__lecturas__libro___4D94879B]
GO
ALTER TABLE [dbo].[lecturas]  WITH CHECK ADD  CONSTRAINT [FK__lecturas__usuari__4CA06362] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
ALTER TABLE [dbo].[lecturas] CHECK CONSTRAINT [FK__lecturas__usuari__4CA06362]
GO
ALTER TABLE [dbo].[libros_en_lista]  WITH CHECK ADD FOREIGN KEY([formato_id])
REFERENCES [dbo].[formatos_libro] ([id])
GO
ALTER TABLE [dbo].[libros_en_lista]  WITH CHECK ADD  CONSTRAINT [FK__libros_en__libro__4AB81AF0] FOREIGN KEY([libro_id])
REFERENCES [dbo].[libros] ([id])
GO
ALTER TABLE [dbo].[libros_en_lista] CHECK CONSTRAINT [FK__libros_en__libro__4AB81AF0]
GO
ALTER TABLE [dbo].[libros_en_lista]  WITH CHECK ADD FOREIGN KEY([lista_id])
REFERENCES [dbo].[listas_libros] ([id])
GO
ALTER TABLE [dbo].[listas_libros]  WITH CHECK ADD  CONSTRAINT [FK__listas_li__usuar__48CFD27E] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
ALTER TABLE [dbo].[listas_libros] CHECK CONSTRAINT [FK__listas_li__usuar__48CFD27E]
GO
ALTER TABLE [dbo].[objetivos_lectura]  WITH CHECK ADD  CONSTRAINT [FK__objetivos__usuar__5070F446] FOREIGN KEY([usuario_id])
REFERENCES [dbo].[usuarios] ([id])
GO
ALTER TABLE [dbo].[objetivos_lectura] CHECK CONSTRAINT [FK__objetivos__usuar__5070F446]
GO
USE [master]
GO
ALTER DATABASE [BookNook] SET  READ_WRITE 
GO
