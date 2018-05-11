using Facturacion.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Controllers.Facturacion
{
	public class PoblacionesController : Controller
	{

		private FacturacionContext db = new FacturacionContext();

		// GET: Poblaciones
		public ActionResult Index()
		{
			return View();
		}

		public JsonResult GetSelect(int id)
		{
			var lista = db.Poblaciones.ToList();

			if (id != 0)
				lista = lista.Where(x => x.ProvinciaId == id).ToList();

			var result = (from p in lista
						  select new { Id = p.PoblacionId, Texto = p.NombrePoblacion, CP = p.CP }).ToList();

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public string GetCP(int id)
		{
			return db.Poblaciones.Where(x => x.PoblacionId == id).FirstOrDefault().CP.ToString();
		}
	}
}