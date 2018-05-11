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
	public class ObrasController : Controller
	{
		FacturacionContext db = new FacturacionContext();

		Configuracion verObra;

		public ObrasController()
		{
			verObra = db.Configuraciones.FirstOrDefault();
		}

		// GET: Obras
		public ActionResult Index()
		{
			//return View(db.Obras.ToList());
			return View();
		}

		public JsonResult GetAll(jqGridViewModel jqgrid)
		{
			IEnumerable<Obra> _obras = db.Obras.ToList();

			// Si no quiero ver las obras terminadas
			//if (verObra.VerObrasTerminadas == false)
			//    _obras = _obras.Where(x => x.Acabada == false);

			if (jqgrid._search)
			{
				_obras = _obras.AsQueryable<Obra>().Where<Obra>(jqgrid.searchField, jqgrid.searchString, (WhereOperation)StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _obras.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;

				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

				_obras = _obras.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _obras.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = pageIndex,
					records = count,
					rows = (
								from obra in data
								select new
								{
									id = obra.ObraId.ToString(),
									cell = new string[] { 
														obra.ObraId.ToString(),
														obra.Nombre,
														obra.NombreAmpliado,
														obra.Direccion,
														obra.Localidad,
														obra.CP,
														obra.Provincia,
														obra.Telefono,
														obra.Movil,
														obra.Fax,
														obra.Tecnico,
														obra.Observaciones,
														obra.Mostrar.ToString(),
														obra.CertificacionesAnteriores.ToString(),
														obra.ContarAbonos.ToString(),
														obra.Acabada.ToString(),
														obra.ACodigoObra.ToString(),
														obra.Actividad.Nombre,
														//obra.ActividadId.ToString(),
														obra.SubActividad.Subactividad,
														obra.Delegacion.NombreDelegacion,
														obra.Cliente.NombreCliente,
														obra.FechaAlta.ToShortDateString(),
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

		public JsonResult GetByCliente(int id, jqGridViewModel jqgrid)
		{
			IEnumerable<Obra> _obras = db.Obras.Where(x => x.ClienteId == id).ToList();

			if (jqgrid._search)
			{
				_obras = _obras.AsQueryable<Obra>().Where<Obra>(jqgrid.searchField, jqgrid.searchString, (WhereOperation)StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _obras.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;

				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

				_obras = _obras.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _obras.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = pageIndex,
					records = count,
					rows = (
								from obra in data
								select new
								{
									id = obra.ObraId.ToString(),
									cell = new string[] { 
														obra.ObraId.ToString(),
														obra.Nombre,
														obra.Acabada.ToString()
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
		//public string EditActividad([Bind(Exclude = "ActividadId")] Obra actividad)
		public JsonResult EditObra([Bind(Exclude = "FechaAlta")]Obra obra)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Obra>(obra).State = EntityState.Modified;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Obra modificada correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}

		}

		public JsonResult AddObra([Bind(Exclude = "ObraId, FechaAlta")]	Obra obra)
		{
			try
			{
				if (ModelState.IsValid)
				{
					db.Entry<Obra>(obra).State = EntityState.Added;
					//db.Actividades.Add(actividad);
					db.SaveChanges();
					return Json(new { Error = false, Msg = "Obra añadida correctamente" });
				}
				else
					return Json(new { Error = true, Msg = "Modelo no válido" });
			}
			catch (Exception ex)
			{

				return Json(new { Error = true, Msg = ex.Message });
			}
		}

		public JsonResult DeleteObra(int Id)
		{
			var obra = db.Actividades.Find(Id);

			if (obra == null)
				return Json(new { Error = true, Msg = "Id no encontrado" });

			try
			{
				db.Actividades.Remove(obra);
				//db.Entry(Id).State = EntityState.Deleted;

				db.SaveChanges();

				return Json(new { Error = false, Msg = "Obra eliminada correctamente" });
			}
			catch (Exception ex)
			{
				return Json(new { Error = true, Msg = ex.Message });
			}
		}
	}
}