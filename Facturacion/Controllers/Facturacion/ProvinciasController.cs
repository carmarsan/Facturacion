using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;

namespace Facturacion.Controllers.Facturacion
{

	public class ProvinciasController : Controller
	{

		private FacturacionContext db = new FacturacionContext();

		// GET: Provincias
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetAll()
		{
			var result = (from p in db.Provincias.ToList().OrderBy(x => x.Nombre).ToList()
						  select new { Id = p.ProvinciaId, Texto = p.Nombre }).ToList();

			result.Insert(0, new { Id = 0, Texto = "Seleccione una Provincia" });

			return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}