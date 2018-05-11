using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Web.Mvc;

namespace Facturacion.Models
{
	[Table("Actividades")]
	public class Actividad
	{
		public Actividad()
		{
			FechaAlta = DateTime.Now;

			Obras = new List<Obra>();
		}


		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int ActividadID { get; set; }

		[Required]
		[Column(TypeName = "varchar")]
		[StringLength(75, MinimumLength = 3, ErrorMessage = "El valor debe estar entre {2} y {1} caracteres.")]
		public string Nombre { get; set; }

		[Required]
		[StringLength(3, MinimumLength = 1, ErrorMessage = "Número máximo de caracteres: 3.")]
		[Column(TypeName = "varchar")]
		public string Abreviatura { get; set; }

		[Required]
		[DefaultValue(true)]
		public bool Mostrar { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[HiddenInput(DisplayValue = false)]
		public DateTime FechaAlta { get; set; }


		#region VIRTUALES

		public virtual ICollection<Obra> Obras { get; set; }

		#endregion




		//public virtual ICollection<Factura> Facturas { get; set; }
	}
}