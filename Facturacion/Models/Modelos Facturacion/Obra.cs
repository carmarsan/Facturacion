using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Facturacion.Models
{

    public class Obra
    {

        #region Constructores

        public Obra()
        {
            this.FechaAlta = DateTime.Now;
        }

        #endregion

        #region Propiedades

        [Key]
        //[DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int ObraId { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
        public string Nombre { get; set; }

        [StringLength(512)]
        [Column(TypeName = "varchar")]
        public string NombreAmpliado { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
        public string Direccion { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(124, MinimumLength = 5, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
        public string Localidad { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Longitud del campo {0} errónea.  Debe estar entre {1} y {2} caracteres")]
        public string CP { get; set; }

        //[Required]
        [StringLength(100)]
        [Column(TypeName = "varchar")]
        public string Provincia { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar")]
        public string Telefono { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar")]
        public string Movil { get; set; }

        [StringLength(15)]
        [Column(TypeName = "varchar")]
        public string Fax { get; set; }

        [StringLength(125)]
        [Column(TypeName = "varchar")]
        public string Tecnico { get; set; }

        [StringLength(2048)]
        [Column(TypeName = "varchar")]
        public string Observaciones { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool Mostrar { get; set; }

        [DefaultValue(true)]
        public bool CertificacionesAnteriores { get; set; }

        [DefaultValue(true)]
        public bool ContarAbonos { get; set; }

        [DefaultValue(false)]
        public bool Acabada { get; set; }

        [DefaultValue(false)]
        public bool ACodigoObra { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [HiddenInput(DisplayValue = false)]
        public DateTime FechaAlta { get; set; }

        [Required]
        public int ActividadId { get; set; }

        [Required]
        public int SubActividadId { get; set; }

        [Required]
        public int DelegacionId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        //[Required]
        //public int FacturaId { get; set; }


        #region VIRTUALES

        // Una obra tiene una Actividad
        public virtual Actividad Actividad { get; set; }

        // Una obra tiene una SubActividad
        public virtual SubActividad SubActividad { get; set; }

        // Una obra tiene una Delegación
        public virtual Delegacion Delegacion { get; set; }

        // Una obra tiene un Cliente
        public virtual Cliente Cliente { get; set; }


        // Un obra tiene muchas facturas
        public virtual ICollection<Factura> Facturas { get; set; }

        #endregion

        #endregion

    }
}
