using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Facturacion.Models
{
	[DataContract(IsReference = true)]
	public class Detalle
	{
		[Key]
		public int DetalleId { get; set; }

		[Required]
		public int FacturaId { get; set; }

		[Required, Column(TypeName = "varchar"), StringLength(255)]
		[Display(Name = "Id Artículo")]
		public string Articulo { get; set; }

		[StringLength(50), Column(TypeName = "varchar"), Display(Name = "Id Art. Alternativo")]
		public string ArticuloAlternativo { get; set; }

		[Required]
		[Display(Name = "Orden")]
		public int Orden { get; set; }

		[Required, Display(Name = "Cantidad"), Column(TypeName = "decimal")]
		public decimal Cantidad { get; set; }

		[Required, Display(Name = "Precio Unitario"), Column(TypeName = "decimal")]
		public decimal PrecioUnitario { get; set; }
		// Como hago este?
		//[Required]
		//public int Iva_Id { get; set; }

		[Display(Name = "Porcentaje IVA"), Required]
		[Column(TypeName = "decimal")]
		public decimal PorcentajeIva { get; set; }

		[Display(Name = "Texto Iva")]
		[Column(TypeName = "varchar")]
		[StringLength(15)]
		public string TextoIva { get; set; }

		[NotMapped]
		[Display(Name = "Total IVA")]
		public decimal TotalIVA { get { return (this.PorcentajeIva / 100) * (this.PrecioUnitario * this.Cantidad); } }


		[NotMapped]
		[Display(Name = "Importe")]
		public decimal Importe { get { return this.Cantidad * this.PrecioUnitario; } }

		//public virtual ICollection<Articulo> Articulo { get; set; }

		//
		//public virtual FacturaF FacturaF { get; set; }

		[ForeignKey("FacturaId")]
		public virtual Factura Factura { get; set; }


	}
}
