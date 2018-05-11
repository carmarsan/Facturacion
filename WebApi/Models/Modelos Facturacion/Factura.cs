using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturacion.Models
{
	[Table("Facturas")]
	public class Factura
	{

		public Factura()
		{
			this.Tipo = "F";
			this.FechaAlta = DateTime.Now;
		}


		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		[Key]
		public int FacturaId { get; set; }

		[Required]
		public int ObraId { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime FechaAlta { get; set; }

		[Required]
		public int NumeroFactura { get; set; }

		[Required]
		public int Orden { get; set; }


		[Required]
		[DataType(DataType.DateTime)]
		public DateTime FechaFactura { get; set; }

		[Required]
		//[Column(TypeName = "char")]
		[StringLength(1)]
		public string Tipo { get; set; }


		//[DefaultValue('0')]
		public int IdSobreFactura { get; set; }

		public int SobreFactura { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(50, MinimumLength = 45)]
		public string RotuloGerencia { get; set; }


		[Column(TypeName = "float")]
		public float PorcentajeBonificacion { get; set; }

		[Column(TypeName = "float")]
		public float PorcentajeRetencion { get; set; }

		[Column(TypeName = "float")]
		public float Subtotal { get; set; }

		[Column(TypeName = "float")]
		public float Bonificacion { get; set; }

		[Column(TypeName = "float")]
		public float BaseImponible { get; set; }

		//[NotMapped]
		//public float CtdadIVA { get { return this.BaseImponible * (this.Iva.Porcentaje / 100); } set { value = CtdadIVA; } }

		[Column(TypeName = "float")]
		public float Retencion { get; set; }

		[Column(TypeName = "float")]
		public float Total { get; set; }

		[Column(TypeName = "float")]
		public float TotalAOrigen { get; set; }

		[Column(TypeName = "float")]
		public float CertificacionesAnteriores { get; set; }

		[DefaultValue(false)]
		public bool Facturado { get; set; }

		public char TipoFacturacion { get; set; }

		[DefaultValue(false)]
		public bool Cerrada { get; set; }

		[DefaultValue(false)]
		public bool FacturaEspecial { get; set; }

		[DefaultValue(true)]
		public bool FacturaUnica { get; set; }

		[DefaultValue(false)]
		public bool Contabilizado { get; set; }

		[DefaultValue(false)]
		public bool TieneTotalPropio { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(50, MinimumLength = 50)]
		public string TextoFactura { get; set; }




		/*
		 * Actividad y Subactividad van con la obra y la forma de pago con el cliente

		 * */
		#region VIRTUALES

		public virtual FacturaF FacturaF { get; set; }

		public virtual ICollection<Detalle> Detalles { get; set; }

		public virtual Obra Obra { get; set; }

		#endregion


	}
}
