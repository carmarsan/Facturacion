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

namespace Facturacion.Controllers.Facturacion
{
	public class ActividadesController : Controller
	{
		private FacturacionContext db = new FacturacionContext();
		int PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PAGE_SIZE"]);

		// GET: Actividades
		public ActionResult Index(int page = 1)
		{
			ActividadViewModels model = new ActividadViewModels
			{
				Actividades = db.Actividades
							.OrderBy(p => p.ActividadID)
							.Skip((page - 1) * PageSize)
							.Take(PageSize),
				PagingInfo = new PagingInfoViewModels
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = db.Actividades.Count()
					//TotalItems = category == null ?
					//    db.Products.Count():
					//    db.Products.Where(e => e.Category == category).Count()
				}
			};

			return View(model);
		}

		// GET: Actividades/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Actividad actividad = db.Actividades.Find(id);
			if (actividad == null)
			{
				return HttpNotFound();
			}
			return View(actividad);
		}

		// GET: Actividades/Create
		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Actividades/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ActividadID,Nombre,Abreviatura")] Actividad actividad)
		{
			if (ModelState.IsValid)
			{
				db.Actividades.Add(actividad);
				db.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(actividad);
		}

		// GET: Actividades/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Actividad actividad = db.Actividades.Find(id);
			if (actividad == null)
			{
				return HttpNotFound();
			}
			return View(actividad);
		}

		// POST: Actividades/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ActividadID,Nombre,Abreviatura")] Actividad actividad)
		{
			if (ModelState.IsValid)
			{
				db.Entry(actividad).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(actividad);
		}

		// GET: Actividades/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			Actividad actividad = db.Actividades.Find(id);

			if (actividad == null)
			{
				return HttpNotFound();
			}

			return View(actividad);
		}

		// POST: Actividades/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Actividad actividad = db.Actividades.Find(id);

			// TODO:  Tenemos que mirar si está en algún registro de las Facturas
			var isInObras = db.Obras.Where(c => c.ActividadId == id).Count();

			if (isInObras == null)
			{
				db.Actividades.Remove(actividad);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			else
			{
				ViewBag.ActividadIsInObra = true;
				return View();
			}

		}

		public JsonResult GetAll(jqGridViewModel jqgrid)
		{
			//var actividades = db.Actividades.ToList();
			IEnumerable<Actividad> _actividades = db.Actividades.ToList();

			if (jqgrid._search)
			{
				_actividades = _actividades.AsQueryable<Actividad>().Where<Actividad>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _actividades.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;

				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_actividades = _actividades.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _actividades.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = pageIndex,
					records = count,
					rows = (
								from actividad in data
								select new
								{
									id = actividad.ActividadID.ToString(),
									cell = new string[] { 
														actividad.ActividadID.ToString(),
														actividad.Nombre,
														actividad.Abreviatura,
														actividad.Mostrar.ToString()
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
		public JsonResult EditActividad([Bind(Exclude = "FechaAlta")]Actividad actividad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Actividad>(actividad).State = EntityState.Modified;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Actividad modificada correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}

		}

		public JsonResult AddActividad([Bind(Exclude = "ActividadId, ActividadId")]	Actividad actividad)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Actividad>(actividad).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Actividad añadida correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteActividad(int Id)
		{
			var actividad = db.Actividades.Find(Id);

			if (actividad == null)
				return Json(new { Error = true, Msg = "Id no encontrado" });

			try
			{
				db.Actividades.Remove(actividad);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Actividad eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult GetSelect()
		{
			var result = (from p in db.Actividades.ToList().OrderBy(x => x.Nombre).ToList()
						  select new { Id = p.ActividadID, Texto = p.Nombre }).ToList();

			result.Insert(0, new { Id = 0, Texto = "Seleccione una Actividad" });

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
