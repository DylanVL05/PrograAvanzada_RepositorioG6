using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_LibreriaRincondelQuijote.Models;

//Esta Historial Controller esta dirigido al administrador 

namespace Proyecto_LibreriaRincondelQuijote.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HistorialComprasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HistorialCompras
        public ActionResult Index()
        {
            var historialCompras = db.HistorialCompras.Include(h => h.Producto);
            return View(historialCompras.ToList());
        }

        // GET: HistorialCompras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialCompra historialCompra = db.HistorialCompras.Find(id);
            if (historialCompra == null)
            {
                return HttpNotFound();
            }
            return View(historialCompra);
        }

        // GET: HistorialCompras/Create
        public ActionResult Create()
        {
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre");
            return View();
        }

        // POST: HistorialCompras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoHistorial,UserId,CodigoProducto,Precio,FechaCompra,Cantidad")] HistorialCompra historialCompra)
        {
            if (ModelState.IsValid)
            {
                db.HistorialCompras.Add(historialCompra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", historialCompra.CodigoProducto);
            return View(historialCompra);
        }

        // GET: HistorialCompras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialCompra historialCompra = db.HistorialCompras.Find(id);
            if (historialCompra == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", historialCompra.CodigoProducto);
            return View(historialCompra);
        }

        // POST: HistorialCompras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoHistorial,UserId,CodigoProducto,Precio,FechaCompra,Cantidad")] HistorialCompra historialCompra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialCompra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", historialCompra.CodigoProducto);
            return View(historialCompra);
        }

        // GET: HistorialCompras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialCompra historialCompra = db.HistorialCompras.Find(id);
            if (historialCompra == null)
            {
                return HttpNotFound();
            }
            return View(historialCompra);
        }

        // POST: HistorialCompras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialCompra historialCompra = db.HistorialCompras.Find(id);
            db.HistorialCompras.Remove(historialCompra);
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
