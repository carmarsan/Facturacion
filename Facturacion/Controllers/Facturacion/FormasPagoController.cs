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
	public class FormasPagoController : Controller
	{
		FacturacionContext db = new FacturacionContext();

		// GET: FormasPago
		public ActionResult Index()
		{
			return View();
			//return View(db.FormasPago.ToList());
		}


		public JsonResult GetAll(jqGridViewModel jqgrid)
		{

			IQueryable<FormaPago> _formapago = db.FormasPago;//.AsQueryable();

			//return Json(new { Error = 1, Msg = "Esto es un error" }, JsonRequestBehavior.AllowGet);
			//return Json(new Exception("Esto es un error como un pino"));

			// Si estamos usando filtros
			//filtring
			if (jqgrid._search)
			{
				//And
				if (jqgrid.filters.groupOP == "AND")

					foreach (var rule in jqgrid.filters.rules)
						_formapago = _formapago.Where<FormaPago>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
				else
				{
					//Or
					var temp = (new List<FormaPago>()).AsQueryable();
					foreach (var rule in jqgrid.filters.rules)
					{
						var t = _formapago.Where<FormaPago>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
						temp = temp.Concat<FormaPago>(t);
					}
					//remove repeating records
					_formapago = temp.Distinct<FormaPago>();
				}
			}

			try
			{
				var count = _formapago.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				//IEnumerable<Iva> ivasSort = null;

				_formapago = _formapago.OrderBy<FormaPago>(jqgrid.sidx, jqgrid.sord);

				//if (!string.IsNullOrWhiteSpace(jqgrid.sidx))

				//	ivasSort = jqgrid.sord == "asc" ? ivas.OrderBy(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x)) : ivas.OrderByDescending(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x));
				//else
				//	ivasSort = ivas.OrderBy(x => x.IvaId);

				//ivasSort = ivasSort.Skip((pageIndex - 1) * pageSize).Take(pageSize);
				var data = _formapago.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

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
									id = fp.FormaPagoId.ToString(),
									cell = new string[] { 
													fp.FormaPagoId.ToString(),
													fp.Descripcion,
													fp.TextoLargo,
													fp.Vcto1.ToString(),
													fp.Vcto2.ToString(),
													fp.Vcto3.ToString(),
													fp.Vcto4.ToString(),
													fp.Vcto5.ToString(),
													fp.Vcto6.ToString(),
													fp.Mostrar.ToString(),
													fp.FechaAlta.ToString()
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
		public JsonResult EditFormaPago([Bind(Exclude = "FechaAlta")]FormaPago formapago)
		{
			// TODO: Esto también vale.  Habría que poner los métodos en el Helper
			// bool b = UpdateDbEntry(banco, x => x.Codigo, x => x.Nombre, x => x.Direccion, x => x.Mostrar);

			var miformapago = db.FormasPago.Find(formapago.FormaPagoId);

			if (miformapago == null)
				return Json(new { Error = true, Msg = "No se encuentra la Forma de Pago en las tablas" });

			try
			{
				if (ModelState.IsValid)
				{
					//if (TryUpdateModel(miformapago, "", new string[] { "Descripcion", "Precio", "PrecioCompra", "PrecioVenta", "CodigoContable", "Ver" }))
					if (TryUpdateModel(miformapago))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Forma de pago grabada correctamente" });
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

		public JsonResult AddFormaPago([Bind(Exclude = "FormaPagoId, FechaAlta")]FormaPago formapago)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<FormaPago>(formapago).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Forma de pago añadida correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteFormaPago(int Id)
		{
			var formapago = db.FormasPago.Find(Id);

			if (formapago == null)
				return Json(new { Error = true, Msg = "Id de Forma Pago no encontrado" });

			// Buscamos si existe algún cliente que tenga esta forma de pago
			var tiene = db.Clientes.Where(x => x.FormaPagoId == Id).Count() > 0;
			if (tiene)
				return Json(new { Error = true, Msg = "Imposible el borrado.  Existen clientes con esta forma de Pago" });

			try
			{
				db.FormasPago.Remove(formapago);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Forma de pago eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		
	}
}