using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_LibreriaRincondelQuijote.Models;

namespace Proyecto_LibreriaRincondelQuijote.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ResenasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resenas
        public ActionResult Index()
        {
            var resenas = db.Resenas.Include(r => r.Producto);
            return View(resenas.ToList());
        }

        // GET: Resenas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resena resena = db.Resenas.Find(id);
            if (resena == null)
            {
                return HttpNotFound();
            }
            return View(resena);
        }

        // GET: Resenas/Create
        public ActionResult Create()
        {
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre");
            return View();
        }

        // POST: Resenas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoResena,TextoResena,Calificacion,UserId,CodigoProducto")] Resena resena)
        {
            if (ModelState.IsValid)
            {
                db.Resenas.Add(resena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", resena.CodigoProducto);
            return View(resena);
        }

        // GET: Resenas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resena resena = db.Resenas.Find(id);
            if (resena == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", resena.CodigoProducto);
            return View(resena);
        }

        // POST: Resenas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoResena,TextoResena,Calificacion,UserId,CodigoProducto")] Resena resena)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resena).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodigoProducto = new SelectList(db.Productos, "CodigoProducto", "Nombre", resena.CodigoProducto);
            return View(resena);
        }

        // GET: Resenas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resena resena = db.Resenas.Find(id);
            if (resena == null)
            {
                return HttpNotFound();
            }
            return View(resena);
        }

        // POST: Resenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resena resena = db.Resenas.Find(id);
            db.Resenas.Remove(resena);
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
