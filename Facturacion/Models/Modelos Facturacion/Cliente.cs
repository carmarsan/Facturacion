using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Facturacion.Models
{
	public class Cliente
	{

		public Cliente()
		{
			this.FechaAlta = DateTime.Now;

			this.DireccionesEntrega = new List<DireccionEntrega>();
		}

		[Key]
		//[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
		public int ClienteId { get; set; }

		//[Required]
		//[Display(Name = "Cód. Cliente")]
		//public int CodigoCliente { get; set; }

		[Required]
		[Display(Name = "Nombre")]
		[Column(TypeName = "varchar")]
		[StringLength(100)]
		public string NombreCliente { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(75)]
		public string Titular { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(125)]
		public string Domicilio { get; set; }

		//[Column(TypeName = "varchar")]
		//[StringLength(150)]
		//public string Localidad { get; set; }

		public int PoblacionId { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(5)]
		public string CP { get; set; }

		//[Column(TypeName = "varchar")]
		//[StringLength(50)]
		//public string Provincia { get; set; }

		public int ProvinciaId { get; set; }

		[Required(ErrorMessage = "Este campo es obligatorio")]
		[Column(TypeName = "varchar")]
		[StringLength(15)]
		public string CIF { get; set; }

		// [Required]
		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Telefono 1")]
		public string Telefono1 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Telefono 2")]
		public string Telefono2 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Movil 1")]
		public string Movil1 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Movil 2")]
		public string Movil2 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Fax 1")]
		public string Fax1 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(10)]
		[Display(Name = "Fax 2")]
		public string Fax2 { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(50)]
		[DataType(DataType.EmailAddress, ErrorMessage = "Por favor, introduzca una dirección de email correcta")]
		public string Email { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(100)]
		public string Web { get; set; }

		[Column(TypeName = "varchar")]
		public string Observaciones { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(50)]
		[Display(Name = "Persona Contacto")]
		public string PersonaContacto { get; set; }


		//[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd-MM-yyyy}")]
		[Required]
		[DataType(DataType.DateTime)]
		[HiddenInput(DisplayValue = false)]
		public DateTime FechaAlta { get; set; }

		[Display(Name = "Mostrar")]
		[Required]
		[DefaultValue(true)]
		public bool Mostrar { get; set; }

		[DefaultValue(false)]
		public bool Sello { get; set; }

		[Column(TypeName = "varchar")]
		[StringLength(50)]
		[Display(Name = "Codigo Contable")]
		public string CodigoContable { get; set; }

		[Display(Name = "Forma de pago", Description = "Forma de pago")]
		public int FormaPagoId { get; set; }

		//public int DireccionEntregaRefId { get; set; }

		#region VIRTUALES

		// Un cliente puede tener UNA formas de pago
		public virtual FormaPago FormaPago { get; set; }

		// Un cliente puede tener muchas direcciones de entrega
		public virtual ICollection<DireccionEntrega> DireccionesEntrega { get; set; }

		// Un cliente puede tener muchas obras
		public virtual ICollection<Obra> Obras { get; set; }

		public virtual Provincia Provincia { get; set; }

		public virtual Poblacion Poblacion { get; set; }


		#endregion

	}
}