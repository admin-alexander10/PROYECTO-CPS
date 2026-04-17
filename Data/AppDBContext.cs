using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Models;

namespace Proyecto_CPS.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<EmpleadoTurno> EmpleadoTurnos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<CategoriaMenu> CategoriaMenus { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetalleMenuPedido> DetalleMenuPedidos { get; set; }
        public DbSet<Boleta> Boletas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<DetalleMetodoPago> DetalleMetodosPago { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<PedidoMetodoPago> PedidoMetodoPagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ROL (1:N) -> Empleado
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Rol)
                .WithMany(r => r.Empleados)
                .HasForeignKey(e => e.RolId);

            // USUARIO - Empleado (1:1 optional because of CHECK rule)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Empleado)
                .WithOne(e => e.Usuarios)
                .HasForeignKey<Usuario>(u => u.EmpleadoId);

            // USUARIO - Cliente (1:1 optional because of CHECK rule)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cliente)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Usuario>(u => u.ClienteId);

            // TURNOS - EmpleadoTurno (1:N) via EmpleadoTurno -> Turno
            modelBuilder.Entity<EmpleadoTurno>()
                .HasOne(et => et.Turnos)
                .WithMany(t => t.EmpleadoTurnos)
                .HasForeignKey(et => et.TurnoId);

            // EMPLEADO - EmpleadoTurno (1:N)
            modelBuilder.Entity<EmpleadoTurno>()
                .HasOne(et => et.Empleados)
                .WithMany(e => e.EmpleadoTurnos)
                .HasForeignKey(et => et.EmpleadoId);

            // MESA - Reserva (1:N)
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Mesas)
                .WithMany(m => m.Reservas)
                .HasForeignKey(r => r.MesaId);

            // CLIENTE - Reserva (1:N)
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Clientes)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId);

            // CATEGORIA_MENU - MENU (1:N)
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.CategoriaMenus)
                .WithMany(c => c.Menus)
                .HasForeignKey(m => m.CategoriaMenuId);

            // MENU - DetalleMenuPedido (1:N)
            modelBuilder.Entity<DetalleMenuPedido>()
                .HasOne(d => d.Menu)
                .WithMany(m => m.DetalleMenuPedidos)
                .HasForeignKey(d => d.MenuId);

            // PEDIDO - DetalleMenuPedido (1:N)
            modelBuilder.Entity<DetalleMenuPedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.DetalleMenuPedidos)
                .HasForeignKey(d => d.PedidoId);

            // Usuario - Pedido (1:N)
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId);

            // PEDIDO - BOLETA (1:1)
            modelBuilder.Entity<Boleta>()
                .HasOne(b => b.Pedidos)
                .WithOne(p => p.Boletas)
                .HasForeignKey<Boleta>(b => b.PedidoId);

            // BOLETA - PAGO (1:N)
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Boleta)
                .WithMany(b => b.Pagos)
                .HasForeignKey(p => p.BoletaId);

            // PAGO - DETALLE_METODO_PAGO (1:N)
            modelBuilder.Entity<DetalleMetodoPago>()
                .HasOne(d => d.Pagos)
                .WithMany(p => p.DetallesMetodoPagos)
                .HasForeignKey(d => d.PagoId);

            // METODO_PAGO - DETALLE_METODO_PAGO (1:N)
            modelBuilder.Entity<DetalleMetodoPago>()
                .HasOne(d => d.MetodoPagos)
                .WithMany(m => m.DetalleMetodoPagos)
                .HasForeignKey(d => d.MetodoPagoId);

            // MENU - COMENTARIO (1:N)
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Menus)
                .WithMany(m => m.Comentarios)
                .HasForeignKey(c => c.MenuId);

            // CLIENTE - COMENTARIO (1:N)
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Clientes)
                .WithMany(cl => cl.Comentarios)
                .HasForeignKey(c => c.ClienteId);

            modelBuilder.Entity<Usuario>()
                .HasOne(c => c.Rol)
                .WithMany(cl => cl.Usuarios)
                .HasForeignKey(c => c.RolId);
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Pedido)
                .WithMany(p => p.Ordenes)
                .HasForeignKey(o => o.PedidoId);
            modelBuilder.Entity<PedidoMetodoPago>()
                .HasOne(pmp => pmp.Pedido)
                .WithOne(p => p.PedidoMetodoPago)
                .HasForeignKey<PedidoMetodoPago>(pmp => pmp.PedidoId);

            modelBuilder.Entity<PedidoMetodoPago>()
                .HasOne(pmp => pmp.MetodoPago)
                .WithMany()
                .HasForeignKey(pmp => pmp.MetodoPagoId);

            // SEED: (opcional) Roles básicos - si quieres que los seed permanezcan, déjalos aquí
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, NombreRol = "Mesero" },
                new Rol { IdRol = 2, NombreRol = "Cocinero" },
                new Rol { IdRol = 3, NombreRol = "Administrador" },
                new Rol { IdRol = 4, NombreRol = "Cliente" }
            );
            modelBuilder.Entity<CategoriaMenu>().HasData(
                new CategoriaMenu { IdCategoria = 1, NombreDeCategoria = "Entradas", Descripcion = "Aperitivos y entradas" },
                new CategoriaMenu { IdCategoria = 2, NombreDeCategoria = "Platos Fuertes", Descripcion = "Platos principales" },
                new CategoriaMenu { IdCategoria = 3, NombreDeCategoria = "Postres", Descripcion = "Dulces y postres" },
                new CategoriaMenu { IdCategoria = 4, NombreDeCategoria = "Bebidas", Descripcion = "Refrescos y bebidas" }
            );
            modelBuilder.Entity<Menu>().HasData(
            // Entradas
            new Menu
            {
                IdMenu = 1,
                Nombre = "Papa a la Huancaína",
                Precio = 18.00m,
                Descripcion = "Papas amarillas con salsa de ají amarillo, queso fresco y huevo",
                ImagenUrl = "https://i.postimg.cc/R0qnrWjS/papa-huancaina.jpg",
                CategoriaMenuId = 1
            },
            new Menu
            {
                IdMenu = 2,
                Nombre = "Ocopa Jesusana",
                Precio = 20.00m,
                Descripcion = "Papas con salsa de huacatay, ají mirasol y maní tostado",
                ImagenUrl = "https://i.postimg.cc/T33Kmy2X/ocopa.jpg",
                CategoriaMenuId = 1
            },

            // Platos Fuertes
            new Menu
            {
                IdMenu = 3,
                Nombre = "Lomo Saltado",
                Precio = 35.00m,
                Descripcion = "Salteado de lomo fino con cebolla, tomate y papas fritas",
                ImagenUrl = "https://i.postimg.cc/pLSLFq1Z/lomo-salt.jpg",
                CategoriaMenuId = 2
            },
            new Menu
            {
                IdMenu = 4,
                Nombre = "Rocoto Relleno",
                Precio = 28.00m,
                Descripcion = "Rocoto relleno de carne molida, queso y hierbas aromáticas",
                ImagenUrl = "https://i.postimg.cc/g0Rm6dhZ/rocoto.webp",
                CategoriaMenuId = 2
            },
            new Menu
            {
                IdMenu = 5,
                Nombre = "Cuy Chactado",
                Precio = 45.00m,
                Descripcion = "Cuy frito bajo una piedra con papas y salsa criolla",
                ImagenUrl = "https://i.postimg.cc/vTZJXd4D/cuy.jpg",
                CategoriaMenuId = 2
            },
            new Menu
            {
                IdMenu = 6,
                Nombre = "Trucha Frita",
                Precio = 32.00m,
                Descripcion = "Trucha fresca frita con yuca y ensalada andina",
                ImagenUrl = "https://i.postimg.cc/1XXdwcdk/trucha.webp",
                CategoriaMenuId = 2
            },
            new Menu
            {
                IdMenu = 7,
                Nombre = "Caldo de Gallina",
                Precio = 25.00m,
                Descripcion = "Caldo nutritivo con gallina criolla, fideos y hierbas",
                ImagenUrl = "https://i.postimg.cc/zBB7cDk5/caldo-gallina.jpg",
                CategoriaMenuId = 2
            },

            // Postres
            new Menu
            {
                IdMenu = 8,
                Nombre = "Mazamorra Morada",
                Precio = 12.00m,
                Descripcion = "Postre de maíz morado con frutas deshidratadas",
                ImagenUrl = "https://i.postimg.cc/L4rdLGHz/mazamorra.webp",
                CategoriaMenuId = 3
            },
            new Menu
            {
                IdMenu = 9,
                Nombre = "Picarones",
                Precio = 15.00m,
                Descripcion = "Anillos fritos de camote y zapato con miel de chancaca",
                ImagenUrl = "https://i.postimg.cc/MTNL0j8p/picarones.jpg",
                CategoriaMenuId = 3
            },

            // Bebidas
            new Menu
            {
                IdMenu = 10,
                Nombre = "Chicha Morada",
                Precio = 8.00m,
                Descripcion = "Refresco tradicional de maíz morado con especias",
                ImagenUrl = "https://i.postimg.cc/nrfkcKJZ/chicha.webp",
                CategoriaMenuId = 4
            },
            new Menu
            {
                IdMenu = 11,
                Nombre = "Emoliente",
                Precio = 6.00m,
                Descripcion = "Bebida caliente de hierbas andinas con linaza",
                ImagenUrl = "https://i.postimg.cc/4N55shFT/emoliente.jpg",
                CategoriaMenuId = 4
            },
            new Menu
            {
                IdMenu = 12,
                Nombre = "Mate de Coca",
                Precio = 5.00m,
                Descripcion = "Infusión de hojas de coca para el mal de altura",
                ImagenUrl = "https://i.postimg.cc/MTV74cQ2/mate-coca.webp",
                CategoriaMenuId = 4
            }
            );
            modelBuilder.Entity<MetodoPago>().HasData(
                new MetodoPago { IdMetodoDePago = 1, Nombre = "Yape" },
                new MetodoPago { IdMetodoDePago = 2, Nombre = "Plin" }
            );


            modelBuilder.Entity<Mesa>().HasData(
                new Mesa { IdMesa = 1, NumMesa = 1, Capacidad = 4 },
                new Mesa { IdMesa = 2, NumMesa = 2, Capacidad = 4 },
                new Mesa { IdMesa = 3, NumMesa = 3, Capacidad = 6 },
                new Mesa { IdMesa = 4, NumMesa = 4, Capacidad = 6 },
                new Mesa { IdMesa = 5, NumMesa = 5, Capacidad = 2 },
                new Mesa { IdMesa = 6, NumMesa = 6, Capacidad = 8 },
                new Mesa { IdMesa = 7, NumMesa = 7, Capacidad = 4 }
            );
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                IdUsuario = 1,
                NombreUsuario = "admin",
                Correo = "admin@ranchoazul.com",
                Contrasena = "123456789",
                FechaCreacion = new DateTime(2025, 01, 01),
                Estado = true,
                RolId = 3,
                EmpleadoId = null,
                ClienteId = null
            }
            );
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                IdUsuario = 2,
                NombreUsuario = "mesero",
                Correo = "mesero@ranchoazul.com",
                Contrasena = "123456789",
                FechaCreacion = new DateTime(2025, 01, 01),
                Estado = true,
                RolId = 1,
                EmpleadoId = null,
                ClienteId = null
            }
            );
            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                IdUsuario = 3,
                NombreUsuario = "cocinero",
                Correo = "cocinero@ranchoazul.com",
                Contrasena = "123456789",
                FechaCreacion = new DateTime(2025, 01, 01),
                Estado = true,
                RolId = 2,
                EmpleadoId = null,
                ClienteId = null
            }
            );

        }
    }
}