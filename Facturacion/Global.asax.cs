using Facturacion.Context;
using Facturacion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Facturacion
{
	public class MvcApplication : System.Web.HttpApplication
	{
		string[] _roles = { "Admin", "canView", "canEdit", "canCreate", "canDelete" };

		protected void Application_Start()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<FacturacionContext, Migrations.Configuration>());

			ApplicationDbContext db = new ApplicationDbContext();
			CreateRoles(db);
			CreateSuperUser(db);
			AddPermisionsToSuperuser(db);

			db.Dispose();

			// Register API routes

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			//Force JSON responses on all requests
			//GlobalConfiguration.Configuration.Formatters.Clear();
			//GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());

		}

		/// <summary>
		/// Añadimos los permisos al superusuario
		/// </summary>
		/// <param name="db"></param>
		private void AddPermisionsToSuperuser(ApplicationDbContext db)
		{
			string nameSuperUser = System.Configuration.ConfigurationManager.AppSettings["SUPERUSER"];

			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

			var user = userManager.FindByName(nameSuperUser);

			foreach (var rol in _roles)
			{
				if (!userManager.IsInRole(user.Id, rol))
					userManager.AddToRole(user.Id, rol);
			}
		}

		private void CreateSuperUser(ApplicationDbContext db)
		{
			string nameSuperUser = System.Configuration.ConfigurationManager.AppSettings["SUPERUSER"];

			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

			var user = userManager.FindByName(nameSuperUser);
			if (user == null)
			{
				user = new ApplicationUser
				{
					UserName = nameSuperUser,
					Email = nameSuperUser
				};
				string passSuperUser = System.Configuration.ConfigurationManager.AppSettings["PASS_SUPERUSER"];
				userManager.Create(user, passSuperUser);
			}
		}

		/// <summary>
		/// Creamos roles por defecto, en caso de no existir
		/// </summary>
		/// <param name="db">ApplicationDbContext</param>
		private void CreateRoles(ApplicationDbContext db)
		{
			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

			foreach (var rol in _roles)
			{
				if (!roleManager.RoleExists(rol))
					roleManager.Create(new IdentityRole(rol));
			}

		}
	}
}
