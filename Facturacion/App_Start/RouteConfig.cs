using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Facturacion
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "Page{page}",
                new { controller = "Actividades", action = "Index" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "Page{page}",
                new { controller = "Articulos", action = "Index" },
                new { page = @"\d+" }
            );

            //routes.MapRoute(null,
            //    "Page{page}",
            //    new { controller = "Bancos", action = "Index" },
            //    new { page = @"\d+" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
