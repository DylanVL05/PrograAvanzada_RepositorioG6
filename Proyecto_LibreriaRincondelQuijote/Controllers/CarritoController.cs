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
        public ActionResult AgregarAlCarrito(int productoId)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Verificar si el producto ya está en el carrito
            var productoEnCarrito = carrito.CarritosProductos
                                           .FirstOrDefault(cp => cp.CodigoProducto == productoId);

            if (productoEnCarrito == null)
            {
                // Si el producto no está en el carrito, lo agregamos
                var producto = db.Productos.Find(productoId);
                if (producto != null)
                {
                    // Agregar el producto al carrito
                    carrito.CarritosProductos.Add(new CarritoProducto
                    {
                        Carrito = carrito,
                        Producto = producto
                    });

                    db.SaveChanges(); // Guardamos los cambios en la base de datos
                }
            }

            return RedirectToAction("VerCarrito");
        }

        [HttpPost]
        public ActionResult ProcederAlPago(FormCollection form)
        {
            // Obtener el carrito del usuario
            var carrito = ObtenerCarrito();

            // Verificar si el carrito tiene productos
            if (carrito.CarritosProductos != null && carrito.CarritosProductos.Any())
            {
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
                                FechaCompra = DateTime.Now
                            };

                            db.HistorialCompras.Add(historial);
                        }

                        // Reducir la cantidad de disponibilidad del producto
                        if (producto.Disponibilidad >= cantidad)
                        {
                            producto.Disponibilidad -= cantidad; // Restamos la cantidad comprada
                        }
                        else
                        {
                            // Si la disponibilidad es menor a la cantidad solicitada, manejar el error
                            return RedirectToAction("Error", new { mensaje = "No hay suficiente stock para completar la compra" });
                        }
                    }
                }

                // Eliminar los productos del carrito
                db.Carritos.Remove(carrito);

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                // Redirigir al usuario a la página de éxito de compra
                return RedirectToAction("CompraExitosa");
            }

            // Si el carrito está vacío, redirigir a la vista del carrito
            return RedirectToAction("VerCarrito");
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