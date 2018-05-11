using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Facturacion.Context
{
	public class FacturacionContext : DbContext
	{

		public FacturacionContext()
			: base("name=FacturacionContext")
		{ }

		public DbSet<Actividad> Actividades { get; set; }

		public DbSet<Articulo> Articulos { get; set; }

		public DbSet<Banco> Bancos { get; set; }

		public DbSet<Cliente> Clientes { get; set; }

		public DbSet<Configuracion> Configuraciones { get; set; }

		public DbSet<Delegacion> Delegaciones { get; set; }

		public DbSet<Detalle> Detalles { get; set; }

		public DbSet<DireccionEntrega> DireccionesEntrega { get; set; }

		//public DbSet<Familia> Familias { get; set; }

		public DbSet<FormaPago> FormasPago { get; set; }

		public DbSet<Iva> Ivas { get; set; }

		public DbSet<Obra> Obras { get; set; }

		public DbSet<SubActividad> SubActividades { get; set; }

		public DbSet<Factura> Facturas { get; set; }

		public DbSet<FacturaF> FacturasF { get; set; }

		public DbSet<Provincia> Provincias { get; set; }

		public DbSet<Poblacion> Poblaciones { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// Relación muchos a muchos entre Clientes y Direcciones de entrega
			//modelBuilder.Entity<Cliente>()
			//	.HasMany<DireccionEntrega>(s => s.DireccionesEntrega)
			//	.WithMany(c => c.Clientes)
			//	.Map(cs =>
			//	{
			//		cs.MapLeftKey("ClienteId");
			//		cs.MapRightKey("DireccionEntregaId");
			//		cs.ToTable("ClienteDireccionEntrega");
			//	});


			modelBuilder.Entity<Articulo>().Property(o => o.PrecioCompra).HasPrecision(9, 2);
			modelBuilder.Entity<Articulo>().Property(o => o.Precio).HasPrecision(9, 2);
			modelBuilder.Entity<Articulo>().Property(o => o.PrecioVenta).HasPrecision(9, 2);


			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();




		}


	}
}