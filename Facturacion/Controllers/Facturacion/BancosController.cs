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
using System.Data.SqlClient;
using Facturacion.Helpers;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MvcGrid.Models.Helpers;

namespace Facturacion.Controllers.Facturacion
{
	public class BancosController : Controller
	{
		private FacturacionContext db = new FacturacionContext();

		int PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PAGE_SIZE"]);

		// GET: Bancos
		public ActionResult Index(int page = 1)
		{
			//object[] parameters = {
			//	new SqlParameter("@pageSize", PageSize),
			//	new SqlParameter("@pageNumber", page)
			//};

			//BancoViewModels model = new BancoViewModels
			//{
			//	//Bancos = db.Bancos
			//	//        .OrderBy(p => p.BancoId)
			//	//        .Skip((page - 1) * PageSize)
			//	//        .Take(PageSize),
			//	Bancos = db.Bancos.SqlQuery("Facturacion.dbo.GetBancosLimit @pageSize, @pageNumber", parameters).ToList(),
			//	PagingInfo = new PagingInfoViewModels
			//	{
			//		CurrentPage = page,
			//		ItemsPerPage = PageSize,
			//		TotalItems = db.Bancos.Count()
			//	}

			//};

			//return View(model);
			return View();
		}

		// GET: Bancos/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Banco banco = db.Bancos.Find(id);
			if (banco == null)
			{
				return HttpNotFound();
			}
			return View(banco);
		}

		// GET: Bancos/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Bancos/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "BancoId,Codigo,Nombre,Direccion")] Banco banco)
		{
			if (ModelState.IsValid)
			{
				db.Bancos.Add(banco);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(banco);
		}

		// GET: Bancos/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Banco banco = db.Bancos.Find(id);
			if (banco == null)
			{
				return HttpNotFound();
			}
			return View(banco);
		}

		// POST: Bancos/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "BancoId,Codigo,Nombre,Direccion")] Banco banco)
		{
			if (ModelState.IsValid)
			{
				db.Entry(banco).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(banco);
		}

		// GET: Bancos/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Banco banco = db.Bancos.Find(id);
			if (banco == null)
			{
				return HttpNotFound();
			}
			return View(banco);
		}

		// POST: Bancos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Banco banco = db.Bancos.Find(id);
			db.Bancos.Remove(banco);
			db.SaveChanges();
			return RedirectToAction("Index");
		}


		public JsonResult GetAll(jqGridViewModel jqgrid)
		{
			IEnumerable<Banco> _bancos = db.Bancos.ToList();

			if (jqgrid._search)
			{
				_bancos = _bancos.AsQueryable<Banco>().Where<Banco>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _bancos.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_bancos = _bancos.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _bancos.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = jqgrid.page,
					records = count,
					rows = (
								from banco in data
								select new
								{
									id = banco.BancoId.ToString(),
									cell = new string[] { 
													banco.BancoId.ToString(),
													banco.Codigo,
													banco.Nombre,
													banco.Direccion,
													banco.Mostrar.ToString(),
													banco.FechaAlta.ToString()
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
		public JsonResult EditBancos([Bind(Exclude = "FechaAlta")]Banco banco)
		{
			try
			{
				if (ModelState.IsValid)
				{
					if (TryUpdateModel(banco, "", new string[] { "Codigo", "Nombre", "Direccion", "Mostrar" }))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Banco modificado correctamente" });
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

		public JsonResult AddBancos([Bind(Exclude = "BancoId, FechaAlta")] Banco banco)
		{
			// TODO: Habría que mirar si el código del nuevo banco existe
			var existe = db.Bancos.FirstOrDefault(x => x.Codigo == banco.Codigo);
			if (existe != null) // es que existe
				return Json(new { Error = true, Msg = "Código de Banco existente.  Por favor, cámbielo." });

			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Banco>(banco).State = EntityState.Added;

					db.SaveChanges();
					return Json(new { Error = false, Msg = "Banco añadido correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteBancos(int Id)
		{
			var banco = db.Bancos.Find(Id);

			if (banco == null)
				return Json(new { Error = true, Msg = "Modelo no válido" });

			try
			{
				db.Bancos.Remove(banco);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Banco borrado correctamente" });
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
