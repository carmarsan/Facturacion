using Facturacion.Models.Modelos_Facturacion;
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
	[Table("FormasPago")]
	public class FormaPago
	{
		public FormaPago()
		{
			this.FechaAlta = DateTime.Now;

			Clientes = new List<Cliente>();
		}

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		[Display(Name = "Forma de pago", Description = "Forma de pago")]
		public int FormaPagoId { get; set; }

		[Required(ErrorMessage = "Es obligatorio poner una descripción.")]
		[StringLength(50)]
		[Display(Name = "Descripción")]
		[Column(TypeName = "varchar")]
		public string Descripcion { get; set; }

		[StringLength(254)]
		[Column(TypeName = "varchar")]
		[Display(Name = "Texto Largo")]
		public string TextoLargo { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto1 { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto2 { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto3 { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto4 { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto5 { get; set; }

		[Display(Name = "Vcto 1")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
		public int? Vcto6 { get; set; }

		[Required]
		[DefaultValue(true)]
		public bool Mostrar { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		[HiddenInput(DisplayValue = false)]
		public DateTime FechaAlta { get; set; }


		// Una forma de pago la pueden tener muchos clientes
		public virtual ICollection<Cliente> Clientes { get; set; }

	}
}