using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models
{
    [Table("Familias")]
    public class Familia
    {
        private string _fecha = DateTime.Now.ToString();
        #region Propiedades

        public Familia()
        {
            this.FechaAlta = DateTime.Now;
        }

        //[Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        [Display(Name = "Id")]
        public int FamiliaId { get; set; }

        [Required]
        [Display(Name = "Familia")]
        [Column(TypeName = "varchar")]
        [StringLength(150, ErrorMessage = "Debe introducir obligatoriamente un {0} o {1}")]
        public string NombreFamilia { get; set; }

        [Required]
        [Display(Name = "Fecha Alta")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FechaAlta { get; set; }

        [Display(Name = "Codigo Contable Compras")]
        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string CodigoContableCompras { get; set; }

        [Display(Name = "Codigo Contable Ventas")]
        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string CodigoContableVentas { get; set; }

        //public virtual ICollection<Articulo> Articulos { get; set; }

        #endregion
    }
}
