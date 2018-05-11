using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Models
{
	[Table("DireccionesEntrega")]
	public class DireccionEntrega
	{

		public DireccionEntrega()
		{
			FechaAlta = DateTime.Now;

			this.Clientes = new List<Cliente>();
		}

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int DireccionEntregaId { get; set; }

		[Required]
		[StringLength(255, MinimumLength = 5)]
		[Column(TypeName = "varchar")]
		public string Direccion { get; set; }

		//[Required]
		[StringLength(127, MinimumLength = 5)]
		[Column(TypeName = "varchar")]
		public string Localidad { get; set; }

		//[Required]
		[StringLength(5, MinimumLength = 5)]
		[Column(TypeName = "varchar")]
		public string CP { get; set; }

		//[Required]
		[StringLength(50, MinimumLength = 5)]
		[Column(TypeName = "varchar")]
		public string Provincia { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(15)]
		public string CIF { get; set; }

		[StringLength(128, MinimumLength = 5)]
		[Column(TypeName = "varchar")]
		public string PersonaContacto { get; set; }

		[Required]
		[DefaultValue(true)]
		public bool Mostrar { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[HiddenInput(DisplayValue = false)]
		public DateTime FechaAlta { get; set; }


		#region VIRTUALES

		//public virtual Cliente Clientes { get; set; }

		public virtual ICollection<Cliente> Clientes { get; set; }

		#endregion



	}
}