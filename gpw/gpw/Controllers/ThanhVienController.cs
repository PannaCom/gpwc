using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gpw.Models;
using gpw.helpers;
using System.IO;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;

namespace gpw.Controllers
{
    public class ThanhVienController : Controller
    {
        private gpwEntities db = new gpwEntities();

        // GET: ThanhVien
        public ActionResult Index()
        {
            return View(db.thanh_vien.ToList());
        }

        // GET: ThanhVien/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            thanh_vien thanh_vien = db.thanh_vien.Find(id);
            if (thanh_vien == null)
            {
                return HttpNotFound();
            }
            return View(thanh_vien);
        }

        // GET: ThanhVien/Create
        public ActionResult Create()
        {
            if (configs.getCookie("thanhvien_id") != "") return RedirectToAction("Details", new { id = configs.getCookie("thanhvien_id") });
            ViewBag.TrinhDo = new List<SelectListItem>() { 
                new SelectListItem() { Value = "Đại học", Text = "Đại học" },
                new SelectListItem() { Value = "Cao đẳng", Text = "Cao đẳng" },
                new SelectListItem() { Value = "Trung cấp chuyên nghiệp", Text = "Trung cấp chuyên nghiệp" },
                new SelectListItem() { Value = "Trung học phổ thông", Text = "Trung học phổ thông" },
                new SelectListItem() { Value = "Trung học cơ sở", Text = "Trung học cơ sở" },
                new SelectListItem() { Value = "Tiểu học", Text = "Tiểu học" }
            };
            return View();
        }

        public ActionResult CheckEmailExist(string email)
        {
            string result = "0";
            if (email == null) email = "";
            var email1 = db.thanh_vien.Any(x => x.email == email);
            if (email1 == true)
            {
                result = "1";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: ThanhVien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(thanh_vien_model model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
            try
            {
                thanh_vien _addNew = new thanh_vien();
                _addNew.ho_ten = model.ho_ten ?? null;
                _addNew.dia_chi = model.dia_chi ?? null;
                _addNew.lon = model.lon ?? null;
                _addNew.lat = model.lat ?? null;
                _addNew.so_cmt = model.so_cmt ?? null;
                _addNew.nghe_nghiep = model.nghe_nghiep ?? null;
                _addNew.hoc_van = model.hoc_van ?? null;
                _addNew.hinh_anh = model.hinh_anh ?? null;
                _addNew.biet_danh = model.biet_danh ?? null;
                _addNew.gioi_tinh = model.gioi_tinh ?? null;
                string dateTime = model.ngay_sinh != null ? model.ngay_sinh : null;
                DateTime? dt = new DateTime();
                if (dateTime != null)
                {
                    dt = Convert.ToDateTime(dateTime);
                }
                _addNew.ngay_sinh = model.ngay_sinh != null ? dt : null;
                _addNew.ngay_tao = DateTime.Now;
                _addNew.cq_ctac = model.cq_ctac ?? null;
                _addNew.email = model.email ?? null;
                var passHash = model.pass != null ? configs.Encrypt(model.pass) : null;
                _addNew.pass = passHash;
                _addNew.trinh_do = model.trinh_do ?? null;
                _addNew.so_dien_thoai = model.so_dien_thoai ?? null;
                db.thanh_vien.Add(_addNew);
                db.SaveChanges();
                // set cookie đăng nhập 
                configs.setCookie("thanhvien_id", _addNew.id.ToString());
                configs.setCookie("ten_thanh_vien", _addNew.ho_ten);

                return RedirectToAction("TaoGiaPha", new { id = _addNew.id });
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
        }

        public ActionResult DangXuat()
        {
            configs.RemoveCookie("thanhvien_id");
            configs.RemoveCookie("ten_thanh_vien");
            return RedirectToAction("Index", "Home");
        }

        // GET: ThanhVien/Edit/5
        public ActionResult Edit(long? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.TrinhDo = new List<SelectListItem>() { 
                new SelectListItem() { Value = "Đại học", Text = "Đại học" },
                new SelectListItem() { Value = "Cao đẳng", Text = "Cao đẳng" },
                new SelectListItem() { Value = "Trung cấp chuyên nghiệp", Text = "Trung cấp chuyên nghiệp" },
                new SelectListItem() { Value = "Trung học phổ thông", Text = "Trung học phổ thông" },
                new SelectListItem() { Value = "Trung học cơ sở", Text = "Trung học cơ sở" },
                new SelectListItem() { Value = "Tiểu học", Text = "Tiểu học" }
            };

            thanh_vien thanh_vien = db.thanh_vien.Find(id);
            if (thanh_vien == null)
            {
                return HttpNotFound();
            }
            var thanhvien = new thanh_vien_model()
            {
                id = thanh_vien.id,
                biet_danh = thanh_vien.biet_danh,
                cq_ctac = thanh_vien.cq_ctac,
                dia_chi = thanh_vien.dia_chi,
                email = thanh_vien.email,
                gioi_tinh = thanh_vien.gioi_tinh,
                hinh_anh = thanh_vien.hinh_anh,
                ho_ten = thanh_vien.ho_ten,
                hoc_van = thanh_vien.hoc_van,
                lat = thanh_vien.lat,
                lon = thanh_vien.lon,
                ngay_sinh = string.Format("{0:MM/dd/yyyy}", thanh_vien.ngay_sinh),
                nghe_nghiep = thanh_vien.nghe_nghiep,
                so_cmt = thanh_vien.so_cmt,
                so_dien_thoai = thanh_vien.so_dien_thoai,
                trinh_do = thanh_vien.trinh_do
            };
            return View(thanhvien);
        }

        // POST: ThanhVien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,ho_ten,dia_chi,so_cmt,nghe_nghiep,hoc_van,hinh_anh,biet_danh,doi_thu,gioi_tinh,ngay_sinh,ngay_tao,trang_thai,quyen_han,lon,lat,cq_ctac,email,pass")] thanh_vien thanh_vien)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(thanh_vien).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(thanh_vien);
        //}

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(thanh_vien_model model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
            try
            {
                var _addNew = db.thanh_vien.Find(model.id);
                if (_addNew == null)
                {
                    return View(model);
                }
                _addNew.ho_ten = model.ho_ten ?? null;
                _addNew.dia_chi = model.dia_chi ?? null;
                _addNew.lon = model.lon ?? null;
                _addNew.lat = model.lat ?? null;
                _addNew.so_cmt = model.so_cmt ?? null;
                _addNew.nghe_nghiep = model.nghe_nghiep ?? null;
                _addNew.hoc_van = model.hoc_van ?? null;
                _addNew.hinh_anh = model.hinh_anh ?? null;
                _addNew.biet_danh = model.biet_danh ?? null;
                _addNew.gioi_tinh = model.gioi_tinh ?? null;
                string dateTime = model.ngay_sinh != null ? model.ngay_sinh : null;
                DateTime? dt = new DateTime();
                if (dateTime != null)
                {
                    dt = Convert.ToDateTime(dateTime);
                }
                _addNew.ngay_sinh = model.ngay_sinh != null ? dt : null;
                _addNew.ngay_tao = DateTime.Now;
                _addNew.cq_ctac = model.cq_ctac ?? null;
                _addNew.email = model.email ?? null;
                var passHash = model.pass != null ? configs.Encrypt(model.pass) : null;
                _addNew.pass = passHash;
                _addNew.trinh_do = model.trinh_do ?? null;
                _addNew.so_dien_thoai = model.so_dien_thoai ?? null;
                db.Entry(_addNew).State = EntityState.Modified;
                db.SaveChanges();
                // set cookie đăng nhập 
                configs.setCookie("thanhvien_id", _addNew.id.ToString());
                configs.setCookie("ten_thanh_vien", _addNew.ho_ten);

                return RedirectToAction("Edit", new { id = _addNew.id });
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
        }

        // GET: ThanhVien/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            thanh_vien thanh_vien = db.thanh_vien.Find(id);
            if (thanh_vien == null)
            {
                return HttpNotFound();
            }
            return View(thanh_vien);
        }

        // POST: ThanhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            thanh_vien thanh_vien = db.thanh_vien.Find(id);
            db.thanh_vien.Remove(thanh_vien);
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

        public string getHocVan(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.hoc_van where q.ten_truong.Contains(keyword) orderby q.ten_truong ascending select q.ten_truong).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }


        public string getNgheNghiep(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.nghe_nghiep where q.ten_nghe_nghiep.Contains(keyword) orderby q.ten_nghe_nghiep ascending select q.ten_nghe_nghiep).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        public string getThanhVien(string keyword)
        {
            if (keyword == null) keyword = "";
            var id = configs.getCookie("thanhvien_id");
            long _id = Convert.ToInt32(id);
            var p = (from q in db.thanh_vien where q.id != _id && q.ho_ten.Contains(keyword) orderby q.ho_ten ascending select new { label = q.ho_ten, value = q.id }).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        [HttpPost]
        public ActionResult saveTaoGiaPha(quan_he_thanh_vien model)
        {
            string data = "";
            // chưa có thì thêm mới
            var dathem = db.quan_he_thanh_vien.Where(x => x.thanh_vien_qh_id == model.thanh_vien_qh_id && x.thanh_vien_id == model.thanh_vien_id).FirstOrDefault();
            if (dathem != null)
            {
                data = "1";
            }
            else
            {
                try
                {
                    quan_he_thanh_vien _newmodel = new quan_he_thanh_vien();
                    _newmodel.thanh_vien_id = model.thanh_vien_id ?? null;
                    _newmodel.ten_quan_he = model.ten_quan_he ?? null;
                    _newmodel.ten_thanh_vien = model.ten_thanh_vien ?? null;
                    _newmodel.que_quan = model.que_quan ?? null;
                    _newmodel.qq_lon = model.qq_lon ?? null;
                    _newmodel.qq_lat = model.qq_lat ?? null;
                    _newmodel.thanh_vien_qh_id = model.thanh_vien_qh_id ?? null;
                    db.quan_he_thanh_vien.Add(_newmodel);
                    db.SaveChanges();
                    data = "<tr id=\"t_vien_" + _newmodel.id + "\"><td>" + _newmodel.ten_thanh_vien + "</td><td>" + _newmodel.ten_quan_he + "</td><td>" + _newmodel.que_quan + "</td><td><span class=\"btn btn-danger\" onclick=\"xoaquanhe(" + _newmodel.id + ");\">Xóa quan hệ</span></td></tr>";
                    
                }
                catch (Exception ex)
                {
                    configs.SaveTolog(ex.ToString());
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
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
                        var originalDirectory = new DirectoryInfo(string.Format("{0}images\\users", Server.MapPath(@"\")));
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
                        fName = "/images/users/" + strDay + "/" + _fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            if (configs.getCookie("thanhvien_id") != "") return RedirectToRoute("TaoGiaPha", new { id = configs.getCookie("thanhvien_id") });
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Login(thanh_vien_login_model model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại các trường.");
                return View(model);
            }
            long? _id = 0;
            try
            {
                string passHash = configs.Encrypt(model.Password);
                var thanhvienlogin = db.thanh_vien.Where(x => x.email == model.Email && x.pass == passHash).FirstOrDefault();
                if (thanhvienlogin == null)
                {
                    ModelState.AddModelError("", "Tài khoản e-mail hoặc mật khẩu không đúng.");
                    return View(model);
                }
                _id = thanhvienlogin.id;
                configs.setCookie("thanhvien_id", thanhvienlogin.id.ToString());
                configs.setCookie("ten_thanh_vien", thanhvienlogin.ho_ten);
            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }
            return RedirectToAction("TaoGiaPha", new { id = _id });
        }

        public ActionResult TaoGiaPha(long? id, int? pg)
        {
            if (configs.getCookie("thanhvien_id") == "") return RedirectToAction("Login");
            if (id == null || id == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var thanhvien = db.thanh_vien.Find(id);
            if (thanhvien == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.thanh_vien_id = thanhvien.id;
            ViewBag.ten_thanh_vien = thanhvien.ho_ten;


            ViewBag.driverId = id;
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;

            var quan_he = db.quan_he_thanh_vien.Where(x => x.thanh_vien_id == id).Select(x => x).ToList();

            return View(quan_he.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult LoadThanhVien(long? id)
        {
            var quan_he = db.quan_he_thanh_vien.Where(x => x.thanh_vien_id == id).Select(x => x).ToList();
            return PartialView("_LoadThanhVien", quan_he);
        }

        public ActionResult LoadThanhVienCungQH(string tqh, string ttv, long? id)
        {
            var ds = db.quan_he_thanh_vien.Where(x => x.ten_quan_he == tqh && x.ten_thanh_vien != ttv && x.thanh_vien_id == id).Select(x => x).ToList();
            //var ds2 = new List<thanh_vien>();
            //foreach (var item in ds)
            //{
            //    var data = db.thanh_vien.Find(item);
            //    if (data != null)
            //    {
            //        ds2.Add(data);
            //    }
            //}
            return PartialView("_LoadThanhVienCungQH", ds);
        }

        [HttpPost]
        public ActionResult xoaquanhe(long? id)
        {
            string data = "";
            try
            {
                quan_he_thanh_vien xoaquanhe = db.quan_he_thanh_vien.Find(id);
                if (xoaquanhe != null)
                {
                    db.quan_he_thanh_vien.Remove(xoaquanhe);
                    db.SaveChanges();
                    data = "1";
                }

            }
            catch (Exception ex)
            {
                configs.SaveTolog(ex.ToString());
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
