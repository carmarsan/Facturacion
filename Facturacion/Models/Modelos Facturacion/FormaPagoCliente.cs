using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Facturacion.Models.Modelos_Facturacion
{
    public class FormaPagoCliente
    {
        //[Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int FormaPagoClienteId { get; set; }
        public int FormaPagoId { get; set; }
        public int ClienteId { get; set; }

        //public virtual FormaPago FormaPagos { get; set; }
        //public virtual Cliente Clientes { get; set; }
    }
}