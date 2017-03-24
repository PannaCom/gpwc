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
    
    public class BusinessController : Controller
    {
        private gpwEntities db = new gpwEntities();

        // GET: Business
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

            var data = (from q in db.businesses select q);
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


        [HttpPost]
        public ActionResult savebusiness(HttpPostedFileBase _file, int id, string name, string website)
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
                var originalDirectory = new DirectoryInfo(string.Format("{0}images\\busi", Server.MapPath(@"\")));
                string strDay = DateTime.Now.ToString("yyyyMM");
                string pathString = System.IO.Path.Combine(originalDirectory.ToString());
                var _fileName = Guid.NewGuid().ToString("N") + ".jpg";
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _file.SaveAs(path);
                fName = "/images/busi/" + _fileName;
            }

            if (id == 0)
            {
                business newbus = new business();
                newbus.name = name ?? null;
                newbus.website = website ?? null;
                newbus.image = fName ?? null;
                db.businesses.Add(newbus);
                db.SaveChanges();
                data = newbus;
            }
            else
            {
                var editbus = db.businesses.Find(id);
                if (editbus != null)
                {
                    editbus.name = name ?? null;
                    editbus.website = website ?? null;
                    if (fName != null)
                    {
                        editbus.image = fName;
                    }
                    db.Entry(editbus).State = EntityState.Modified;
                    db.SaveChanges();
                    data = editbus;
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult editbusiness(int id)
        {
            var data = new object() { };
            var editbus = db.businesses.Find(id);
            if (editbus != null)
            {
                data = editbus;
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult deletebusiness(int id)
        {
            string delete = "0";
            var deletebus = db.businesses.Find(id);
            if (deletebus != null)
            {
                db.businesses.Remove(deletebus);
                db.SaveChanges();
                delete = "1";
            }
            return Json(delete, JsonRequestBehavior.AllowGet);
        }

        // GET: Business/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            business business = db.businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // GET: Business/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Business/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,website,image")] business business)
        {
            if (ModelState.IsValid)
            {
                db.businesses.Add(business);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(business);
        }

        // GET: Business/Edit/5
        public ActionResult Edit(int? id)
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            business business = db.businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // POST: Business/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,website,image")] business business)
        {
            if (ModelState.IsValid)
            {
                db.Entry(business).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(business);
        }

        // GET: Business/Delete/5
        public ActionResult Delete(int? id)
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            business business = db.businesses.Find(id);
            if (business == null)
            {
                return HttpNotFound();
            }
            return View(business);
        }

        // POST: Business/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            business business = db.businesses.Find(id);
            db.businesses.Remove(business);
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
