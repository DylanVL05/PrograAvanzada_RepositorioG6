using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using Proyecto_LibreriaRincondelQuijote.Models;

[assembly: OwinStartupAttribute(typeof(Proyecto_LibreriaRincondelQuijote.Startup))]
namespace Proyecto_LibreriaRincondelQuijote
{
        public partial class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                ConfigureAuth(app);
                InitializeRoles();  // Llamar solo a la creación de roles
            }

          /**  private void InitializeRoles()
            {
                using (var context = new ApplicationDbContext())
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    // Crear el rol "Admin" si no existe
                    if (!roleManager.RoleExists("Admin"))
                    {
                        var role = new IdentityRole("Admin");
                        roleManager.Create(role);
                    }

                    // Crear el rol "User" si no existe
                    if (!roleManager.RoleExists("User"))
                    {
                        var role = new IdentityRole("User");
                        roleManager.Create(role);
                    }

                    // Crear un usuario "Admin" si no existe
                    var user = userManager.FindByEmail("admin@dominio.com");

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = "admin@dominio.com",
                            Email = "admin@dominio.com"
                        };

                        // Crear el usuario con una contraseña
                        var result = userManager.Create(user, "Password@123");  // Cambia esta contraseña según lo necesario

                        if (result.Succeeded)
                        {
                            // Asignar el rol "Admin" al nuevo usuario
                            userManager.AddToRole(user.Id, "Admin");
                        }
                    }**/
                }
            }
        
    
