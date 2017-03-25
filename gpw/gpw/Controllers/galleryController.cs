using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gpw.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using gpw.helpers;
namespace gpw.Controllers
{
    public class galleryController : Controller
    {
        private gpwEntities db = new gpwEntities();

        // GET: gallery
        [Authorize]
        public ActionResult Index(int? pg, string search)
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = (from q in db.galleries select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.name.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderByDescending(x => x.id);

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
        }

		[Authorize]
        [HttpPost]
        public ActionResult savegallery(HttpPostedFileBase _file, int id, string name)
        {
            var data = new object() { };
            string fName = null;
            if (Request.Files.Count > 0)
            {
                foreach (string file in Request.Files)
                {
                    _file = Request.Files[file];
                }
            }
            //Save file content goes here
            if (_file != null && _file.ContentLength > 0)
            {
                var originalDirectory = new DirectoryInfo(string.Format("{0}images\\gallery", Server.MapPath(@"\")));
                string pathString = System.IO.Path.Combine(originalDirectory.ToString());
                var _fileName = Guid.NewGuid().ToString("N") + ".jpg";
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _file.SaveAs(path);
                fName = "/images/gallery/" + _fileName;
            }

            if (id == 0)
            {
                gallery newgallery = new gallery();
                newgallery.name = name ?? null;
                newgallery.image = fName ?? null;
                db.galleries.Add(newgallery);
                db.SaveChanges();
                data = newgallery;
            }
            else
            {
                var editgalery = db.galleries.Find(id);
                if (editgalery != null)
                {
                    editgalery.name = name ?? null;
                    if (fName != null)
                    {
                        editgalery.image = fName;
                    }
                    db.Entry(editgalery).State = EntityState.Modified;
                    db.SaveChanges();
                    data = editgalery;
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult editgallery(int id)
        {
            var data = new object() { };
            var editga = db.galleries.Find(id);
            if (editga != null)
            {
                data = editga;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult deletegallery(int id)
        {
            string delete = "0";
            var deletega = db.galleries.Find(id);
            if (deletega != null)
            {
                db.galleries.Remove(deletega);
                db.SaveChanges();
                delete = "1";
            }
            return Json(delete, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult List(int? pg)
        {
            int pageSize = 20;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = (from q in db.galleries select q);
            if (data == null)
            {
                return View(data);
            }
            
            data = data.OrderByDescending(x => x.id);

            return View(data.ToList().ToPagedList(pageNumber, pageSize));
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
