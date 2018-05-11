using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
    [Table("Configuraciones")]
    public class Configuracion
    {
        [Key]
        public int ConfiguracionId { get; set; }

        //[Required]
        [Display(Name = "Abreviatura")]
        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string Abreviatura { get; set; }

        // [Required]
        [Display(Name = "Numeración Factura")]
        public int NumeracionFactura { get; set; }

        //[Required]
        [Display(Name = "Numeración Abono")]
        public int NumeracionAbono { get; set; }

        //[Required]
        [Display(Name = "Nombre Delegación")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string NombreDelegacion { get; set; }

        // [Required]
        [Display(Name = "Dirección")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Direccion { get; set; }

        // [Required]
        [Display(Name = "CP")]
        [Column(TypeName = "varchar")]
        [StringLength(5)]
        public String CP { get; set; }

        //[Required]
        [Display(Name = "Ciudad")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Ciudad { get; set; }

        //[Required]
        [Display(Name = "Provincia")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Provincia { get; set; }

        [Display(Name = "Telefono 1")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Telefono1 { get; set; }

        [Display(Name = "Telefono 2")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Telefono2 { get; set; }

        [Display(Name = "Fax 1")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Fax1 { get; set; }

        [Display(Name = "Fax 2")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Fax2 { get; set; }

        [Display(Name = "Movil1 ")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Movil1 { get; set; }

        [Display(Name = "Movil 2")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String Movil2 { get; set; }

        [Display(Name = "Persona Contacto 1")]
        [Column(TypeName = "varchar")]
        public String PersonaContacto1 { get; set; }

        [Display(Name = "Persona Contacto 2")]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public String PersonaContacto2 { get; set; }

        [Display(Name = "Observaciones")]
        [Column(TypeName = "varchar")]
        [StringLength(1024, MinimumLength = 10)]
        public String Observaciones { get; set; }


        //[Required]
        [Display(Name = "Ruta Conta Plus")]
        [Column(TypeName = "varchar")]
        [StringLength(1024, MinimumLength = 10)]
        public String RutaContaPlus { get; set; }

        //[Required]
        [Display(Name = "Ruta Exportación Contaplus")]
        [Column(TypeName = "varchar")]
        [StringLength(1024, MinimumLength = 10)]
        public String RutaExportacionContaPlus { get; set; }

        // [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Por favor, introduzca una dirección de email correcta")]
        [EmailAddress(ErrorMessage = "Por favor, introduzca una dirección de email correcta")]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        [Display(Name = "Forma de Pago")]
        public int? FormaPagoId { get; set; }


        //public virtual FormaPago FormaPago { get; set; }
    }
}