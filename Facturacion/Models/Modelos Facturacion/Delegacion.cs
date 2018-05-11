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
	[Table("Delegaciones")]
	public class Delegacion
	{

		public Delegacion()
		{
			this.FechaAlta = DateTime.Now;

			Obras = new List<Obra>();
		}

		#region Propiedades

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int DelegacionId { get; set; }

		[Required]
		[Display(Name = "Nombre Delegación")]
		[Column(TypeName = "varchar")]
		[StringLength(150)]
		public string NombreDelegacion { get; set; }

		[Required]
		[Display(Name = "Abreviatura")]
		[Column(TypeName = "varchar")]
		[StringLength(5)]
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
