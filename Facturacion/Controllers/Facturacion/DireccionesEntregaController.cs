using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Context;
using Facturacion.Models;
using MvcGrid.Models.Helpers;
using System.Data.Entity;
using Facturacion.ModelView;

namespace Facturacion.Controllers.Facturacion
{
	public class DireccionesEntregaController : Controller
	{
		FacturacionContext db = new FacturacionContext();

		// GET: DireccionesEntrega
		public ActionResult Index()
		{
			return View();
			//return View(db.DireccionesEntrega.ToList());
		}

		public JsonResult GetAll(jqGridViewModel jqgrid)
		{

			//IQueryable<Cliente> _cliente = db.Clientes.Include("Provincia");//.AsQueryable();
			IEnumerable<DireccionEntrega> _direccionies = db.DireccionesEntrega.ToList();

			//return Json(new { Error = 1, Msg = "Esto es un error" }, JsonRequestBehavior.AllowGet);
			//return Json(new Exception("Esto es un error como un pino"));

			// Si estamos usando filtros
			//filtring
			if (jqgrid._search)
			{
				_direccionies = _direccionies.AsQueryable<DireccionEntrega>().Where<DireccionEntrega>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
				//if (jqgrid.filters.groupOP == "AND")
				//{
				//	foreach (var rule in jqgrid.filters.rules)
				//		//_cliente = _cliente.AsQueryable().Where<Cliente>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op));
				//		_cliente = _cliente.AsQueryable<Cliente>().Where<Cliente>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op)).ToList();
				//}
				//else
				//{
				//	//Or
				//	var temp = (new List<Cliente>()).AsQueryable();
				//	//IEnumerable<Cliente> temp =  null;
				//	foreach (var rule in jqgrid.filters.rules)
				//	{
				//		var t = _cliente.AsQueryable<Cliente>().Where<Cliente>(rule.field, rule.data, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), rule.op)).ToList();
				//		temp = temp.Concat<Cliente>(t);
				//	}
				//	//remove repeating records
				//	_cliente = temp.Distinct<Cliente>();

				//}
			}

			try
			{
				var count = _direccionies.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				//IEnumerable<Iva> ivasSort = null;

				//_cliente = _cliente.OrderBy<Cliente>(jqgrid.sidx, jqgrid.sord);

				_direccionies = _direccionies.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				//if (!string.IsNullOrWhiteSpace(jqgrid.sidx))

				//	_cliente = jqgrid.sord == "asc" ? _cliente.OrderBy(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x)) : _cliente.OrderByDescending(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x));
				//else
				//	_cliente = _cliente.OrderBy(x => x.ClienteId);

				//ivasSort = ivasSort.Skip((pageIndex - 1) * pageSize).Take(pageSize);
				var data = _direccionies.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

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
									id = fp.DireccionEntregaId.ToString(),
									cell = new string[] { 
													fp.DireccionEntregaId.ToString(),
													fp.Direccion,
													fp.Localidad,
													fp.CP,
													fp.ProvinciaId.ToString(), 
													fp.CIF,
													fp.PersonaContacto,
													fp.Cliente == null ? string.Empty : fp.Cliente.NombreCliente,
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
		public JsonResult EditDireccion([Bind(Exclude = "FechaAlta")]DireccionEntrega direccion)
		{
			// TODO: Esto también vale.  Habría que poner los métodos en el Helper
			// bool b = UpdateDbEntry(banco, x => x.Codigo, x => x.Nombre, x => x.Direccion, x => x.Mostrar);

			var miDireccion = db.Clientes.Find(direccion.DireccionEntregaId);

			if (miDireccion == null)
				return Json(new { Error = true, Msg = "No se encuentra la Dirección de Entrega en las tablas" });

			try
			{
				if (ModelState.IsValid)
				{
					//if (TryUpdateModel(miformapago, "", new string[] { "Descripcion", "Precio", "PrecioCompra", "PrecioVenta", "CodigoContable", "Ver" }))
					if (TryUpdateModel(miDireccion))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Dirección de Entrega grabada correctamente" });
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

		public JsonResult AddDireccion([Bind(Exclude = "DireccionEntregaId, FechaAlta")]DireccionEntrega direccion)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<DireccionEntrega>(direccion).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Dirección de Entrega añadido correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteDireccion(int Id)
		{
			var direccion = db.DireccionesEntrega.Find(Id);

			if (direccion == null)
				return Json(new { Error = true, Msg = "Id no encontrado" });

			// TODO: Tenemos que controlar que no se borre una dirección que tenga algún cliente
			//var clientesCount = db.Clientes.Where(x => x. == Id).Count() > 0;

			//if (obrasCount)
			//	return Json(new { Error = true, Msg = "El Cliente no se puede eliminar ya que dispone de obras." });

			try
			{
				db.DireccionesEntrega.Remove(direccion);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Dirección de Entrega eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		/// <summary>
		/// Actualizamos las direcciones de Entrega de los clientes.
		/// </summary>
		/// <param name="datos"></param>
		/// <returns></returns>
		public JsonResult UpdateClientDirections(DirectionsClient datos)
		{
			foreach (string ids in datos.IdsDirecciones)
			{
				var direccion = db.DireccionesEntrega.Find(int.Parse(ids));
				if (direccion != null)
				{
					if (direccion.Cliente == null)
					{
						direccion.Cliente = db.Clientes.Find(datos.IdCliente);

						if (direccion.Cliente == null)
							return Json(new { Error = true, Msg = string.Format("Error encontrando al Cliente con Id: {0}", datos.IdCliente) });
					}
					else
						direccion.Cliente.ClienteId = datos.IdCliente;

					if (TryUpdateModel(direccion))
					{
						db.SaveChanges();
						//
					}
					else
						return Json(new { Error = true, Msg = "Error en la Actualización" });
				}
				//else
				//	return Json(new { Error = true, Msg = "Dirección No encontrada" });
			}

			return Json(new { Error = false, Msg = "Direcciones de Entrega grabadas correctamente" });
		}



		//public ActionResult GetFormasPago()
		//{
		//	var result = (from p in db.FormasPago.ToList()
		//				  select new { Id = p.FormaPagoId, Texto = p.Descripcion }).ToList();

		//	return Json(result, JsonRequestBehavior.AllowGet);
		//}
	}
}