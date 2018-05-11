using Facturacion.Context;
using Facturacion.Models;
using Facturacion.Helpers;
using MvcGrid.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facturacion.Helpers;

namespace Facturacion.Controllers.Facturacion
{
	public class ClientesController : Controller
	{

		private FacturacionContext db = new FacturacionContext();

		// GET: Clientes
		public ActionResult Index()
		{
			return View();
			//return View(db.Clientes.ToList());
		}

		public JsonResult GetAll(jqGridViewModel jqgrid)
		{

			//IQueryable<Cliente> _cliente = db.Clientes.Include("Provincia");//.AsQueryable();
			IEnumerable<Cliente> _cliente = db.Clientes.ToList();

			//return Json(new { Error = 1, Msg = "Esto es un error" }, JsonRequestBehavior.AllowGet);
			//return Json(new Exception("Esto es un error como un pino"));

			// Si estamos usando filtros
			//filtring
			if (jqgrid._search)
			{
				_cliente = _cliente.AsQueryable<Cliente>().Where<Cliente>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
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
				var count = _cliente.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;
				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				//IEnumerable<Iva> ivasSort = null;

				//_cliente = _cliente.OrderBy<Cliente>(jqgrid.sidx, jqgrid.sord);

				_cliente = _cliente.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				//if (!string.IsNullOrWhiteSpace(jqgrid.sidx))

				//	_cliente = jqgrid.sord == "asc" ? _cliente.OrderBy(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x)) : _cliente.OrderByDescending(x => x.GetType().GetProperty(jqgrid.sidx).GetValue(x));
				//else
				//	_cliente = _cliente.OrderBy(x => x.ClienteId);

				//ivasSort = ivasSort.Skip((pageIndex - 1) * pageSize).Take(pageSize);
				var data = _cliente.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

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
									id = fp.ClienteId.ToString(),
									cell = new string[] { 
													fp.ClienteId.ToString(),
													//fp.CodigoCliente.ToString(),
													fp.NombreCliente,
													fp.Titular,
													fp.Domicilio,
													fp.Provincia.Nombre, // ProvinciaId.ToString(),
													fp.Poblacion.NombrePoblacion,
													fp.CP,
													fp.CIF,
													fp.Telefono1,
													fp.Telefono2,
													fp.Movil1,
													fp.Movil2,
													fp.Fax1,
													fp.Fax2,
													fp.Email,
													fp.Web,
													fp.Observaciones,
													fp.PersonaContacto,
													
													fp.Sello.ToString(),
													fp.CodigoContable,
													fp.FormaPago.Descripcion,
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
		public JsonResult EditCliente([Bind(Exclude = "FechaAlta")]Cliente cliente)
		{
			// TODO: Esto también vale.  Habría que poner los métodos en el Helper
			// bool b = UpdateDbEntry(banco, x => x.Codigo, x => x.Nombre, x => x.Direccion, x => x.Mostrar);

			var micliente = db.Clientes.Find(cliente.ClienteId);

			if (micliente == null)
				return Json(new { Error = true, Msg = "No se encuentra el cliente en las tablas" });

			// Antes de nada comprobamos el CIF en caso de tenerlo.
			if (!string.IsNullOrWhiteSpace(cliente.CIF))
				if (!ValidateNIF.valida_NIFCIFNIE(cliente.CIF))
					return Json(new { Error = true, Msg = "CIF erróneo" });

			try
			{
				if (ModelState.IsValid)
				{
					//if (TryUpdateModel(miformapago, "", new string[] { "Descripcion", "Precio", "PrecioCompra", "PrecioVenta", "CodigoContable", "Ver" }))
					if (TryUpdateModel(micliente))
					{
						db.SaveChanges();
						return Json(new { Error = false, Msg = "Cliente grabada correctamente" });
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

		public JsonResult AddCliente([Bind(Exclude = "ClienteId, FechaAlta")]Cliente cliente)
		{
			// Antes de nada comprobamos el CIF en caso de tenerlo.
			if (!string.IsNullOrWhiteSpace(cliente.CIF))
				if (!ValidateNIF.valida_NIFCIFNIE(cliente.CIF))
					return Json(new { Error = true, Msg = "CIF erróneo" });

			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Cliente>(cliente).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Cliente añadido correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteCliente(int Id)
		{
			var cliente = db.Clientes.Find(Id);

			if (cliente == null)
				return Json(new { Error = true, Msg = "Id de Cliente no encontrado" });

			// TODO: Tenemos que controlar que no tenga ninguna obra
			var obrasCount = db.Obras.Where(x => x.ClienteId == Id).Count() > 0;

			if (obrasCount)
				return Json(new { Error = true, Msg = "El Cliente no se puede eliminar ya que dispone de obras." });

			try
			{
				db.Clientes.Remove(cliente);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Cliente eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public ActionResult GetFormasPago()
		{
			var result = (from p in db.FormasPago.ToList()
						  select new { Id = p.FormaPagoId, Texto = p.Descripcion }).ToList();

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetSelectClientes()
		{
			var result = (from p in db.Clientes.ToList().OrderBy(x => x.NombreCliente).ToList()
						  select new { Id = p.ClienteId, Texto = p.NombreCliente }).ToList();

			result.Insert(0, new { Id = 0, Texto = "Seleccione un Cliente" });

			return Json(result, System.Web.Mvc.JsonRequestBehavior.AllowGet);
		}
	}
}