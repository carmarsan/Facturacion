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
	public class ArticulosController : Controller
	{
		private FacturacionContext db = new FacturacionContext();
		int PageSize = 4;

		#region MVC NORMAL
		// GET: Articulos
		public ActionResult Index(int page = 1)
		{
			return View();
			//var arti = db.Articulos.ToList();
			//ArticuloViewModels articulosVM = new ArticuloViewModels
			//{
			//	Articulos = db.Articulos
			//		//.Include(a => a.Iva)
			//					.OrderBy(p => p.ArticuloId)
			//					.Skip((page - 1) * PageSize)
			//					.Take(PageSize),
			//	PagingInfo = new PagingInfoViewModels
			//	{
			//		CurrentPage = page,
			//		ItemsPerPage = PageSize,
			//		TotalItems = db.Articulos.Count()
			//	}
			//};


			//return View(articulosVM);
		}

		// GET: Articulos/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Articulo articulo = db.Articulos.Find(id);
			if (articulo == null)
			{
				return HttpNotFound();
			}
			return View(articulo);
		}

		// GET: Articulos/Create
		//[Authorize(Roles = "View")]
		public ActionResult Create()
		{
			ViewBag.IvaId = new SelectList(db.Ivas, "IvaId", "Texto");
			return View();
		}

		// POST: Articulos/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "ArticuloId,Descripcion,Precio,IvaId,PrecioCompra,PrecioVenta,Ver,CodigoContable")] Articulo articulo)
		{
			if (ModelState.IsValid)
			{
				db.Articulos.Add(articulo);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			// ViewBag.IvaId = new SelectList(db.Ivas, "IvaId", "Texto", articulo.IvaId);
			return View(articulo);
		}

		// GET: Articulos/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Articulo articulo = db.Articulos.Find(id);
			if (articulo == null)
			{
				return HttpNotFound();
			}
			//ViewBag.IvaId = new SelectList(db.Ivas, "IvaId", "Texto", articulo.IvaId);
			return View(articulo);
		}

		// POST: Articulos/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "ArticuloId,Descripcion,Precio,IvaId,PrecioCompra,PrecioVenta,Ver,CodigoContable")] Articulo articulo)
		{
			if (ModelState.IsValid)
			{
				db.Entry(articulo).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			//ViewBag.IvaId = new SelectList(db.Ivas, "IvaId", "Texto", articulo.IvaId);
			return View(articulo);
		}

		// GET: Articulos/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Articulo articulo = db.Articulos.Find(id);
			if (articulo == null)
			{
				return HttpNotFound();
			}
			return View(articulo);
		}

		// POST: Articulos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Articulo articulo = db.Articulos.Find(id);

			db.Articulos.Remove(articulo);
			try
			{
				db.SaveChanges();
			}
			catch (Exception)
			{

				throw;
			}
			return RedirectToAction("Index");
		}
		#endregion


		public JsonResult GetAll(jqGridViewModel jqgrid)
		{

			IQueryable<Articulo> articulos = db.Articulos.Include("Iva");//.AsQueryable();

			//return Json(new { Error = 1, Msg = "Esto es un error" }, JsonRequestBehavior.AllowGet);
			//return Json(new Exception("Esto es un error como un pino"));

			// Si estamos usando filtros
			//filtring
			if (jqgrid._search)
			{
				//StringBuilder sb = new StringBuilder();
				//string rtRule = string.Empty;
				//string rtWhere = string.Empty;
				//JavaScriptSerializer serializer = new JavaScriptSerializer();
				////Filters filters = serializer.Deserialize<Filters>(jqgrid.filter);
				//Filters filters = serializer.Deserialize<Filters>(Request.QueryString["filters"]);

				//foreach (Rules regla in filters.rules)
				//{
				//	rtRule = LinqDynamicConditionHelper.GetCondition<Iva>(regla);
				//	if (rtRule.Length > 0)
				//	{
				//		sb.Append(rtRule);
				//		sb.Append(filters.groupOP.ToLower() == "and" ? " && " : " || ");
				//	}
				//}

				//ivas.Where<T>(sb.ToString(), 1);


				//And
				if (jqgrid.filters.groupOP == "AND")

					foreach (var rule in jqgrid.filters.rules)
						articulos = articulos.Where<Articulo>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
				else
				{
					//Or
					var temp = (new List<Articulo>()).AsQueryable();
					foreach (var rule in jqgrid.filters.rules)
					{
						var t = articulos.Where<Articulo>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
						temp = temp.Concat<Articulo>(t);
					}
					//remove repeating records
					articulos = temp.Distinct<Articulo>();
				}
			}

			try
			{
				var count = articulos.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				//IEnumerable<Iva> ivasSort = null;

				articulos = articulos.OrderBy<Articulo>(jqgrid.sidx, jqgrid.sord);

				//if (!string.IsNullOrWhiteSpace(jqgrid.sidx))

				//	ivasSort = jqgrid.sord == "asc" ? ivas.OrderBy(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x)) : ivas.OrderByDescending(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x));
				//else
				//	ivasSort = ivas.OrderBy(x => x.IvaId);

				//ivasSort = ivasSort.Skip((pageIndex - 1) * pageSize).Take(pageSize);
				var data = articulos.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = jqgrid.page,
					records = count,
					rows = (
						//from ivaSort in ivasSort
								from articulo in data
								select new
								{
									id = articulo.ArticuloId.ToString(),
									cell = new string[] { 
													articulo.ArticuloId.ToString(),
													articulo.Descripcion,
													articulo.Precio.ToString(),
													articulo.PrecioCompra.ToString(),
													articulo.PrecioVenta.ToString(),
													articulo.Iva.Texto,
													//articulo.Iva.IvaId.ToString(),
													articulo.CodigoContable,
													articulo.Ver.ToString()
												}
								}).ToArray()
				};

				return Json(result, JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		// Por si queremos excluir algún campo
		//public string EditActividad([Bind(Exclude = "ActividadId")] Actividad actividad)
		public JsonResult EditArticulo([Bind(Exclude = "ArticuloId, FechaAlta, IvaId")]Articulo articulo)
		{
			// TODO: Esto también vale.  Habría que poner los métodos en el Helper
			// bool b = UpdateDbEntry(banco, x => x.Codigo, x => x.Nombre, x => x.Direccion, x => x.Mostrar);

			//return Json(new { Error = false, Msg = "Esto es un error" });
			//return new Exception("Esto es una excepcion");

			var miArticulo = db.Articulos.Find(articulo.IvaId);

			if (miArticulo == null)
				return Json(new { Error = true, Msg = "No se encuentra el IVA en las tablas" });

			// No se puede borrar el iva por defecto del registro que lo tiene. Siempre tiene que haber un IVA por defecto
			//if (iva.IvaId == miIva.IvaId && (iva.Defecto == false && miIva.Defecto == true))
			//	return Json(new { Error = true, Msg = "No se puede quitar el valor por defecto.  Siempre tiene que haber un IVA por defecto." });

			try
			{
				if (ModelState.IsValid)
				{
					if (TryUpdateModel(miArticulo, "", new string[] { "Descripcion", "Precio", "PrecioCompra", "PrecioVenta", "CodigoContable", "Ver" }))
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

		public JsonResult AddArticulo([Bind(Exclude = "ArticuloId, FechaAlta")]Articulo articulo)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Articulo>(articulo).State = EntityState.Added;
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

		public JsonResult DeleteArticulo(int Id)
		{
			var articulo = db.Articulos.Find(Id);

			if (articulo == null)
				return Json(new { Error = true, Msg = "Id de Artículo no encontrado" });

			try
			{
				db.Articulos.Remove(articulo);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Artículo eliminado correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
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
