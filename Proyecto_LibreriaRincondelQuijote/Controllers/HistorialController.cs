using Proyecto_LibreriaRincondelQuijote.Models;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Proyecto_LibreriaRincondelQuijote.Controllers
{
    public class HistorialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Acción para ver el historial de compras del usuario
        public ActionResult VerHistorial()
        {
            // Obtener el UserId del usuario autenticado
            string userId = User.Identity.GetUserId();

            // Obtener el historial de compras para el usuario autenticado, incluyendo los productos
            var historialCompras = db.HistorialCompras
                                     .Where(h => h.UserId == userId)
                                     .Include(h => h.Producto)  // Asegura que el Producto asociado se cargue
                                     .OrderByDescending(h => h.FechaCompra)
                                     .ToList();

            // Si el usuario no tiene historial de compras, redirigimos o mostramos un mensaje
            if (historialCompras == null || !historialCompras.Any())
            {
                TempData["Error"] = "No tienes historial de compras.";
                return RedirectToAction("Index", "Home"); // Redirigir a la página principal si no tiene historial
            }

            // Devolver el historial a la vista
            return View(historialCompras);
        }

    }
}
