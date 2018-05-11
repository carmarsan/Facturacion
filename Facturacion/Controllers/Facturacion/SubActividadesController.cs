using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;

namespace Facturacion.Controllers.Facturacion
{
	public class SubActividadesController : Controller
	{
		FacturacionContext db = new FacturacionContext();

		// GET: SubActividades
		public ActionResult Index()
		{
			return View(db.SubActividades.ToList());
		}

		public JsonResult GetSelect()
		{
			var result = (from p in db.SubActividades.ToList().OrderBy(x => x.Subactividad).ToList()
						  select new { Id = p.SubActividadId, Texto = p.Subactividad }).ToList();

			result.Insert(0, new { Id = 0, Texto = "Seleccione una SubActividad" });

			return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
		}
	}
}