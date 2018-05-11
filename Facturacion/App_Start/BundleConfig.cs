using System.Web;
using System.Web.Optimization;

namespace Facturacion
{
	public class BundleConfig
	{
		// Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-1.12.4.js",
						"~/Scripts/jquery-ui-1.11.4.js",
						"~/Scripts/i18n/grid.locale-es.js",
						"~/Scripts/jquery.unobtrusive-ajax.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqGrid").Include(
						"~/Scripts/jquery.jqGrid.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
			// preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
						"~/Scripts/bootstrap.js",
						"~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/themes/redmon/jquery-ui.css",
						"~/Content/bootstrap.css",
						"~/Content/jquery.jqGrid/ui.jqgrid-bootstrap.css",
				//"~/Content/themes/redmon/jquery-ui.css",
					  "~/Content/site.css"));
		}
	}
}
