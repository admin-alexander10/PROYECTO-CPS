IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [CategoriaMenus] (
    [IdCategoria] int NOT NULL IDENTITY,
    [NombreDeCategoria] nvarchar(100) NOT NULL,
    [Descripcion] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_CategoriaMenus] PRIMARY KEY ([IdCategoria])
);
GO

CREATE TABLE [Clientes] (
    [IdCliente] int NOT NULL IDENTITY,
    [PrimNombre] nvarchar(50) NOT NULL,
    [SegNombre] nvarchar(50) NOT NULL,
    [ApellidoP] nvarchar(50) NOT NULL,
    [ApellidoM] nvarchar(50) NOT NULL,
    [Telefono] nvarchar(20) NOT NULL,
    [Correo] nvarchar(100) NOT NULL,
    [UsuarioId] int NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([IdCliente])
);
GO

CREATE TABLE [Mesas] (
    [IdMesa] int NOT NULL IDENTITY,
    [NumMesa] int NOT NULL,
    [Capacidad] int NOT NULL,
    CONSTRAINT [PK_Mesas] PRIMARY KEY ([IdMesa])
);
GO

CREATE TABLE [MetodosPago] (
    [IdMetodoDePago] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_MetodosPago] PRIMARY KEY ([IdMetodoDePago])
);
GO

CREATE TABLE [Roles] (
    [IdRol] int NOT NULL IDENTITY,
    [NombreRol] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([IdRol])
);
GO

CREATE TABLE [Turnos] (
    [IdTurno] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [HoraInicio] datetime2 NOT NULL,
    [HoraFin] datetime2 NOT NULL,
    CONSTRAINT [PK_Turnos] PRIMARY KEY ([IdTurno])
);
GO

CREATE TABLE [Menus] (
    [IdMenu] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Descripcion] nvarchar(255) NOT NULL,
    [ImagenUrl] nvarchar(255) NOT NULL,
    [CategoriaMenuId] int NOT NULL,
    CONSTRAINT [PK_Menus] PRIMARY KEY ([IdMenu]),
    CONSTRAINT [FK_Menus_CategoriaMenus_CategoriaMenuId] FOREIGN KEY ([CategoriaMenuId]) REFERENCES [CategoriaMenus] ([IdCategoria]) ON DELETE CASCADE
);
GO

CREATE TABLE [Reservas] (
    [IdReserva] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [Hora] datetime2 NOT NULL,
    [Estado] bit NOT NULL,
    [ClienteId] int NOT NULL,
    [MesaId] int NOT NULL,
    CONSTRAINT [PK_Reservas] PRIMARY KEY ([IdReserva]),
    CONSTRAINT [FK_Reservas_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([IdCliente]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reservas_Mesas_MesaId] FOREIGN KEY ([MesaId]) REFERENCES [Mesas] ([IdMesa]) ON DELETE CASCADE
);
GO

CREATE TABLE [Empleados] (
    [IdEmpleado] int NOT NULL IDENTITY,
    [PrimNombre] nvarchar(50) NOT NULL,
    [SegNombre] nvarchar(50) NOT NULL,
    [ApellidoP] nvarchar(50) NOT NULL,
    [ApellidoM] nvarchar(50) NOT NULL,
    [FechaContrato] datetime2 NOT NULL,
    [Telefono] nvarchar(20) NOT NULL,
    [Correo] nvarchar(100) NOT NULL,
    [RolId] int NOT NULL,
    CONSTRAINT [PK_Empleados] PRIMARY KEY ([IdEmpleado]),
    CONSTRAINT [FK_Empleados_Roles_RolId] FOREIGN KEY ([RolId]) REFERENCES [Roles] ([IdRol]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comentarios] (
    [IdComentario] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(255) NOT NULL,
    [FechaComentario] datetime2 NOT NULL,
    [ClienteId] int NOT NULL,
    [MenuId] int NOT NULL,
    CONSTRAINT [PK_Comentarios] PRIMARY KEY ([IdComentario]),
    CONSTRAINT [FK_Comentarios_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([IdCliente]) ON DELETE CASCADE,
    CONSTRAINT [FK_Comentarios_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menus] ([IdMenu]) ON DELETE CASCADE
);
GO

CREATE TABLE [EmpleadoTurnos] (
    [IdEmpleadoTurno] int NOT NULL IDENTITY,
    [FechaTurno] datetime2 NOT NULL,
    [EmpleadoId] int NOT NULL,
    [TurnoId] int NOT NULL,
    CONSTRAINT [PK_EmpleadoTurnos] PRIMARY KEY ([IdEmpleadoTurno]),
    CONSTRAINT [FK_EmpleadoTurnos_Empleados_EmpleadoId] FOREIGN KEY ([EmpleadoId]) REFERENCES [Empleados] ([IdEmpleado]) ON DELETE CASCADE,
    CONSTRAINT [FK_EmpleadoTurnos_Turnos_TurnoId] FOREIGN KEY ([TurnoId]) REFERENCES [Turnos] ([IdTurno]) ON DELETE CASCADE
);
GO

CREATE TABLE [Usuarios] (
    [IdUsuario] int NOT NULL IDENTITY,
    [NombreUsuario] nvarchar(50) NOT NULL,
    [Correo] nvarchar(255) NOT NULL,
    [Contrasena] nvarchar(30) NOT NULL,
    [FechaCreacion] datetime2 NOT NULL,
    [Estado] bit NOT NULL,
    [EmpleadoId] int NULL,
    [ClienteId] int NULL,
    [RolId] int NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([IdUsuario]),
    CONSTRAINT [FK_Usuarios_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([IdCliente]),
    CONSTRAINT [FK_Usuarios_Empleados_EmpleadoId] FOREIGN KEY ([EmpleadoId]) REFERENCES [Empleados] ([IdEmpleado]),
    CONSTRAINT [FK_Usuarios_Roles_RolId] FOREIGN KEY ([RolId]) REFERENCES [Roles] ([IdRol]) ON DELETE CASCADE
);
GO

CREATE TABLE [Pedidos] (
    [IdPedido] int NOT NULL IDENTITY,
    [FechaPedido] datetime2 NOT NULL,
    [Estado] bit NOT NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([IdPedido]),
    CONSTRAINT [FK_Pedidos_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE CASCADE
);
GO

CREATE TABLE [Boletas] (
    [IdBoleta] int NOT NULL IDENTITY,
    [NumBoleta] nvarchar(50) NOT NULL,
    [FechaEmicion] datetime2 NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [PedidoId] int NOT NULL,
    CONSTRAINT [PK_Boletas] PRIMARY KEY ([IdBoleta]),
    CONSTRAINT [FK_Boletas_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([IdPedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [DetalleMenuPedidos] (
    [IdDetallePedido] int NOT NULL IDENTITY,
    [Cantidad] int NOT NULL,
    [PedidoId] int NOT NULL,
    [MenuId] int NOT NULL,
    CONSTRAINT [PK_DetalleMenuPedidos] PRIMARY KEY ([IdDetallePedido]),
    CONSTRAINT [FK_DetalleMenuPedidos_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menus] ([IdMenu]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetalleMenuPedidos_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([IdPedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [Ordenes] (
    [IdOrden] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [EstadoCocina] nvarchar(max) NOT NULL,
    [PedidoId] int NOT NULL,
    CONSTRAINT [PK_Ordenes] PRIMARY KEY ([IdOrden]),
    CONSTRAINT [FK_Ordenes_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([IdPedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [PedidoMetodoPagos] (
    [IdPedidoMetodoPago] int NOT NULL IDENTITY,
    [PedidoId] int NOT NULL,
    [MetodoPagoId] int NOT NULL,
    [FechaSeleccion] datetime2 NOT NULL,
    CONSTRAINT [PK_PedidoMetodoPagos] PRIMARY KEY ([IdPedidoMetodoPago]),
    CONSTRAINT [FK_PedidoMetodoPagos_MetodosPago_MetodoPagoId] FOREIGN KEY ([MetodoPagoId]) REFERENCES [MetodosPago] ([IdMetodoDePago]) ON DELETE CASCADE,
    CONSTRAINT [FK_PedidoMetodoPagos_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([IdPedido]) ON DELETE CASCADE
);
GO

CREATE TABLE [Pagos] (
    [IdPago] int NOT NULL IDENTITY,
    [FechaPago] datetime2 NOT NULL,
    [BoletaId] int NOT NULL,
    CONSTRAINT [PK_Pagos] PRIMARY KEY ([IdPago]),
    CONSTRAINT [FK_Pagos_Boletas_BoletaId] FOREIGN KEY ([BoletaId]) REFERENCES [Boletas] ([IdBoleta]) ON DELETE CASCADE
);
GO

CREATE TABLE [DetalleMetodosPago] (
    [IdDetallePago] int NOT NULL IDENTITY,
    [MontoPago] decimal(18,2) NOT NULL,
    [PagoId] int NOT NULL,
    [MetodoPagoId] int NOT NULL,
    CONSTRAINT [PK_DetalleMetodosPago] PRIMARY KEY ([IdDetallePago]),
    CONSTRAINT [FK_DetalleMetodosPago_MetodosPago_MetodoPagoId] FOREIGN KEY ([MetodoPagoId]) REFERENCES [MetodosPago] ([IdMetodoDePago]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetalleMetodosPago_Pagos_PagoId] FOREIGN KEY ([PagoId]) REFERENCES [Pagos] ([IdPago]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCategoria', N'Descripcion', N'NombreDeCategoria') AND [object_id] = OBJECT_ID(N'[CategoriaMenus]'))
    SET IDENTITY_INSERT [CategoriaMenus] ON;
INSERT INTO [CategoriaMenus] ([IdCategoria], [Descripcion], [NombreDeCategoria])
VALUES (1, N'Aperitivos y entradas', N'Entradas'),
(2, N'Platos principales', N'Platos Fuertes'),
(3, N'Dulces y postres', N'Postres'),
(4, N'Refrescos y bebidas', N'Bebidas');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdCategoria', N'Descripcion', N'NombreDeCategoria') AND [object_id] = OBJECT_ID(N'[CategoriaMenus]'))
    SET IDENTITY_INSERT [CategoriaMenus] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMesa', N'Capacidad', N'NumMesa') AND [object_id] = OBJECT_ID(N'[Mesas]'))
    SET IDENTITY_INSERT [Mesas] ON;
INSERT INTO [Mesas] ([IdMesa], [Capacidad], [NumMesa])
VALUES (1, 4, 1),
(2, 4, 2),
(3, 6, 3),
(4, 6, 4),
(5, 2, 5),
(6, 8, 6),
(7, 4, 7);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMesa', N'Capacidad', N'NumMesa') AND [object_id] = OBJECT_ID(N'[Mesas]'))
    SET IDENTITY_INSERT [Mesas] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMetodoDePago', N'Nombre') AND [object_id] = OBJECT_ID(N'[MetodosPago]'))
    SET IDENTITY_INSERT [MetodosPago] ON;
INSERT INTO [MetodosPago] ([IdMetodoDePago], [Nombre])
VALUES (1, N'Yape'),
(2, N'Plin');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMetodoDePago', N'Nombre') AND [object_id] = OBJECT_ID(N'[MetodosPago]'))
    SET IDENTITY_INSERT [MetodosPago] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdRol', N'NombreRol') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([IdRol], [NombreRol])
VALUES (1, N'Mesero'),
(2, N'Cocinero'),
(3, N'Administrador'),
(4, N'Cliente');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdRol', N'NombreRol') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMenu', N'CategoriaMenuId', N'Descripcion', N'ImagenUrl', N'Nombre', N'Precio') AND [object_id] = OBJECT_ID(N'[Menus]'))
    SET IDENTITY_INSERT [Menus] ON;
INSERT INTO [Menus] ([IdMenu], [CategoriaMenuId], [Descripcion], [ImagenUrl], [Nombre], [Precio])
VALUES (1, 1, N'Papas amarillas con salsa de ají amarillo, queso fresco y huevo', N'https://i.postimg.cc/R0qnrWjS/papa-huancaina.jpg', N'Papa a la Huancaína', 18.0),
(2, 1, N'Papas con salsa de huacatay, ají mirasol y maní tostado', N'https://i.postimg.cc/T33Kmy2X/ocopa.jpg', N'Ocopa Jesusana', 20.0),
(3, 2, N'Salteado de lomo fino con cebolla, tomate y papas fritas', N'https://i.postimg.cc/pLSLFq1Z/lomo-salt.jpg', N'Lomo Saltado', 35.0),
(4, 2, N'Rocoto relleno de carne molida, queso y hierbas aromáticas', N'https://i.postimg.cc/g0Rm6dhZ/rocoto.webp', N'Rocoto Relleno', 28.0),
(5, 2, N'Cuy frito bajo una piedra con papas y salsa criolla', N'https://i.postimg.cc/vTZJXd4D/cuy.jpg', N'Cuy Chactado', 45.0),
(6, 2, N'Trucha fresca frita con yuca y ensalada andina', N'https://i.postimg.cc/1XXdwcdk/trucha.webp', N'Trucha Frita', 32.0),
(7, 2, N'Caldo nutritivo con gallina criolla, fideos y hierbas', N'https://i.postimg.cc/zBB7cDk5/caldo-gallina.jpg', N'Caldo de Gallina', 25.0),
(8, 3, N'Postre de maíz morado con frutas deshidratadas', N'https://i.postimg.cc/L4rdLGHz/mazamorra.webp', N'Mazamorra Morada', 12.0),
(9, 3, N'Anillos fritos de camote y zapato con miel de chancaca', N'https://i.postimg.cc/MTNL0j8p/picarones.jpg', N'Picarones', 15.0),
(10, 4, N'Refresco tradicional de maíz morado con especias', N'https://i.postimg.cc/nrfkcKJZ/chicha.webp', N'Chicha Morada', 8.0),
(11, 4, N'Bebida caliente de hierbas andinas con linaza', N'https://i.postimg.cc/4N55shFT/emoliente.jpg', N'Emoliente', 6.0),
(12, 4, N'Infusión de hojas de coca para el mal de altura', N'https://i.postimg.cc/MTV74cQ2/mate-coca.webp', N'Mate de Coca', 5.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMenu', N'CategoriaMenuId', N'Descripcion', N'ImagenUrl', N'Nombre', N'Precio') AND [object_id] = OBJECT_ID(N'[Menus]'))
    SET IDENTITY_INSERT [Menus] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'ClienteId', N'Contrasena', N'Correo', N'EmpleadoId', N'Estado', N'FechaCreacion', N'NombreUsuario', N'RolId') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([IdUsuario], [ClienteId], [Contrasena], [Correo], [EmpleadoId], [Estado], [FechaCreacion], [NombreUsuario], [RolId])
VALUES (1, NULL, N'123456789', N'admin@puquio.com', NULL, CAST(1 AS bit), '2025-01-01T00:00:00.0000000', N'admin', 3),
(2, NULL, N'123456789', N'mesero@puquio.com', NULL, CAST(1 AS bit), '2025-01-01T00:00:00.0000000', N'mesero', 1),
(3, NULL, N'123456789', N'cocinero@puquio.com', NULL, CAST(1 AS bit), '2025-01-01T00:00:00.0000000', N'cocinero', 2);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'ClienteId', N'Contrasena', N'Correo', N'EmpleadoId', N'Estado', N'FechaCreacion', N'NombreUsuario', N'RolId') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
    SET IDENTITY_INSERT [Usuarios] OFF;
GO

CREATE UNIQUE INDEX [IX_Boletas_PedidoId] ON [Boletas] ([PedidoId]);
GO

CREATE INDEX [IX_Comentarios_ClienteId] ON [Comentarios] ([ClienteId]);
GO

CREATE INDEX [IX_Comentarios_MenuId] ON [Comentarios] ([MenuId]);
GO

CREATE INDEX [IX_DetalleMenuPedidos_MenuId] ON [DetalleMenuPedidos] ([MenuId]);
GO

CREATE INDEX [IX_DetalleMenuPedidos_PedidoId] ON [DetalleMenuPedidos] ([PedidoId]);
GO

CREATE INDEX [IX_DetalleMetodosPago_MetodoPagoId] ON [DetalleMetodosPago] ([MetodoPagoId]);
GO

CREATE INDEX [IX_DetalleMetodosPago_PagoId] ON [DetalleMetodosPago] ([PagoId]);
GO

CREATE INDEX [IX_Empleados_RolId] ON [Empleados] ([RolId]);
GO

CREATE INDEX [IX_EmpleadoTurnos_EmpleadoId] ON [EmpleadoTurnos] ([EmpleadoId]);
GO

CREATE INDEX [IX_EmpleadoTurnos_TurnoId] ON [EmpleadoTurnos] ([TurnoId]);
GO

CREATE INDEX [IX_Menus_CategoriaMenuId] ON [Menus] ([CategoriaMenuId]);
GO

CREATE INDEX [IX_Ordenes_PedidoId] ON [Ordenes] ([PedidoId]);
GO

CREATE INDEX [IX_Pagos_BoletaId] ON [Pagos] ([BoletaId]);
GO

CREATE INDEX [IX_PedidoMetodoPagos_MetodoPagoId] ON [PedidoMetodoPagos] ([MetodoPagoId]);
GO

CREATE UNIQUE INDEX [IX_PedidoMetodoPagos_PedidoId] ON [PedidoMetodoPagos] ([PedidoId]);
GO

CREATE INDEX [IX_Pedidos_UsuarioId] ON [Pedidos] ([UsuarioId]);
GO

CREATE INDEX [IX_Reservas_ClienteId] ON [Reservas] ([ClienteId]);
GO

CREATE INDEX [IX_Reservas_MesaId] ON [Reservas] ([MesaId]);
GO

CREATE UNIQUE INDEX [IX_Usuarios_ClienteId] ON [Usuarios] ([ClienteId]) WHERE [ClienteId] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Usuarios_EmpleadoId] ON [Usuarios] ([EmpleadoId]) WHERE [EmpleadoId] IS NOT NULL;
GO

CREATE INDEX [IX_Usuarios_RolId] ON [Usuarios] ([RolId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260430175455_BDPCS', N'8.0.0');
GO

COMMIT;
GO

