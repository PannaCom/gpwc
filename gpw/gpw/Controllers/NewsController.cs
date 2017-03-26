using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using gpw.Models;
using System.IO;
using gpw.helpers;
using PagedList;
using PagedList.Mvc;
using System.Drawing;

namespace gpw.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private gpwEntities db = new gpwEntities();

        public string setNewHot(long? id)
        {
            try
            {
                string sql = "update news set isHot=1 where id=" + id;
                var updateNew = db.Database.ExecuteSqlCommand(sql);
                return "1";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
                return "0";
            }
        }

        //removeNewHot 
        public string removeNewHot(long? id)
        {
            try
            {
                string sql = "update news set isHot=0 where id=" + id;
                var updateNew = db.Database.ExecuteSqlCommand(sql);
                return "1";
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
                return "0";
            }
        }

        // GET: News
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

            var data = (from q in db.news select q);
            if (data == null)
            {
                return View(data);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                data = data.Where(x => x.new_title.Contains(search));
                ViewBag.search = search;
            }

            data = data.OrderByDescending(x => x.id);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        // GET: News/Details/5
        [AllowAnonymous]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.des = news.new_des;
            ViewBag.image = Config.domain + news.new_img;
            ViewBag.url = Config.domain + "/" + Config.unicodeToNoMark(news.new_title) + "-" + id;
            ViewBag.title = news.new_title;
           
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            ViewBag.danhmuctin = danhmuctin();
            ViewBag.trang_thai = new List<SelectListItem>() {
                new SelectListItem() { Value = "1", Text = "Công khai" },
                new SelectListItem() { Value = "2", Text = "Lưu Nháp" }
            };
            ViewBag.quyen_hang = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "1", Text = "Công khai" },
                new SelectListItem() { Value = "2", Text = "Chỉ thành viên trong nhóm" },
                new SelectListItem() { Value = "3", Text = "Bạn bè" },
                new SelectListItem() { Value = "4", Text = "Chỉ mình tôi" },
            };
            ViewBag.isHot = new List<SelectListItem>() { 
                new SelectListItem() { Value = "0", Text = "Tin thường" },
                new SelectListItem() { Value = "1", Text = "Tin nổi bật" }
            };

            return View();
        }

        public List<SelectListItem> danhmuctin()
        {
            var newList = db.cats.Select(x => new SelectListItem()
            {
                Value = x.id.ToString(),
                Text = x.cat_name
            }).ToList();
            return newList;
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(news news)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToAction("Create");
            }

            news baiviet = new news();
            baiviet.new_title = news.new_title ?? null;
            baiviet.new_flug = baiviet.new_title != null ? configs.unicodeToNoMark(baiviet.new_title) : null;
            baiviet.cat_id = news.cat_id ?? null;
            baiviet.isHot = news.isHot ?? null;
            baiviet.new_content = news.new_content ?? null;
            baiviet.new_img = news.new_img ?? null;
            baiviet.ngay_tao = DateTime.Now;
            baiviet.quyen_hang = news.quyen_hang ?? null;
            baiviet.trang_thai = news.trang_thai ?? null;
            baiviet.new_des = news.new_des ?? null;
            var userId = User.Identity.GetUserId();
            baiviet.user_id = userId;
            db.news.Add(baiviet);
            db.SaveChanges();
            TempData["update"] = "Thêm bài viết thành công";
            return RedirectToAction("Create");
            
        }

        // GET: News/Edit/5
        public ActionResult Edit(long? id)
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.danhmuctin = danhmuctin();
            ViewBag.trang_thai = new List<SelectListItem>() {
                new SelectListItem() { Value = "1", Text = "Công khai" },
                new SelectListItem() { Value = "2", Text = "Lưu Nháp" }
            };
            ViewBag.quyen_hang = new List<SelectListItem>()
            {
                new SelectListItem() { Value = "1", Text = "Công khai" },
                new SelectListItem() { Value = "2", Text = "Chỉ thành viên trong nhóm" },
                new SelectListItem() { Value = "3", Text = "Bạn bè" },
                new SelectListItem() { Value = "4", Text = "Chỉ mình tôi" },
            };
            ViewBag.isHot = new List<SelectListItem>() { 
                new SelectListItem() { Value = "0", Text = "Tin thường" },
                new SelectListItem() { Value = "1", Text = "Tin nổi bật" }
            };
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(news news)
        {
            
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Vui lòng kiểm tra lại các trường.";
                return RedirectToAction("Edit", new { id = news.id });
            }
            var baiviet = db.news.Find(news.id);
            
            baiviet.new_title = news.new_title ?? null;
            baiviet.new_flug = baiviet.new_title != null ? configs.unicodeToNoMark(baiviet.new_title) : null;
            baiviet.cat_id = news.cat_id ?? null;
            baiviet.isHot = news.isHot ?? null;
            baiviet.new_content = news.new_content ?? null;
            baiviet.new_img = news.new_img ?? null;
            //baiviet.ngay_tao = DateTime.Now;
            baiviet.quyen_hang = news.quyen_hang ?? null;
            baiviet.trang_thai = news.trang_thai ?? null;
            baiviet.new_des = news.new_des ?? null;
            var userId = User.Identity.GetUserId();
            baiviet.user_id = userId;            
            TempData["update"] = "Cập nhật bài viết thành công.";
            db.Entry(baiviet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = news.id });
        }

        // GET: News/Delete/5
        public ActionResult Delete(long? id)
        {
            if (configs.getCookie("admin") == null || configs.getCookie("admin") == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            news news = db.news.Find(id);
            db.news.Remove(news);
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

        public ActionResult uploadimg()
        {
            var fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}images\\news", Server.MapPath(@"\")));
                        string strDay = DateTime.Now.ToString("yyyyMM");
                        string pathString = System.IO.Path.Combine(originalDirectory.ToString(), strDay);

                        var _fileName = Guid.NewGuid().ToString("N") + ".jpg";

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, _fileName);
                        //System.Drawing.Image bm = System.Drawing.Image.FromStream(file.InputStream);
                        // Thay đổi kích thước ảnh
                        //bm = ResizeBitmap((Bitmap)bm, 100, 100); /// new width, height
                        //// Giảm dung lượng ảnh trước khi lưu
                        //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                        //ImageCodecInfo ici = null;
                        //foreach (ImageCodecInfo codec in codecs)
                        //{
                        //    if (codec.MimeType == "image/jpeg")
                        //        ici = codec;
                        //}
                        //EncoderParameters ep = new EncoderParameters();
                        //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)80);
                        //bm.Save(path, ici, ep);
                        //bm.Save(path);
                        file.SaveAs(path);
                        FileInfo f2 = new FileInfo(path);
                        if (f2.Length > 100000) {
                            int percent = 50;
                            if (f2.Length > 1000000) percent = 10;
                            else
                                if (f2.Length > 500000) percent = 20;
                                else if (f2.Length > 300000) percent = 30;
                            ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
                            iFF.Load(path).Quality(percent).Save(path);
                        }
                        fName = "/images/news/" + strDay + "/" + _fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
        }
        public string smooth()
        {

            var p = (from q in db.news select q).ToList();
            for (int i = 0; i < p.Count; i++)
            {
                string physicalPath = HttpContext.Server.MapPath("../Images/news\\");
                string nameFile = p[i].new_img.Replace("/images/news/", "");
                if (!System.IO.File.Exists(physicalPath + "/" + nameFile)) continue;

                //return resizeImage(Config.imgWidthProduct, Config.imgHeightProduct, physicalPath + nameFile, Config.ProductImagePath + "/" + nameFile);
                
                FileInfo f2 = new FileInfo(physicalPath + "/" + nameFile);
                if (f2.Length > 100000)
                {
                    int percent = 50;
                    if (f2.Length > 1000000) percent = 10;
                    else
                    if (f2.Length > 500000) percent = 20;
                    else if (f2.Length > 300000) percent = 30;
                    ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
                    //int depth = (int)(f2.Length / 90000);
                    //float need = (float)((f2.Length - (depth * 90000)) / f2.Length);
                    //float percent = 1 - need;
                    iFF.Load(physicalPath + "/" + nameFile).Quality(percent).Save(physicalPath + nameFile);
                }
                ////Tạo ra file thumbail không có watermark
                //Size size1 = new Size(255, 170);
                //iFF.Load(physicalPath + "/" + nameFile).Resize(size1).BackgroundColor(Color.WhiteSmoke).Save(physicalPath + "/" + nameFile);
                //iFF.Load(physicalPath + nameFile).co(Color.White).Resize(size1).Save(physicalPath + nameFile);
            }
            return "ok";
        }
        //public string resizeImage(string fullPath, string path, string path2)
        //{
        //    string physicalPath = fullPath;
        //    string nameFile = path;
        //    string nameFile2 = path2;
        //    ImageProcessor.ImageFactory iFF = new ImageProcessor.ImageFactory();
        //    ////Tạo ra file thumbail không có watermark
        //    Size size1 = new Size(255, 170);
        //    iFF.Load(physicalPath + "/" + nameFile).Resize(size1).BackgroundColor(Color.WhiteSmoke).Save(physicalPath + "/" + nameFile2);
        //    return "ok";

        //}
        [AllowAnonymous]
        public ActionResult tin(string url, int? pg)
        {
            if (url == null || url == "")
	        {
                 return RedirectToAction("Index", "Home");
	        }
            ViewBag.url = url;
            var cat = (from c in db.cats where c.cat_url == url select c).FirstOrDefault();
            if (cat == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.danhmuctin = cat.cat_name;

            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var data = (from q in db.news where q.cat_id == cat.id orderby q.id descending select q);
            if (data == null)
            {
                return View(data);
            }

            //data = data.OrderBy(x => x.id);

            return View(data.ToPagedList(pageNumber, pageSize));
        }

    }
}
