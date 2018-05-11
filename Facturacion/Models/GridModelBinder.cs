using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Models
{
	public class GridModelBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			try
			{
				var request = controllerContext.HttpContext.Request;
				return new jqGridViewModel
				{
					_search = bool.Parse(request["_search"] ?? "false"),
					page = int.Parse(request["page"] ?? "1"),
					rows = int.Parse(request["rows"] ?? "10"),
					sidx = request["sidx"] ?? "",
					sord = request["sord"] ?? "asc",
					searchField = request["searchField"] ?? "asc",
					searchOper = request["searchOper"] ?? "asc",
					searchString = request["searchString"] ?? "asc",
					filters = Filters.Create(request["filters"] ?? "")
				};
			}
			catch
			{
				return null;
			}
		}
	}
}