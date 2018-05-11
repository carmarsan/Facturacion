using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;
using Facturacion.Models;
using Facturacion.ModelView;
using Facturacion.HtmlHelpers;
using MvcGrid.Models.Helpers;
using System.Web.Script.Serialization;
using System.Text;
using Facturacion.Helpers;
using System.Linq.Dynamic;

namespace Facturacion.Controllers.Facturacion
{
	public class IvasController : Controller
	{
		private FacturacionContext db = new FacturacionContext();

		int PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PAGE_SIZE"]);

		// GET: Ivas
		public ActionResult Index(int page = 1)
		{
			return View();
			//IvaViewModel model = new IvaViewModel
			//{

			//	Ivas = db.Ivas
			//			.OrderBy(p => p.IvaId)
			//			.Skip((page - 1) * PageSize)
			//			.Take(PageSize),
			//	PagingInfo = new PagingInfoViewModels
			//	{
			//		CurrentPage = page,
			//		ItemsPerPage = PageSize,
			//		TotalItems = db.Ivas.Count()
			//	}

			//};

			//return View(model);
		}

		// GET: Ivas/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Iva iva = db.Ivas.Find(id);
			if (iva == null)
			{
				return HttpNotFound();
			}
			return View(iva);
		}

		// GET: Ivas/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Ivas/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "IvaId,Texto,Porcentaje,CuentaContable,Defecto")] Iva iva)
		{
			if (ModelState.IsValid)
			{
				db.Ivas.Add(iva);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(iva);
		}

		// GET: Ivas/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Iva iva = db.Ivas.Find(id);
			if (iva == null)
			{
				return HttpNotFound();
			}
			return View(iva);
		}

		// POST: Ivas/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "IvaId,Texto,Porcentaje,CuentaContable,Defecto")] Iva iva)
		{
			if (ModelState.IsValid)
			{
				db.Entry(iva).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(iva);
		}

		// GET: Ivas/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Iva iva = db.Ivas.Find(id);
			if (iva == null)
			{
				return HttpNotFound();
			}
			return View(iva);
		}

		// POST: Ivas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Iva iva = db.Ivas.Find(id);

			// Controlamos que ningún articulo tenga ese IVA
			//if (db.Articulos.Any(p => p.IvaId == id))
			//{
			//    ViewBag.Error = "Iva ya asociado con algún artículo. Imposible borrarlo";
			//    return View(iva);
			//}

			db.Ivas.Remove(iva);
			db.SaveChanges();
			return RedirectToAction("Index");
		}


		public JsonResult GetAll(jqGridViewModel jqgrid)
		{
			IEnumerable<Iva> _ivas = db.Ivas.ToList();//.AsQueryable();

			if (jqgrid._search)
			{
				_ivas = _ivas.AsQueryable<Iva>().Where<Iva>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();

				//And
				//if (jqgrid.filters.groupOP == "AND")

				//	foreach (var rule in jqgrid.filters.rules)
				//		ivas = ivas.Where<Iva>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
				//else
				//{
				//	//Or
				//	var temp = (new List<Iva>()).AsQueryable();
				//	foreach (var rule in jqgrid.filters.rules)
				//	{
				//		var t = ivas.Where<Iva>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
				//		temp = temp.Concat<Iva>(t);
				//	}
				//	//remove repeating records
				//	ivas = temp.Distinct<Iva>();
				//}
			}

			try
			{
				var count = _ivas.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_ivas = _ivas.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _ivas.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = jqgrid.page,
					records = count,
					rows = (
						//from ivaSort in ivasSort
								from iva in data
								select new
								{
									id = iva.IvaId.ToString(),
									cell = new string[] { 
													iva.IvaId.ToString(),
													iva.Texto,
													iva.Porcentaje.ToString(),
													iva.CuentaContable,
													iva.Defecto.ToString(),
													iva.Mostrar.ToString()
												}
								}).ToArray()
				};

				return Json(result, JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}

		}

		// Por si queremos excluir algún campo
		//public string EditActividad([Bind(Exclude = "ActividadId")] Actividad actividad)
		public JsonResult EditIva([Bind(Exclude = "FechaAlta")]Iva iva)
		{

			var miIva = db.Ivas.Find(iva.IvaId);

			if (miIva == null)
				return Json(new { Error = true, Msg = "No se encuentra el IVA en las tablas" });

			// No se puede borrar el iva por defecto del registro que lo tiene. Siempre tiene que haber un IVA por defecto
			if (iva.IvaId == miIva.IvaId && (iva.Defecto == false && miIva.Defecto == true))
				return Json(new { Error = true, Msg = "No se puede quitar el valor por defecto.  Siempre tiene que haber un IVA por defecto." });

			// Quitamos el IVA por defecto al registro que lo tenga
			try
			{
				CambioIvaPorDefecto();
			}
			catch (Exception)
			{
				return Json(new { Error = true, Msg = "Error interno eleminando el IVA por defecto" });
			}


			try
			{
				if (ModelState.IsValid)
				{
					if (TryUpdateModel(miIva, "", new string[] { "Texto", "Porcentaje", "CuentaContable", "Defecto", "Mostrar" }))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Registro grabado correctamente" });
					}
					else
						return Json(new { Error = true, Msg = "Error en la Actualización" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}


		}

		public JsonResult AddIva([Bind(Exclude = "IvaId, FechaAlta")]Iva iva)
		{
			// TODO: Habría que mirar si el código del nuevo banco existe
			var existe = db.Ivas.FirstOrDefault(x => x.Texto == iva.Texto);

			if (existe != null) // es que existe
				return Json(new { Error = true, Msg = "Texto de Iva existente.  Por favor, cámbielo." });

			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Iva>(iva).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Registro añadido correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteIva(int Id)
		{
			var iva = db.Ivas.Find(Id);

			if (iva == null)
				return Json(new { Error = true, Msg = "Id de IVA no encontrado" });

			// TODO: Tenemos que ver que pasa cuando se quiera borrar un IVA  que está en un artículo.
			var cuenta = db.Articulos.Count(c => c.IvaId == iva.IvaId);
			if (cuenta > 0)
				return Json(new { Error = true, Msg = "No se puede borrar el IVA seleccionado.  Existen artículos que lo tienen." });

			try
			{
				db.Ivas.Remove(iva);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Registro eliminado." });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		/// <summary>
		/// Pone a False el Iva por defecto del registro que lo tenga.
		/// </summary>
		/// <param name="id">Id del registro a poner por defecto</param>
		private void CambioIvaPorDefecto()
		{
			var ivaPorDefecto = db.Ivas.FirstOrDefault(c => c.Defecto == true);

			ivaPorDefecto.Defecto = false;

			db.Entry<Iva>(ivaPorDefecto).State = EntityState.Modified;

			db.SaveChanges();

		}

		public ActionResult GetSelectIvas()
		{
			var result = (from p in db.Ivas.ToList()
						  select new { Id = p.IvaId, Texto = p.Texto }).ToList();

			return Json(result, JsonRequestBehavior.AllowGet);
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
