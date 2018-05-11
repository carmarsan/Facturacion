using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Facturacion.ModelView
{
    public class UsuarioViewModel
    {

        public string UsuarioId { get; set; }

        [Display(Name="Usuario")]
        [Required(ErrorMessage="Debe introducir un {0}")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Debe introducir un {0}")]
        public string Email { get; set; }

        public RoleViewModel Role { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
}