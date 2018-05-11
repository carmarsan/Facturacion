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
using MvcGrid.Models.Helpers;

namespace Facturacion.Controllers.Facturacion
{
	public class DetallesController : Controller
	{
		private FacturacionContext db = new FacturacionContext();

		// GET: Detalles
		public ActionResult Index()
		{
			return View(db.Detalles.ToList());
		}

		// GET: Detalles/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Detalle detalle = db.Detalles.Find(id);
			if (detalle == null)
			{
				return HttpNotFound();
			}
			return View(detalle);
		}

		// GET: Detalles/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Detalles/Create
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "DetalleID,NumFac,Articulo_Id,IdArticuloAlternativo,Orden,Cantidad,Iva_Id,PorcentajeIva,TextoIva,IVA,PVP,ImporteMes,ImporteAnterior")] Detalle detalle)
		{
			if (ModelState.IsValid)
			{
				db.Detalles.Add(detalle);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(detalle);
		}

		// GET: Detalles/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Detalle detalle = db.Detalles.Find(id);
			if (detalle == null)
			{
				return HttpNotFound();
			}
			return View(detalle);
		}

		// POST: Detalles/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "DetalleID,NumFac,Articulo_Id,IdArticuloAlternativo,Orden,Cantidad,Iva_Id,PorcentajeIva,TextoIva,IVA,PVP,ImporteMes,ImporteAnterior")] Detalle detalle)
		{
			if (ModelState.IsValid)
			{
				db.Entry(detalle).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(detalle);
		}

		// GET: Detalles/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Detalle detalle = db.Detalles.Find(id);
			if (detalle == null)
			{
				return HttpNotFound();
			}
			return View(detalle);
		}

		// POST: Detalles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Detalle detalle = db.Detalles.Find(id);
			db.Detalles.Remove(detalle);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Buscamos la líneas de detalle de una factura
		/// </summary>
		/// <param name="id">Id de la Factura</param>
		/// <returns></returns>
		//public JsonResult GetDetails(int id)
		//{
		//	//db.Configuration.ProxyCreationEnabled = false;

		//	//var _detalle = db.Detalles.Where(x => x.FacturaId == id).ToList();

		//	var _factura = db.Facturas.FirstOrDefault(x => x.FacturaId == id);

		//	if (_factura == null)
		//		return Json(new { Error = true, Msg = "Factura no encontrada." });

		//	var result = new
		//	{
		//		detalle = (from deta in _factura.Detalles
		//				   select new
		//				   {
		//					   _D_DetalleId = deta.DetalleId,
		//					   _D_Articulo = deta.Articulo,
		//					   _D_ArticuloAlternativo = deta.ArticuloAlternativo,
		//					   _D_Orden = deta.Orden,
		//					   _D_Cantidad = deta.Cantidad,
		//					   _D_PrecioUnitario = deta.PrecioUnitario,
		//					   _D_PorcentajeIva = deta.PorcentajeIva,
		//					   _D_Importe = deta.Importe,
		//					   _D_TextoIva = deta.TextoIva,
		//					   _D_TotalIva = deta.TotalIVA,
		//				   }
		//				),
		//		factura = new
		//		{
		//			_F_NumeroFactura = _factura.NumeroFactura,
		//			_F_Orden = _factura.Orden,
		//			_F_FechaFactura = _factura.FechaFactura.ToShortDateString(),
		//			_F_Tipo = _factura.Tipo,
		//			_F_idSobreFactura = _factura.IdSobreFactura,
		//			_F_SobreFactura = _factura.SobreFactura,
		//			_F_RotuloGerencia = _factura.RotuloGerencia,
		//			_F_PorcentajeBonificacion = _factura.PorcentajeBonificacion.ToString(),
		//			_F_PorcentajeRetencion = _factura.PorcentajeRetencion.ToString(),
		//			_F_Subtotal = _factura.Subtotal.ToString(),
		//			_F_Bonificacion = _factura.Bonificacion.ToString(),
		//			_F_BaseImponible = _factura.BaseImponible.ToString(),
		//			_F_Retencion = _factura.Retencion.ToString(),
		//			_F_TotalAOrigen = _factura.TotalAOrigen.ToString(),
		//			_F_CertificacionesAnteriores = _factura.CertificacionesAnteriores.ToString(),
		//			_F_Facturardo = _factura.Facturado.ToString(),
		//			_F_TipoFacturacion = _factura.TipoFacturacion,
		//			_F_Cerrada = _factura.Cerrada.ToString(),
		//			_F_FacturaEspecial = _factura.FacturaEspecial.ToString(),
		//			_F_FacturaUnica = _factura.FacturaUnica.ToString(),
		//			_F_Contabilizado = _factura.Contabilizado.ToString(),
		//			_F_TieneTotalPropio = _factura.TieneTotalPropio.ToString(),
		//			_F_TextoFactura = _factura.TextoFactura,
		//			_F_Total = _factura.Total
		//		}

		//	};

		//	return Json(result, JsonRequestBehavior.AllowGet);

		//}


		public JsonResult GetDetails(int id, jqGridViewModel jqgrid)
		{
			IEnumerable<Detalle> _detalle = db.Detalles.Where(x => x.FacturaId == id).ToList();

			if (_detalle == null)
				return Json(new { Error = true, Msg = "Detalle no encontrado." });

			if (jqgrid._search)
			{
				_detalle = _detalle.AsQueryable<Detalle>().Where<Detalle>(jqgrid.searchField, jqgrid.searchString, ( WhereOperation )StringEnum.Parse(typeof(WhereOperation), jqgrid.searchOper)).ToList();
			}

			try
			{
				var count = _detalle.Count();
				int pageIndex = jqgrid.page;
				int pageSize = jqgrid.rows;

				int startRow = (pageIndex * pageSize) + 1;
				int totalRecords = count;
				int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

				_detalle = _detalle.AsQueryable().OrderBy(jqgrid.sidx, jqgrid.sord).ToList();

				var data = _detalle.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToArray();

				var result = new
				{
					total = totalPages,
					page = pageIndex,
					records = count,
					rows = (
								from detalle in data
								select new
								{
									id = detalle.DetalleId.ToString(),
									cell = new string[] { 
														detalle.DetalleId.ToString(),
														detalle.Orden.ToString(),
														detalle.Articulo,
														detalle.Cantidad.ToString().Replace(',', '.'),
														detalle.PrecioUnitario.ToString().Replace(',', '.'),
														detalle.Importe.ToString().Replace(',', '.'),
													    detalle.ArticuloAlternativo,
														detalle.PorcentajeIva.ToString().Replace(',', '.'),
														detalle.TextoIva,
														detalle.TotalIVA.ToString().Replace(',', '.'),
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

		/// <summary>
		/// Devuelve una lista de los IVA sumados de una factura
		/// </summary>
		/// <param name="id">id de la Factura</param>
		/// <returns></returns>
		public JsonResult GetIvasDeFactura(int id)
		{
			var result = from line in db.Detalles.AsEnumerable()
						 where line.FacturaId == id
						 group line by new { line.TextoIva, line.PorcentajeIva } into g
						 select new
						 {
							 _TextoIva = g.Key.TextoIva,
							 // _CuotaIva = 0,
							 _PorcentajeIva = g.Key.PorcentajeIva,
							 _BaseIva = g.Sum(_ => _.Importe),
							 _CuotaIva = g.Sum((x => (x.PorcentajeIva / 100) * x.Importe)).ToString().Replace(',', '.')
						 };


			//result.ForEach(x => x._CuotaIva = (x._PorcentajeIva / 100) * x._CuotaIva);
			//foreach (var item in result)
			//{

			//	item._CuotaIva = ((Convert.ToDouble(item._PorcentajeIva) / 100) * Convert.ToDouble(item._BaseIva)).ToString();
			//}

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

	public class miIva
	{
		public decimal _BaseIva { get; set; }
		public string _TextoIva { get; set; }
		public decimal _CuotaIva { get; set; }
		public decimal _PorcentajeIva { get; set; }
	}
}
