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
	[Table("Bancos")]
	public class Banco
	{
		public Banco()
		{
			FechaAlta = DateTime.Now;
		}

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int BancoId { get; set; }

		[Required]
		[Display(Name = "Código")]
		[Column(TypeName = "varchar")]
		[StringLength(8)]
		public string Codigo { get; set; }

		[Required]
		[Column(TypeName = "varchar")]
		[StringLength(50)]
		public string Nombre { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(200)]
		[Display(Name = "Dirección")]
		public string Direccion { get; set; }

		[Required]
		[DefaultValue(true)]
		public bool Mostrar { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[HiddenInput(DisplayValue = false)]
		public DateTime FechaAlta { get; set; }
	}
}