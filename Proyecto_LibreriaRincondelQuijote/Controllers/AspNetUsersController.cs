using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proyecto_LibreriaRincondelQuijote.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_LibreriaRincondelQuijote.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AspNetUsersController()
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            roleManager = new RoleManager<IdentityRole>(roleStore);
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // Acción para ver los usuarios
        public ActionResult Index()
        {
            var users = userManager.Users.ToList();

            // Creamos un diccionario para almacenar los roles de cada usuario
            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                var roles = userManager.GetRoles(user.Id); // Obtén los roles para el usuario
                userRoles[user.Id] = roles; // Guarda los roles del usuario en el diccionario
            }

            // Pasamos los usuarios y sus roles a la vista
            ViewBag.UserRoles = userRoles;

            return View(users);
        }


        // Acción para editar el rol de un usuario
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var roles = roleManager.Roles.ToList();
            var userRoles = await userManager.GetRolesAsync(user.Id);

            ViewBag.Roles = new SelectList(roles, "Name", "Name", userRoles.FirstOrDefault());
            return View(user);
        }

        // Acción para guardar los cambios en el rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string selectedRole)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var currentRoles = await userManager.GetRolesAsync(user.Id);
            foreach (var role in currentRoles)
            {
                await userManager.RemoveFromRoleAsync(user.Id, role);
            }

            if (!string.IsNullOrEmpty(selectedRole))
            {
                await userManager.AddToRoleAsync(user.Id, selectedRole);
            }

            return RedirectToAction("Index");
        }

        // Acción para eliminar un usuario (opcional)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
    }
}
