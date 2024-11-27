using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
     
    public class CatalogoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Catalogo
        public ActionResult Index(string categoriaFiltro, decimal? minPrecio, decimal? maxPrecio, int? disponibilidad)
        {
            // Inicializamos la consulta a los productos y sus relaciones
            var productos = db.Productos.Include(p => p.Categoria).Include(p => p.Estado).AsQueryable();

            // Filtrar por categoría
            if (!string.IsNullOrEmpty(categoriaFiltro))
            {
                productos = productos.Where(p => p.Categoria.Categoria_.Contains(categoriaFiltro));
            }

            // Filtrar por precio
            if (minPrecio.HasValue)
            {
                productos = productos.Where(p => p.Precio >= minPrecio.Value);
            }

            if (maxPrecio.HasValue)
            {
                productos = productos.Where(p => p.Precio <= maxPrecio.Value);
            }

            // Filtrar por disponibilidad
            if (disponibilidad.HasValue)
            {
                // 1: Disponible
                if (disponibilidad.Value == 1)
                {
                    productos = productos.Where(p => p.Disponibilidad > 0);
                }
                // 0: No disponible
                else if (disponibilidad.Value == 0)
                {
                    productos = productos.Where(p => p.Disponibilidad == 0);
                }
            }

            // Obtener todas las categorías disponibles para el filtro de categoría (sin duplicados)
            var categorias = db.Categorias.Select(c => c.Categoria_).Distinct().ToList();
            ViewBag.Categorias = categorias ?? new List<string>();

            // Obtener el rango de precios disponible en la base de datos (mínimo y máximo)
            try
            {
                ViewBag.MinPrecio = db.Productos.Min(p => p.Precio);
                ViewBag.MaxPrecio = db.Productos.Max(p => p.Precio);
            }
            catch (InvalidOperationException)
            {
                // Si no hay productos en la base de datos entonces valores predeterminados
                ViewBag.MinPrecio = 0;
                ViewBag.MaxPrecio = 0;
            }
            // Opciones de disponibilidad: 0: No disponible, 1: Disponible
            ViewBag.Disponibilidad = new List<int> { 0, 1 };

            // Ejecutamos la consulta y obtenemos la lista de productos filtrados
            var productosFiltrados = productos.ToList();

            return View(productosFiltrados);
        }



        [Authorize]
        public ActionResult AgregarCarrito(int id)
        {
            // Obtener el usuario autenticado
            string userId = User.Identity.GetUserId(); // Obtiene el UserId del usuario actual

            // Buscar el producto en la base de datos
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            // Obtener el carrito del usuario
            var carrito = db.Carritos.Include(c => c.CarritosProductos).FirstOrDefault(c => c.UserId == userId);

            if (carrito == null)
            {
                // Si no existe un carrito para el usuario, lo creamos
                carrito = new Carrito
                {
                    UserId = userId,
                    CarritosProductos = new List<CarritoProducto>()
                };
                db.Carritos.Add(carrito);
                db.SaveChanges(); // Guardar los cambios
            }

            // Verificar si el producto ya está en el carrito
            var carritoProducto = carrito.CarritosProductos.FirstOrDefault(cp => cp.CodigoProducto == producto.CodigoProducto);
            if (carritoProducto == null)
            {
                // Si el producto no está en el carrito, lo agregamos
                carrito.CarritosProductos.Add(new CarritoProducto
                {
                    Carrito = carrito,
                    Producto = producto
                });

                db.SaveChanges(); // Guardar los cambios en la base de datos
            }

            // Redirigir al catálogo (puedes cambiar esto según la lógica de tu aplicación)
            return RedirectToAction("Index");
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Obtener el producto junto con las reseñas asociadas
            Producto producto = db.Productos.Include("Resenas")
                                            .FirstOrDefault(p => p.CodigoProducto == id);

            if (producto == null)
            {
                return HttpNotFound();
            }

            // Ya no es necesario asignar reseñas a ViewBag, ya que están en el modelo
            return View(producto); // Pasar solo el producto, ya que las reseñas están en el modelo
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AgregarResena(int id, string TextoResena, int Calificacion)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Obtener el usuario actual
                string userId = User.Identity.GetUserId(); // Usar la identidad para obtener el usuario

                // Obtener el producto al que se va a agregar la reseña
                Producto producto = db.Productos.Find(id);
                if (producto == null)
                {
                    return HttpNotFound();
                }

                // Crear una nueva reseña
                var resena = new Resena
                {
                    TextoResena = TextoResena,
                    Calificacion = Calificacion,
                    UserId = userId, // Relacionamos la reseña con el usuario logueado
                    CodigoProducto = id,
                    Producto = producto // Relacionamos la reseña con el producto
                };

                // Guardar la reseña en la base de datos
                db.Resenas.Add(resena);
                db.SaveChanges();

                // Redirigir a la página de detalles para mostrar la reseña
                return RedirectToAction("Details", new { id = id });
            }

            return RedirectToAction("Login", "Account");
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
