using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Models
{
	[ModelBinder(typeof(GridModelBinder))]
	public class jqGridViewModel
	{
		public string sidx { get; set; }
		public string sord { get; set; }
		public int page { get; set; }
		public int rows { get; set; }
		public bool _search { get; set; }
		public string searchField { get; set; }
		public string searchString { get; set; }
		public string searchOper { get; set; }
		public Filters filters { get; set; }
		//public string filter { get; set; }
	}
	//public class jqGridViewModel
	//{
	//	public string sidx { get; set; }
	//	public string sord { get; set; }
	//	public int page { get; set; }
	//	public int rows { get; set; }
	//	public bool _search { get; set; }
	//	public Filters filters { get; set; }
	//	//public string filter { get; set; }
	//}

	public class Filters
	{
		public string groupOP { get; set; }
		public Rules[] rules { get; set; }

		public static Filters Create(string jsonData)
		{
			try
			{
				var serializer = new DataContractJsonSerializer(typeof(Filters));
				System.IO.StringReader reader = new System.IO.StringReader(jsonData);
				System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData));
				return serializer.ReadObject(ms) as Filters;
			}
			catch
			{
				return null;
			}
		}
	}

	public class Rules
	{
		public string field { get; set; }
		public string op { get; set; }
		public string data { get; set; }

	}
}