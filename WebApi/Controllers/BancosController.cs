using Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
	public class BancosController : ApiController
	{
		Banco[] bancos = new Banco[]
		{
			new Banco {BancoId = 1, Codigo = "cod1", Nombre = "Nombre 1", Direccion = "Direccion 1", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 2, Codigo = "cod2", Nombre = "Nombre 2", Direccion = "Direccion 2", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 3, Codigo = "cod3", Nombre = "Nombre 3", Direccion = "Direccion 3", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 4, Codigo = "cod4", Nombre = "Nombre 4", Direccion = "Direccion 4", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 5, Codigo = "cod5", Nombre = "Nombre 5", Direccion = "Direccion 5", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 6, Codigo = "cod6", Nombre = "Nombre 6", Direccion = "Direccion 6", FechaAlta = DateTime.Now, Mostrar = true},
			new Banco {BancoId = 7, Codigo = "cod7", Nombre = "Nombre 7", Direccion = "Direccion 7", FechaAlta = DateTime.Now, Mostrar = true},

		};

		public dynamic GetAll(string sidx, string sord, int page, int rows)
		{
			//var bancos = bancos.ToList();

			var count = bancos.Length;
			int pageIndex = Convert.ToInt32(page) - 1;
			int pageSize = rows;

			int startRow = (pageIndex * pageSize) + 1;
			int totalRecords = count;
			int totalPages = ( int )Math.Ceiling(( float )totalRecords / ( float )pageSize);

			return new
			{
				total = totalPages,
				page = pageIndex,
				records = count,
				rows = (
							from banco in bancos
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
							.Skip((pageIndex - 1) * pageSize)
							.Take(pageSize)
							.ToArray()
			};
		}

		public Banco GetBancoById(int id)
		{
			var mibanco = bancos.FirstOrDefault(x => x.BancoId == id);

			if (mibanco == null)
				throw new HttpResponseException(HttpStatusCode.NotFound);

			return mibanco;
		}


	}
}
