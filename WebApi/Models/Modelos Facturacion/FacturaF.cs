using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
	[Table("FacturaF")]
	public class FacturaF
	{

		public FacturaF()
		{

		}

		[Key, ForeignKey("Facturas")]
		public int FacturaFId { get; set; }

		[Required]
		public int NumeroFacturaF { get; set; }

		[Column(TypeName = "float")]
		public float SubTotalF { get; set; }

		[Column(TypeName = "float")]
		public float TotalAOrigenF { get; set; }

		[Column(TypeName = "float")]
		public float CertificacionesAnterioresF { get; set; }

		[Column(TypeName = "float")]
		public float BonificacionF { get; set; }

		[Column(TypeName = "float")]
		public float BaseImponibleF { get; set; }

		[Column(TypeName = "float")]
		public float IVAF { get; set; }

		[Column(TypeName = "float")]
		public float RetencionF { get; set; }

		[Column(TypeName = "float")]
		public float TotalF { get; set; }

		public virtual Factura Facturas { get; set; }
	}
}