using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Proyecto_CPS.Migrations
{
    /// <inheritdoc />
    public partial class BD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaMenus",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreDeCategoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaMenus", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SegNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidoP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidoM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Mesas",
                columns: table => new
                {
                    IdMesa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumMesa = table.Column<int>(type: "int", nullable: false),
                    Capacidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesas", x => x.IdMesa);
                });

            migrationBuilder.CreateTable(
                name: "MetodosPago",
                columns: table => new
                {
                    IdMetodoDePago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetodosPago", x => x.IdMetodoDePago);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    IdMenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoriaMenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.IdMenu);
                    table.ForeignKey(
                        name: "FK_Menus_CategoriaMenus_CategoriaMenuId",
                        column: x => x.CategoriaMenuId,
                        principalTable: "CategoriaMenus",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    MesaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reservas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Mesas_MesaId",
                        column: x => x.MesaId,
                        principalTable: "Mesas",
                        principalColumn: "IdMesa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    IdEmpleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SegNombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidoP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidoM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaContrato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.IdEmpleado);
                    table.ForeignKey(
                        name: "FK_Empleados_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    IdComentario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaComentario = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentarios_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "IdMenu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoTurnos",
                columns: table => new
                {
                    IdEmpleadoTurno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaTurno = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoTurnos", x => x.IdEmpleadoTurno);
                    table.ForeignKey(
                        name: "FK_EmpleadoTurnos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoTurnos_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "IdTurno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK_Usuarios_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "IdEmpleado");
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Boletas",
                columns: table => new
                {
                    IdBoleta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumBoleta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaEmicion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boletas", x => x.IdBoleta);
                    table.ForeignKey(
                        name: "FK_Boletas_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleMenuPedidos",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleMenuPedidos", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_DetalleMenuPedidos_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "IdMenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleMenuPedidos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    IdOrden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoCocina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.IdOrden);
                    table.ForeignKey(
                        name: "FK_Ordenes_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoMetodoPagos",
                columns: table => new
                {
                    IdPedidoMetodoPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    MetodoPagoId = table.Column<int>(type: "int", nullable: false),
                    FechaSeleccion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoMetodoPagos", x => x.IdPedidoMetodoPago);
                    table.ForeignKey(
                        name: "FK_PedidoMetodoPagos_MetodosPago_MetodoPagoId",
                        column: x => x.MetodoPagoId,
                        principalTable: "MetodosPago",
                        principalColumn: "IdMetodoDePago",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoMetodoPagos_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoletaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Boletas_BoletaId",
                        column: x => x.BoletaId,
                        principalTable: "Boletas",
                        principalColumn: "IdBoleta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleMetodosPago",
                columns: table => new
                {
                    IdDetallePago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoPago = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PagoId = table.Column<int>(type: "int", nullable: false),
                    MetodoPagoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleMetodosPago", x => x.IdDetallePago);
                    table.ForeignKey(
                        name: "FK_DetalleMetodosPago_MetodosPago_MetodoPagoId",
                        column: x => x.MetodoPagoId,
                        principalTable: "MetodosPago",
                        principalColumn: "IdMetodoDePago",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleMetodosPago_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "IdPago",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoriaMenus",
                columns: new[] { "IdCategoria", "Descripcion", "NombreDeCategoria" },
                values: new object[,]
                {
                    { 1, "Aperitivos y entradas", "Entradas" },
                    { 2, "Platos principales", "Platos Fuertes" },
                    { 3, "Dulces y postres", "Postres" },
                    { 4, "Refrescos y bebidas", "Bebidas" }
                });

            migrationBuilder.InsertData(
                table: "Mesas",
                columns: new[] { "IdMesa", "Capacidad", "NumMesa" },
                values: new object[,]
                {
                    { 1, 4, 1 },
                    { 2, 4, 2 },
                    { 3, 6, 3 },
                    { 4, 6, 4 },
                    { 5, 2, 5 },
                    { 6, 8, 6 },
                    { 7, 4, 7 }
                });

            migrationBuilder.InsertData(
                table: "MetodosPago",
                columns: new[] { "IdMetodoDePago", "Nombre" },
                values: new object[,]
                {
                    { 1, "Yape" },
                    { 2, "Plin" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "IdRol", "NombreRol" },
                values: new object[,]
                {
                    { 1, "Mesero" },
                    { 2, "Cocinero" },
                    { 3, "Administrador" },
                    { 4, "Cliente" }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "IdMenu", "CategoriaMenuId", "Descripcion", "ImagenUrl", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, 1, "Papas amarillas con salsa de ají amarillo, queso fresco y huevo", "https://i.postimg.cc/R0qnrWjS/papa-huancaina.jpg", "Papa a la Huancaína", 18.00m },
                    { 2, 1, "Papas con salsa de huacatay, ají mirasol y maní tostado", "https://i.postimg.cc/T33Kmy2X/ocopa.jpg", "Ocopa Jesusana", 20.00m },
                    { 3, 2, "Salteado de lomo fino con cebolla, tomate y papas fritas", "https://i.postimg.cc/pLSLFq1Z/lomo-salt.jpg", "Lomo Saltado", 35.00m },
                    { 4, 2, "Rocoto relleno de carne molida, queso y hierbas aromáticas", "https://i.postimg.cc/g0Rm6dhZ/rocoto.webp", "Rocoto Relleno", 28.00m },
                    { 5, 2, "Cuy frito bajo una piedra con papas y salsa criolla", "https://i.postimg.cc/vTZJXd4D/cuy.jpg", "Cuy Chactado", 45.00m },
                    { 6, 2, "Trucha fresca frita con yuca y ensalada andina", "https://i.postimg.cc/1XXdwcdk/trucha.webp", "Trucha Frita", 32.00m },
                    { 7, 2, "Caldo nutritivo con gallina criolla, fideos y hierbas", "https://i.postimg.cc/zBB7cDk5/caldo-gallina.jpg", "Caldo de Gallina", 25.00m },
                    { 8, 3, "Postre de maíz morado con frutas deshidratadas", "https://i.postimg.cc/L4rdLGHz/mazamorra.webp", "Mazamorra Morada", 12.00m },
                    { 9, 3, "Anillos fritos de camote y zapato con miel de chancaca", "https://i.postimg.cc/MTNL0j8p/picarones.jpg", "Picarones", 15.00m },
                    { 10, 4, "Refresco tradicional de maíz morado con especias", "https://i.postimg.cc/nrfkcKJZ/chicha.webp", "Chicha Morada", 8.00m },
                    { 11, 4, "Bebida caliente de hierbas andinas con linaza", "https://i.postimg.cc/4N55shFT/emoliente.jpg", "Emoliente", 6.00m },
                    { 12, 4, "Infusión de hojas de coca para el mal de altura", "https://i.postimg.cc/MTV74cQ2/mate-coca.webp", "Mate de Coca", 5.00m }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "ClienteId", "Contrasena", "Correo", "EmpleadoId", "Estado", "FechaCreacion", "NombreUsuario", "RolId" },
                values: new object[,]
                {
                    { 1, null, "123456789", "admin@ranchoazul.com", null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin", 3 },
                    { 2, null, "123456789", "mesero@ranchoazul.com", null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mesero", 1 },
                    { 3, null, "123456789", "cocinero@ranchoazul.com", null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cocinero", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boletas_PedidoId",
                table: "Boletas",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ClienteId",
                table: "Comentarios",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_MenuId",
                table: "Comentarios",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMenuPedidos_MenuId",
                table: "DetalleMenuPedidos",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMenuPedidos_PedidoId",
                table: "DetalleMenuPedidos",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMetodosPago_MetodoPagoId",
                table: "DetalleMetodosPago",
                column: "MetodoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMetodosPago_PagoId",
                table: "DetalleMetodosPago",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_RolId",
                table: "Empleados",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoTurnos_EmpleadoId",
                table: "EmpleadoTurnos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoTurnos_TurnoId",
                table: "EmpleadoTurnos",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CategoriaMenuId",
                table: "Menus",
                column: "CategoriaMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_PedidoId",
                table: "Ordenes",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_BoletaId",
                table: "Pagos",
                column: "BoletaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoMetodoPagos_MetodoPagoId",
                table: "PedidoMetodoPagos",
                column: "MetodoPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoMetodoPagos_PedidoId",
                table: "PedidoMetodoPagos",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClienteId",
                table: "Reservas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_MesaId",
                table: "Reservas",
                column: "MesaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ClienteId",
                table: "Usuarios",
                column: "ClienteId",
                unique: true,
                filter: "[ClienteId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpleadoId",
                table: "Usuarios",
                column: "EmpleadoId",
                unique: true,
                filter: "[EmpleadoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "DetalleMenuPedidos");

            migrationBuilder.DropTable(
                name: "DetalleMetodosPago");

            migrationBuilder.DropTable(
                name: "EmpleadoTurnos");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "PedidoMetodoPagos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "MetodosPago");

            migrationBuilder.DropTable(
                name: "Mesas");

            migrationBuilder.DropTable(
                name: "CategoriaMenus");

            migrationBuilder.DropTable(
                name: "Boletas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
