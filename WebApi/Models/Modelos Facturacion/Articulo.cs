using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Facturacion.Models
{
	[Table("Articulos")]
	public class Articulo
	{


		[Key]
		//[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ArticuloId { get; set; }

		[Required]
		[Display(Name = "Descripción")]
		[Column(TypeName = "varchar")]
		[StringLength(150, MinimumLength = 3, ErrorMessage = "El valor debe estar entre {2} y {1} caracteres.")]
		public string Descripcion { get; set; }

		[Required]
		[Column(TypeName = "decimal")]
		[DataType(DataType.Currency)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
		public decimal Precio { get; set; }

		[Required]
		[Display(Name = "Iva")]
		public int IvaId { get; set; }

		[Display(Name = "Precio Compra")]
		[DataType(DataType.Currency)]
		[Column(TypeName = "decimal")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
		public decimal PrecioCompra { get; set; }

		[Display(Name = "Precio Venta")]
		[DataType(DataType.Currency)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
		[Column(TypeName = "decimal")]
		//[Required]
		public decimal PrecioVenta { get; set; }

		[HiddenInput(DisplayValue = false)]
		[Display(Name = "Fecha Alta")]
		[DataType(DataType.DateTime)]
		//[Column(TypeName = "date")]
		//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
		//[Required]
		public DateTime FechaAlta { get; set; }

		[Required]
		[DefaultValue(true)]
		[Display(Name = "Mostrar")]
		public bool Ver { get; set; }

		[Display(Name = "Código Contable")]
		[StringLength(12, MinimumLength = 3, ErrorMessage = "El valor debe estar entre {2} y {1} caracteres.")]
		public string CodigoContable { get; set; }


		public Articulo()
		{
			this.FechaAlta = DateTime.Now;
		}


		#region VIRTUALES

		public virtual Iva Iva { get; set; }

		#endregion

	}
}
