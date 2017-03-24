using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gpw.Models;

namespace gpw.Controllers
{
    public class galleryController : Controller
    {
        private gpwEntities db = new gpwEntities();

        // GET: gallery
        public ActionResult Index()
        {
            return View(db.galleries.ToList());
        }

        // GET: gallery/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallery gallery = db.galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: gallery/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: gallery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,image")] gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.galleries.Add(gallery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gallery);
        }

        // GET: gallery/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallery gallery = db.galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: gallery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,image")] gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: gallery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gallery gallery = db.galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            gallery gallery = db.galleries.Find(id);
            db.galleries.Remove(gallery);
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
