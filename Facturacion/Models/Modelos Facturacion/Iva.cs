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
	[Table("Ivas")]
	public class Iva
	{
		public Iva()
		{
			this.FechaAlta = DateTime.Now;

			Articulos = new List<Articulo>();
		}

		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Display(Name = "Iva")]
		public int IvaId { get; set; }

		[Required]
		[Display(Name = "Descripción IVA")]
		[Column(TypeName = "varchar")]
		[StringLength(75, MinimumLength = 3, ErrorMessage = "El valor debe estar entre {2} y {1} caracteres.")]
		public string Texto { get; set; }

		[Required]
		//[Column(TypeName = "float")]
		//[DataType(DataType.Currency)]
		//[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C2}")]
		public decimal Porcentaje { get; set; }

		[Display(Name = "Cuentea Contable")]
		[Column(TypeName = "varchar")]
		[StringLength(20)]
		public string CuentaContable { get; set; }

		[DefaultValue(false)]
		public Boolean Defecto { get; set; }

		[Required]
		[DefaultValue(true)]
		[Display(Name = "IVA por defecto")]
		public bool Mostrar { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		//[HiddenInput(DisplayValue = false)]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[DefaultValue("getutcdate()")]
		public DateTime FechaAlta { get; set; }


		#region VIRTUALES

		public virtual ICollection<Articulo> Articulos { get; set; }

		#endregion


	}
}
