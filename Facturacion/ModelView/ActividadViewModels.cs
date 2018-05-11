using Facturacion.HtmlHelpers;
using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facturacion.ModelView
{
    public class ActividadViewModels
    {
        public IEnumerable<Actividad> Actividades { get; set; }
        public PagingInfoViewModels PagingInfo { get; set; }

    }
}