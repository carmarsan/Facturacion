using Facturacion.Context;
using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Facturacion.Controllers.API
{
	public class BancosController : ApiController
	{
		private FacturacionContext db = new FacturacionContext();

		public dynamic GetAll(string sidx, string sord, int page, int rows/*jqGridViewModel jqgrid*/)
		{
			var bancos = db.Bancos.ToList();

			var count = bancos.Count;
			int pageIndex = Convert.ToInt32(page) - 1;
			int pageSize = rows;

			int startRow = (pageIndex * pageSize) + 1;
			int totalRecords = count;
			int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

			IEnumerable<Banco> banks = null;
			if (!string.IsNullOrWhiteSpace(sidx))

				banks = sord == "asc" ? (from x in bancos orderby sidx select x) : (from x in bancos orderby sidx descending select x);
			//if (sord == "asc")
			//	banks = ;
			//else
			//	banks = bancos.OrderByDescending(p => sidx);

			//banks = banks.Skip(pageIndex * pageSize).Take(pageSize);


			//banks = banks.OrderBy(sidx + " " + sord);



			var result = new
							{
								total = totalPages,
								page = page,
								records = count,
								rows = (
											from banco in banks
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



			return result;
		}

		public Banco GetBancoById(int? id)
		{
			var mibanco = db.Bancos.FirstOrDefault(x => x.BancoId == id);

			if (mibanco == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return mibanco;
		}

		// Añadir
		public HttpResponseMessage PostProduct(Banco banco)
		{
			// Ponemos la fecha de alta
			banco.FechaAlta = DateTime.Now;

			try
			{
				db.Entry<Banco>(banco).State = EntityState.Added;

				db.SaveChanges();

				return Request.CreateResponse<Banco>(HttpStatusCode.Created, banco);
			}
			catch (Exception ex)
			{

				return Request.CreateResponse<Banco>(HttpStatusCode.BadRequest, banco);
			}

			//var response = Request.CreateResponse<Banco>(HttpStatusCode.Created, item);

			//string uri = Url.Link("DefaultApi", );//, new { id = item.Id });
			//response.Headers.Location = new Uri(uri);
			//return response;

		}

		// Actualizar
		//[ResponseType(typeof(void))]
		//[HttpPut]
		//public IHttpActionResult PutBanco(int id, Banco banco)
		//{
		//	banco.BancoId = id;

		//	try
		//	{
		//		db.Entry<Banco>(banco).State = EntityState.Modified;

		//		db.SaveChanges();

		//		return Ok(banco);
		//	}
		//	catch (Exception)
		//	{
		//		return NotFound();
		//	}
		//}

		public void PutBanco(Banco banco, int id)
		{
			//banco.BancoId = id;

			try
			{
				db.Entry<Banco>(banco).State = EntityState.Modified;

				db.SaveChanges();

				//return Ok(banco);
			}
			catch (Exception)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
			}
		}

		public void DeleteBanco(int id)
		{
			var banco = db.Bancos.Find(id);

			db.Entry<Banco>(banco).State = EntityState.Deleted;

			db.SaveChanges();
		}


		// Por si queremos excluir algún campo
		//public string EditActividad([Bind(Exclude = "ActividadId")] Actividad actividad)
		//public string ([Bind(Exclude = "FechaAlta")]Actividad actividad)
		//{
		//	try
		//	{
		//		db.Entry<Actividad>(actividad).State = EntityState.Modified;
		//		//db.Actividades.Add(actividad);
		//		db.SaveChanges();
		//		return "OK";
		//	}
		//	catch (Exception)
		//	{
		//		return "NOK";
		//	}

		//}

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
