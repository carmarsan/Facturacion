using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
	public class Detalle
	{
		[Key]
		public int DetalleID { get; set; }

		//[Required, Display(Name = "Num. Fac.")]
		//[Required]
		//public int NumeroFactura { get; set; }

		[Display(Name = "Id Artículo")]
		public int Articulo_Id { get; set; }

		[Display(Name = "Id Art. Alternativo")]
		public string IdArticuloAlternativo { get; set; }

		[Display(Name = "Orden")]
		[Required]
		public int Orden { get; set; }

		[Display(Name = "Cantidad")]
		[Required]
		public decimal Cantidad { get; set; }

		// Como hago este?
		//[Required]
		//public int Iva_Id { get; set; }

		[Display(Name = "Porcentaje IVA")]
		public decimal PorcentajeIva { get; set; }

		[Display(Name = "Texto Iva")]
		public string TextoIva { get; set; }

		[Display(Name = "Total IVA")]
		public decimal TotalIVA { get; set; }

		[Display(Name = "PVP")]
		public decimal PVP { get; set; }

		[NotMapped]
		[Display(Name = "Importe")]
		public decimal Importe { get { return (this.TotalIVA / 100) * this.PVP; } }

		[Display(Name = "Importe Mes")]
		public decimal ImporteMes { get; set; }

		[Display(Name = "Importe Anterior")]
		public decimal ImporteAnterior { get; set; }

		//[Required]
		//public int FacturaRefId { get; set; }
		[Required]
		public int FacturaId { get; set; }

		//public virtual ICollection<Articulo> Articulo { get; set; }

		//[ForeignKey("FacturaRefId")]
		public virtual Factura Factura { get; set; }


	}
}
