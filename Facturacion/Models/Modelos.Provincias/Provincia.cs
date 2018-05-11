using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
	public class Provincia
	{
		[Key]
		public int ProvinciaId { get; set; }

		[StringLength(50, ErrorMessage = "Demasiados caracteres")]
		public string Nombre { get; set; }

		[StringLength(50)]
		public string Provinciaseo { get; set; }

		[StringLength(3)]
		public string Iniciales { get; set; }


		#region VIRTUALES

		public virtual IEnumerable<Poblacion> Poblaciones { get; set; }

		public virtual IEnumerable<Cliente> Clientes { get; set; }

		#endregion
	}
}