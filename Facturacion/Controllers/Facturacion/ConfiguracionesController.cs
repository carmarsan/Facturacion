using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;

namespace Facturacion.Controllers.Facturacion
{
    public class ConfiguracionesController : Controller
    {
        FacturacionContext db = new FacturacionContext();

        // GET: Configuraciones
        public ActionResult Index()
        {
            return View(db.Configuraciones.ToList());
        }
    }
}