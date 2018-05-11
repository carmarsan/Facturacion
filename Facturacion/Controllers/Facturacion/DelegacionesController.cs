using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;
using Facturacion.Models;
using MvcGrid.Models.Helpers;
using System.Data.Entity;

namespace Facturacion.Controllers.Facturacion
{
	public class DelegacionesController : Controller
	{
		FacturacionContext db = new FacturacionContext();

		// GET: Delegaciones
		public ActionResult Index()
		{
			return View();
			//return View(db.Delegaciones.ToList());
		}

		public JsonResult GetAll(jqGridViewModel jqgrid)
		{

			//IQueryable<Cliente> _cliente = db.Clientes.Include("Provincia");//.AsQueryable();
			IEnumerable<Delegacion> _delegaciones = db.Delegaciones.ToList();

			//return Json(new { Error = 1, Msg = "Esto es un error" }, JsonRequestBehavior.AllowGet);
			//return Json(new Exception("Esto es un error como un pino"));

			// Si estamos usando filtros
			//filtring
			if (jqgrid._search)
			{
				_delegaciones = _delegaciones.AsQueryable<Delegacion>().Where<Delegacion>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _delegaciones.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_delegaciones = _delegaciones.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();
				var data = _delegaciones.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = jqgrid.page,
					records = count,
					rows = (
						//from ivaSort in ivasSort
								from fp in data
								select new
								{
									id = fp.DelegacionId.ToString(),
									cell = new string[] { 
													fp.DelegacionId.ToString(),
													fp.NombreDelegacion,
													fp.Abreviatura,
													fp.Mostrar.ToString(),
													fp.FechaAlta.ToString(), 
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
		public JsonResult EditDelegacion([Bind(Exclude = "FechaAlta")]Delegacion delegacion)
		{
			var miDelegacion = db.Delegaciones.Find(delegacion.DelegacionId);

			if (miDelegacion == null)
				return Json(new { Error = true, Msg = "No se encuentra la Delegación en las tablas" });

			try
			{
				if (ModelState.IsValid)
				{
					//if (TryUpdateModel(miformapago, "", new string[] { "Descripcion", "Precio", "PrecioCompra", "PrecioVenta", "CodigoContable", "Ver" }))
					if (TryUpdateModel(miDelegacion))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Actualización grabada correctamente" });
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

		public JsonResult AddDelegacion([Bind(Exclude = "DelegacionId, FechaAlta")]Delegacion direccion)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Delegacion>(direccion).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Delegación añadida correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteDelegacion(int Id)
		{
			var delegacion = db.Delegaciones.Find(Id);

			if (delegacion == null)
				return Json(new { Error = true, Msg = "Delegación no encontrada" });

			var existe = db.Obras.Count(x => x.DelegacionId == Id);

			if (existe > 0)
				return Json(new { Error = true, Msg = string.Format("Esta Delegación no se puede borrar porque existe en {0} obras", existe) });

			try
			{
				db.Delegaciones.Remove(delegacion);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Delegación eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult GetSelect()
		{
			var result = (from p in db.Delegaciones.ToList().OrderBy(x => x.NombreDelegacion).ToList()
						  select new { Id = p.DelegacionId, Texto = p.NombreDelegacion }).ToList();

			result.Insert(0, new { Id = 0, Texto = "Seleccione una Delegación" });

			return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
		}
	}
}