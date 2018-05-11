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
	[Table("Subactividades")]
	public class SubActividad
	{

		public SubActividad()
		{
			this.FechaAlta = DateTime.Now;

			Obras = new List<Obra>();
		}

		#region Propiedades

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int SubActividadId { get; set; }

		[Required]
		[Column(TypeName = "varchar")]
		[StringLength(50, MinimumLength = 5, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
		public string Subactividad { get; set; }

		[Required]
		[Column(TypeName = "varchar")]
		[StringLength(4, MinimumLength = 3, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
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



		#endregion
	}
}
