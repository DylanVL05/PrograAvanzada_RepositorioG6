using Microsoft.AspNet.Identity;
using Proyecto_LibreriaRincondelQuijote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_LibreriaRincondelQuijote.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Método para obtener o crear el carrito del usuario
        public Carrito ObtenerCarrito()
        {
            // Obtener el UserId del usuario autenticado
            string userId = User.Identity.GetUserId();

            // Buscar si ya existe un carrito para el usuario
            var carrito = db.Carritos
                            .Include("CarritosProductos.Producto")  // Asegura que los productos asociados se carguen
                            .FirstOrDefault(c => c.UserId == userId);

            if (carrito == null)
            {
                // Si no existe, creamos un carrito nuevo
                carrito = new Carrito
                {
                    UserId = userId,
                    CarritosProductos = new List<CarritoProducto>()
                };
                db.Carritos.Add(carrito);
                db.SaveChanges(); // Guardamos el nuevo carrito en la base de datos
            }

            return carrito;
        }


        // Acción para agregar un producto al carrito
        public ActionResult AgregarAlCarrito(int productoId, int cantidad)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Buscar el producto en el carrito
            var productoEnCarrito = carrito.CarritosProductos
                                           .FirstOrDefault(cp => cp.CodigoProducto == productoId);

            var producto = db.Productos.Find(productoId);

            // Verificar si el producto existe
            if (producto == null)
            {
                return RedirectToAction("Error", new { mensaje = "Producto no encontrado" });
            }

            // Verificar si la cantidad solicitada no supera la disponibilidad del producto
            if (cantidad > producto.Disponibilidad)
            {
                TempData["Error"] = "No hay suficiente stock para completar la solicitud.";
                return RedirectToAction("VerCarrito");
            }

            if (productoEnCarrito == null)
            {
                // Si el producto no está en el carrito, lo agregamos
                carrito.CarritosProductos.Add(new CarritoProducto
                {
                    Carrito = carrito,
                    Producto = producto,
                    Cantidad = cantidad
                });
            }
            else
            {
                // Si el producto ya está en el carrito, actualizamos la cantidad
                productoEnCarrito.Cantidad += cantidad;
            }

            db.SaveChanges(); // Guardamos los cambios en la base de datos

            // Redirigir a la vista del carrito
            return RedirectToAction("VerCarrito");
        }
        [HttpPost]
        public ActionResult Comprar(FormCollection form)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Verificar si el carrito tiene productos
            if (carrito.CarritosProductos != null && carrito.CarritosProductos.Any())
            {
                // Lista para guardar los productos que deben ser eliminados
                var productosAEliminar = new List<Producto>();

                foreach (var item in carrito.CarritosProductos)
                {
                    var producto = item.Producto;

                    // Obtener la cantidad desde el formulario (asegúrate de que la vista pase la cantidad correcta)
                    int cantidad = int.Parse(form["Cantidad_" + item.CodigoCarrito]);

                    if (producto != null)
                    {
                        // Crear el registro en el historial de compras
                        for (int i = 0; i < cantidad; i++) // Crear tantos registros como la cantidad comprada
                        {
                            var historial = new HistorialCompra
                            {
                                UserId = carrito.UserId,
                                CodigoProducto = producto.CodigoProducto,
                                Producto = producto,
                                Precio = producto.Precio,
                                FechaCompra = DateTime.Now,
                                Cantidad = cantidad
                            };

                            db.HistorialCompras.Add(historial);
                        }

                        // Reducir la cantidad de disponibilidad del producto
                        if (producto.Disponibilidad >= cantidad)
                        {
                            producto.Disponibilidad -= cantidad; // Restamos la cantidad comprada

                            // Si la disponibilidad llega a cero, marcar el producto para eliminación
                            if (producto.Disponibilidad == 0)
                            {
                                productosAEliminar.Add(producto); // Agregar a la lista de productos para eliminar
                            }
                        }
                        else
                        {
                            // Si la disponibilidad es menor a la cantidad solicitada, manejar el error
                            TempData["Error"] = "No hay suficiente stock para completar la compra.";
                            return RedirectToAction("VerCarrito");
                        }
                    }
                }

                // Eliminar los productos marcados después de que se haya completado el foreach
                foreach (var producto in productosAEliminar)
                {
                    db.Productos.Remove(producto); // Eliminar los productos de la base de datos
                }

                // Eliminar el carrito después de la compra
                db.Carritos.Remove(carrito); // Eliminar el carrito
                db.SaveChanges(); // Guardar los cambios en la base de datos

                // Redirigir al usuario a la página de éxito de compra
                return RedirectToAction("CompraExitosa");
            }

            // Si el carrito está vacío, redirigir a la vista del carrito
            TempData["Error"] = "Tu carrito está vacío. No puedes proceder con la compra.";
            return RedirectToAction("VerCarrito");
        }


        public ActionResult CompraExitosa()
        {
            return View();
        }



        // Acción para eliminar un producto del carrito
        public ActionResult EliminarDelCarrito(int id)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Buscar el producto en el carrito utilizando el id del carritoProducto
            var productoEnCarrito = carrito.CarritosProductos.FirstOrDefault(cp => cp.CodigoCarrito == id);

            if (productoEnCarrito != null)
            {
                // Eliminar el producto del carrito
                carrito.CarritosProductos.Remove(productoEnCarrito);
                db.SaveChanges();  // Guardar los cambios en la base de datos
            }

            // Verificar si el carrito está vacío
            if (!carrito.CarritosProductos.Any())
            {
                // Si no hay productos en el carrito, eliminar el carrito de la base de datos
                db.Carritos.Remove(carrito);
                db.SaveChanges();  // Guardar los cambios para eliminar el carrito vacío
            }

            // Redirigir al carrito para que se actualice la vista
            return RedirectToAction("VerCarrito");
        }



        [HttpPost]
        public ActionResult ActualizarCantidad(int id, int cantidad)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Buscar el producto en el carrito
            var productoEnCarrito = carrito.CarritosProductos.FirstOrDefault(cp => cp.CodigoCarrito == id);
            var producto = db.Productos.Find(productoEnCarrito.CodigoProducto);

            if (producto != null)
            {
                // Verificar que la cantidad no exceda la disponibilidad
                if (cantidad > producto.Disponibilidad)
                {
                    TempData["Error"] = "No hay suficiente stock para completar la solicitud.";
                    return RedirectToAction("VerCarrito");
                }

                // Actualizar la cantidad
                productoEnCarrito.Cantidad = cantidad;
                db.SaveChanges();
            }

            return RedirectToAction("VerCarrito");
        }











        // Acción para ver el carrito
        public ActionResult VerCarrito()
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Devolver el carrito a la vista
            return View(carrito);
        }
    }
    }