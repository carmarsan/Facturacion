using Facturacion.Context;
using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Facturacion.Controllers.API
{
	public class ActividadesAPIController : ApiController
	{
		private FacturacionContext db = new FacturacionContext();

		// GET: api/ArticulosAPI
		//public JsonResult Get(jqGridViewModel jqgrid)
		//{
		//	var actividades = db.Actividades.ToList();

		//	var count = actividades.Count;
		//	int pageIndex = jqgrid.page;
		//	int pageSize = jqgrid.rows;

		//	int startRow = (pageIndex * pageSize) + 1;
		//	int totalRecords = count;
		//	int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

		//	var result = new
		//	{
		//		total = totalPages,
		//		page = pageIndex,
		//		records = count,
		//		rows = actividades.Select(x => new
		//		{
		//			x.ActividadID,
		//			x.Nombre,
		//			x.Abreviatura,
		//			x.Mostrar
		//		}
		//									).ToArray()
		//				.Select(x => new
		//				{
		//					id = x.ActividadID,
		//					cell = new string[] {	x.ActividadID.ToString(),  
		//															x.Nombre,   
		//															x.Abreviatura,  
		//															x.Mostrar.ToString()  
		//														  }
		//				}
		//			  ).ToArray()
		//	};


		//	return Json(result, JsonRequestBehavior.AllowGet);

		//}

		// GET: api/ArticulosAPI/5
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/ArticulosAPI
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/ArticulosAPI/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/ArticulosAPI/5
		public void Delete(int id)
		{
		}
	}
}
