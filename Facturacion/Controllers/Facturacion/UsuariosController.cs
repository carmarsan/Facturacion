using Facturacion.Models;
using Facturacion.ModelView;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Facturacion.Controllers.Facturacion
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Usuarios
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var users = userManager.Users.ToList();
            var usersView = new List<UsuarioViewModel>();

            foreach (var usuario in users)
            {
                var userView = new UsuarioViewModel
                {
                    Email = usuario.Email,
                    Name = usuario.UserName,
                    UsuarioId = usuario.Id
                };
                usersView.Add(userView);
            }

            return View(usersView);
        }

        public ActionResult Roles(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var users = userManager.Users.ToList();
            var allRoles = roleManager.Roles.ToList();
            var user = users.Find(u => u.Id == userId);

            if (user == null)
                return HttpNotFound();

            var roles = (from ur in user.Roles
                         let rol = allRoles.Where(s => s.Id == ur.RoleId).FirstOrDefault()
                         select new RoleViewModel { RoleId = rol.Id, Nombre = rol.Name }).ToList();

            var userView = new UsuarioViewModel
            {
                Email = user.Email,
                Name = user.UserName,
                UsuarioId = user.Id,
                Roles = roles
            };

            return View(userView);
        }

        /// <summary>
        /// Añade un rol al usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult AddRoles(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userId);
            var allRoles = roleManager.Roles.ToList();

            if (user == null)
                return HttpNotFound();

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel
            {
                Email = user.Email,
                Name = user.UserName,
                UsuarioId = user.Id,
                Roles = (from ur in user.Roles
                         let rol = allRoles.Where(s => s.Id == ur.RoleId).FirstOrDefault()
                         select new RoleViewModel { RoleId = rol.Id, Nombre = rol.Name }).ToList()
            };


            allRoles.OrderBy(s => s.Name).ToList();
            allRoles.Insert(0, new IdentityRole { Id = "-1", Name = "[Seleccione un roll...]" });
            ViewBag.RoleId = new SelectList(allRoles, "Id", "Name");

            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult AddRoles(string userId, FormCollection form)
        {
            var roleId = Request["RoleId"];

            // devolvemos a la vista los datos 
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userId);
            var allRoles = roleManager.Roles.ToList();

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel
            {
                Email = user.Email,
                Name = user.UserName,
                UsuarioId = user.Id,
                Roles = (from ur in user.Roles
                         let rol = allRoles.Where(s => s.Id == ur.RoleId).FirstOrDefault()
                         select new RoleViewModel { RoleId = rol.Id, Nombre = rol.Name }).ToList()
            };

            if (roleId == null)
            {
                ViewBag.Error = "Debe seleccionar un Rol";

                allRoles.OrderBy(s => s.Name).ToList();
                allRoles.Insert(0, new IdentityRole { Id = "-1", Name = "[Seleccione un roll...]" });
                ViewBag.RoleId = new SelectList(allRoles, "Id", "Name");

                return View(usuarioViewModel);
            }

            var role = roleManager.Roles.ToList().Find(r => r.Id == roleId);
            if (!userManager.IsInRole(userId, role.Name))
            {
                userManager.AddToRole(userId, role.Name);
            }

            return View("Roles", usuarioViewModel);
        }

        public ActionResult Delete(string userId, string rolId)
        {



            return View();
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
}