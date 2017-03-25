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
using ImageProcessor.Web;
using ImageProcessor.Processors;
using ImageProcessor.Imaging;
using System.Drawing;
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
                string basicUID = Guid.NewGuid().ToString("N");
                var _fileName = basicUID + ".jpg";
                var _fileName2 = basicUID + "_2.jpg";
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, _fileName);

                _file.SaveAs(path);
                fName = "/images/gallery/" + _fileName;
                resizeImage(pathString,_fileName, _fileName2);
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
        public string smooth()
        {
            
            var p = (from q in db.galleries select q).ToList();
            for (int i = 0; i < p.Count; i++)
            {
                string physicalPath = HttpContext.Server.MapPath("../Images/Gallery\\");
                string nameFile = p[i].image.Replace("/images/gallery/", "");
                string nameFile2 = nameFile.Replace(".jpg", "_2.jpg");
                if (!System.IO.File.Exists(physicalPath +"/"+ nameFile)) continue;
                
                //return resizeImage(Config.imgWidthProduct, Config.imgHeightProduct, physicalPath + nameFile, Config.ProductImagePath + "/" + nameFile);
                ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
                ////Tạo ra file thumbail không có watermark
                Size size1 = new Size(255, 170);
                iFF.Load(physicalPath + "/" + nameFile).Resize(size1).BackgroundColor(Color.WhiteSmoke).Save(physicalPath + "/" + nameFile2);
                //iFF.Load(physicalPath + nameFile).co(Color.White).Resize(size1).Save(physicalPath + nameFile);
            }
            return "ok";
        }
        public string resizeImage(string fullPath, string path,string path2)
        {
            string physicalPath = fullPath;
            string nameFile = path;
            string nameFile2 = path2;
            ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
            ////Tạo ra file thumbail không có watermark
            Size size1 = new Size(255, 170);
            iFF.Load(physicalPath + "/" + nameFile).Resize(size1).BackgroundColor(Color.WhiteSmoke).Save(physicalPath + "/" + nameFile2);
            return "ok";

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
            ViewBag.des = "Thiết kế gia phả, phả đồ, phả hệ theo yêu cầu riêng của mỗi dòng họ, dịch vụ uy tín, chuyên nghiệp, giúp khách hàng nắm bắt những thông tin về lịch sử dòng họ của mình.";
            ViewBag.image = "http://vietgiapha.com/images/gallery/049e5884e9f741efbcfdbac8a077063e_2.jpg";
            ViewBag.url = Config.domain + "/thiet-ke-gia-pha";
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
