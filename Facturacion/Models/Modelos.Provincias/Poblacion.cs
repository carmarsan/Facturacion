using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
	[Table("Poblaciones")]
	public class Poblacion
	{

		[Key]
		public int PoblacionId { get; set; }

		public int ProvinciaId { get; set; }

		[StringLength(150)]
		public string NombrePoblacion { get; set; }

		[StringLength(150)]
		public string PoblacionSeo { get; set; }

		[Range(0, 99999)]
		public int CP { get; set; }

		public decimal Latitud { get; set; }

		public decimal Longitud { get; set; }


		public virtual Provincia Provincia { get; set; }

		public virtual IEnumerable<Cliente> Clientes { get; set; }

	}
}