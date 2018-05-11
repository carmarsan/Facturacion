using Facturacion.Context;
using Facturacion.Models;
using MvcGrid.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Controllers.Facturacion
{
	public class FacturasController : Controller
	{
		FacturacionContext db = new FacturacionContext();
		// GET: Facturas
		public ActionResult Index()
		{
			//return View(db.Facturas.ToList());


			return View();
		}

		/// <summary>
		/// Coge las facturas de una obra
		/// </summary>
		/// <param name="id">id de la obra</param>
		/// <param name="jqgrid">parámetros del grid</param>
		/// <returns></returns>
		public JsonResult GetFacturasDeObra(int id, jqGridViewModel jqgrid)
		{
			IEnumerable<Factura> _facturas = db.Facturas.Where(x => x.ObraId == id).ToList();

			if (jqgrid._search)
			{
				_facturas = _facturas.AsQueryable<Factura>().Where<Factura>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _facturas.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;

				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_facturas = _facturas.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _facturas.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = pageIndex,
					records = count,
					rows = (
								from factura in data
								select new
								{
									id = factura.FacturaId.ToString(),
									cell = new string[] { 
														factura.FacturaId.ToString(),
														factura.Orden.ToString(),
														factura.NumeroFactura.ToString(),
														factura.FechaFactura.ToShortDateString(),
														factura.BaseImponible.ToString().Replace(',', '.'),
														factura.Tipo,
														factura.ObraId.ToString(),
														factura.RotuloGerencia,
														factura.PorcentajeBonificacion.ToString().Replace(',', '.'),
														factura.PorcentajeRetencion.ToString().Replace(',', '.'),
														factura.Subtotal.ToString().Replace(',', '.'),
														factura.Bonificacion.ToString().Replace(',', '.'),
														factura.Retencion.ToString().Replace(',', '.'),
														factura.Total.ToString().Replace(',', '.'),
														factura.TotalAOrigen.ToString().Replace(',', '.'),
														factura.CertificacionesAnteriores.ToString().Replace(',', '.'),
														factura.Cerrada.ToString(),
														factura.FacturaEspecial.ToString(),
														factura.Contabilizado.ToString(),
														factura.TieneTotalPropio.ToString(),
														factura.Detalles.Sum(x => x.TotalIVA).ToString().Replace(',', '.')
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

		public JsonResult GetDataFromFactura(int? id)
		{
			var _data = db.Facturas.Where(x => x.FacturaId == id).FirstOrDefault();

			if (_data == null)
				return Json(new { Error = true, Mgs = string.Format("No se encuentra la Factura con el Id: {0}", id) });

			var result = new
			{
				CodCliente = _data.Obra.Cliente.ClienteId.ToString(),
				Cliente = _data.Obra.Cliente.NombreCliente,
				CodObra = _data.Obra.ObraId.ToString(),
				Obra = _data.Obra.Nombre,
				ObraPlus = _data.Obra.NombreAmpliado,
				TextoFac = _data.TextoFactura,
				DAS = string.Concat(_data.Obra.Delegacion.Abreviatura, "/", _data.Obra.Actividad.Abreviatura, "/", _data.Obra.SubActividad.Abreviatura),
				DireccionCliente = _data.Obra.Cliente.Domicilio,
				Localidad = _data.Obra.Cliente.Poblacion.NombrePoblacion,
				CP = _data.Obra.Cliente.CP,
				Provincia = _data.Obra.Cliente.Provincia.Nombre,
				CIF = _data.Obra.Cliente.CIF,
				FormaPago = _data.Obra.Cliente.FormaPago.Descripcion,
				Observaciones = _data.Obra.Observaciones

			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}