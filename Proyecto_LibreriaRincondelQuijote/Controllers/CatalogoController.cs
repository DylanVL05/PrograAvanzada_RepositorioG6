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
    [Authorize]  
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
            ViewBag.MinPrecio = db.Productos.Min(p => p.Precio);
            ViewBag.MaxPrecio = db.Productos.Max(p => p.Precio);

            // Opciones de disponibilidad: 0: No disponible, 1: Disponible
            ViewBag.Disponibilidad = new List<int> { 0, 1 };

            // Ejecutamos la consulta y obtenemos la lista de productos filtrados
            var productosFiltrados = productos.ToList();

            return View(productosFiltrados);
        }



        
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
    








    // GET: Catalogo/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Catalogo/Create
        public ActionResult Create()
        {
            ViewBag.CodigoCategoria = new SelectList(db.Categorias, "CodigoCategoria", "Categoria_");
            ViewBag.CodigoEstado = new SelectList(db.Estados, "CodigoEstado", "Estado_actual");
            return View();
        }

        // POST: Catalogo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoProducto,Nombre,Precio,Disponibilidad,Imagen1,Imagen2,Imagen3,CodigoEstado,CodigoCategoria")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoCategoria = new SelectList(db.Categorias, "CodigoCategoria", "Categoria_", producto.CodigoCategoria);
            ViewBag.CodigoEstado = new SelectList(db.Estados, "CodigoEstado", "Estado_actual", producto.CodigoEstado);
            return View(producto);
        }

        // GET: Catalogo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoCategoria = new SelectList(db.Categorias, "CodigoCategoria", "Categoria_", producto.CodigoCategoria);
            ViewBag.CodigoEstado = new SelectList(db.Estados, "CodigoEstado", "Estado_actual", producto.CodigoEstado);
            return View(producto);
        }

        // POST: Catalogo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoProducto,Nombre,Precio,Disponibilidad,Imagen1,Imagen2,Imagen3,CodigoEstado,CodigoCategoria")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoCategoria = new SelectList(db.Categorias, "CodigoCategoria", "Categoria_", producto.CodigoCategoria);
            ViewBag.CodigoEstado = new SelectList(db.Estados, "CodigoEstado", "Estado_actual", producto.CodigoEstado);
            return View(producto);
        }

        // GET: Catalogo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Catalogo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
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
