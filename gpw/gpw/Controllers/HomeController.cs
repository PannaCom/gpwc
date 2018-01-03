using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gpw.Models;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Xml;
using System.Text;
using gpw.helpers;
namespace gpw.Controllers
{
    public class HomeController : Controller
    {
        private gpwEntities db = new gpwEntities();
        public ActionResult Index()
        {
            var hot1 = (from s in db.news where s.isHot == 1 orderby s.ngay_tao descending select s).Take(3).ToList();
            ViewBag.hot1 = hot1;
            var hot2 = (from s in db.news where s.isHot == 1 orderby s.ngay_tao descending select s).Skip(3).Take(3).ToList();
            ViewBag.hot2 = hot2;
            var model = (from s in db.user_news where s.status != null && s.status == 1 orderby s.date_time descending select s).Take(6).ToList();
            ViewBag.user_news = model;
            var model1 = db.news.Where(x => x.cat_id == 1).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            var model2 = db.news.Where(x => x.cat_id == 2).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            var model4 = db.news.Where(x => x.cat_id == 4).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            ViewBag.news1 = model1;
            ViewBag.news2 = model2;
            ViewBag.news4 = model4;
            return View();
        }
        public ActionResult Test()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        //[Authorize]
        public ActionResult Search(string keyword, int? pg)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            if (keyword == null) keyword = "";
            ViewBag.keyword = keyword;
            string sql = "select * from news where new_title like N'%" + keyword + "%' or new_des like N'%" + keyword + "%'";

            //if (keyword != null && keyword != "")
            //{
            //    ViewBag.keyword = keyword;
            //    sql += " where t1.Email like '%" + keyword + "%'" + " or t1.PhoneNumber like '%" + keyword + "%'";
            //}

            var data = db.Database.SqlQuery<news>(sql).ToList();

            return View(data.ToPagedList(pageNumber, pageSize));
        }

        //[Authorize]
        //public ActionResult Profile(string id)        
        //{
        //    var profile = db.thong_tin_user.Where(x => x.user_id == id).Select(x=>x).FirstOrDefault();
        //    return View(profile);
        //}

        public ActionResult addfriend(string id)
        {
            
            ViewBag.userId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addfriend(string userId, int quanhe)
        {
            int dosau = 0;
            if (quanhe == 1) { dosau = 4; }
            if (quanhe == 2) { dosau = 3; }
            if (quanhe == 3 || quanhe == 4 || quanhe == 5) { dosau = 2; }
            if (quanhe == 6 || quanhe == 7) { dosau = 1; }

            var userId2= User.Identity.GetUserId();

            friend NewFriend = new friend();
            NewFriend.user1 = userId2;
            NewFriend.user2 = userId;
            NewFriend.quan_he_id = quanhe;
            NewFriend.do_sau = dosau;
            db.friends.Add(NewFriend);
            db.SaveChanges();


            return RedirectToAction("profile");
        }

        public ActionResult LoadNewHot()
        {
            var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x=>x.ngay_tao).Take(10).ToList();
            return PartialView("_LoadNewHot", model);
        }

        public ActionResult LoadNewHot2()
        {
            var model = (from s in db.news where s.isHot == 1 orderby s.ngay_tao descending select s).Take(3).ToList();
            //var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x => x.id).Select(x => x).Take(7).ToList();
            return PartialView("_LoadNewHot2", model);
        }

        public ActionResult LoadNewTopHot2()
        {
            var model = (from s in db.news where s.isHot == 1 orderby s.ngay_tao descending select s).Skip(3).Take(3).ToList();

            //var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x => x.id).Select(x => x).Skip(7).Take(3).ToList();
            return PartialView("_LoadNewTopHot2", model);
        }
        public ActionResult LoadUserPost()
        {
            var model = (from s in db.user_news where s.status!=null && s.status==1 orderby s.date_time descending select s).Take(6).ToList();

            //var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x => x.id).Select(x => x).Skip(7).Take(3).ToList();
            return PartialView("_LoadUserPost", model);
        }
        public ActionResult LoadNewCat()
        {
            var model = from c in db.cats select c;
            return PartialView("_LoadNewCat", model.ToList());
        }

        public ActionResult LoadNewCat2()
        {
            var model = from c in db.cats select c;
            return PartialView("_LoadNewCat2", model.ToList());
        }
        public ActionResult Cat1()
        {
            var model = db.news.Where(x => x.cat_id == 1).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            return PartialView("Cat1", model.ToList());
        }
        public ActionResult Cat2()
        {
            var model = db.news.Where(x => x.cat_id == 2).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            return PartialView("Cat2", model.ToList());
        }
        public ActionResult Cat4()
        {
            var model = db.news.Where(x => x.cat_id == 4).OrderByDescending(x => x.ngay_tao).Take(8).ToList();
            return PartialView("Cat4", model.ToList());
        }
        public ActionResult LoadNewInCat(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.ngay_tao).Take(8).ToList();// && x.isHot == 0
            return PartialView("_LoadNewInCat", model.ToList());
        }

        public ActionResult LoadNewInCat2(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.ngay_tao).Skip(1).Take(8).ToList();//Select(x => x).
            return PartialView("_LoadNewInCat2", model.ToList());// && x.isHot == 0
        }

        public ActionResult LoadNewInCat_top(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.ngay_tao).First();// && x.isHot == 0//Select(x => x).
            return PartialView("_LoadNewInCat_top", model);
        }

        //public ActionResult DsUser()
        //{
        //    var model = db.thong_tin_user.Select(x => x).ToList();
        //    return PartialView("_DsUser", model);
        //}

        public ActionResult GiaPha()
        {
            return View();
        }
        public ActionResult PhanMemGiaPha(string dir)
        {
            if (dir == null || dir == "") dir = "t2b";
            ViewBag.dir = dir;
            ViewBag.des = "Phần mềm gia phả, cây gia phả, sơ đồ phả hệ, phả đồ";
            ViewBag.image = "http://vietgiapha.com/images/logo.png";
            ViewBag.url = Config.domain + "/Home/PhanMemGiaPha";
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult ThietKeWebsite()
        {
            return View();
        }
        public ActionResult HuongDan()
        {
            return View();
        }
        public string generateSiteMap()
        {

            try
            {

                XmlWriterSettings settings = null;
                string xmlDoc = null;

                settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = Encoding.UTF8;
                xmlDoc = HttpRuntime.AppDomainAppPath + "sitemap.xml";//HttpContext.Server.MapPath("../") + 
                //float percent = 0.85f;

                string urllink = "";
                using (XmlTextWriter writer = new XmlTextWriter(xmlDoc, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com");
                    writer.WriteElementString("changefreq", "always");
                    writer.WriteElementString("priority", "1");
                    writer.WriteEndElement();
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/thiet-ke-gia-pha");
                    writer.WriteElementString("changefreq", "weekly");
                    writer.WriteElementString("priority", "0.99");
                    writer.WriteEndElement();
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/thiet-ke-website-gia-pha-dong-ho");
                    writer.WriteElementString("changefreq", "weekly");
                    writer.WriteElementString("priority", "0.99");
                    writer.WriteEndElement();
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/phan-mem-gia-pha");
                    writer.WriteElementString("changefreq", "weekly");
                    writer.WriteElementString("priority", "0.99");
                    writer.WriteEndElement();
                    //writer.WriteStartElement("url");
                    //writer.WriteElementString("loc", "http://vietgiapha.com/gioi-thieu");
                    //writer.WriteElementString("changefreq", "always");
                    //writer.WriteElementString("priority", "0.99");
                    //writer.WriteEndElement();
                    //writer.WriteStartElement("url");
                    //writer.WriteElementString("loc", "http://vietgiapha.com/ThanhVien/Create");
                    //writer.WriteElementString("changefreq", "always");
                    //writer.WriteElementString("priority", "0.99");
                    //writer.WriteEndElement();
                    //tài xế

                    //Xe Buýt
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/tin/gioi-thieu");
                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/tin/doanh-nghiep-dong-ho");
                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();

                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", "http://vietgiapha.com/tin/gia-pha");
                    writer.WriteElementString("changefreq", "daily");
                    writer.WriteElementString("priority", "0.97");
                    writer.WriteEndElement();
                    var p4 = (from q in db.news select q).OrderByDescending(o => o.ngay_tao).ToList();
                    for (int i = 0; i < p4.Count; i++)
                    {
                        try
                        {

                            writer.WriteStartElement("url");
                            urllink = "http://vietgiapha.com/" + configs.unicodeToNoMark(p4[i].new_title) + "-" + p4[i].id;
                            writer.WriteElementString("loc", urllink);
                            writer.WriteElementString("changefreq", "weekly");
                            //percent = 0.70f;
                            writer.WriteElementString("priority", "0.96");
                            writer.WriteEndElement();
                        }
                        catch (Exception ex4)
                        {
                        }
                    }
                   
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

            }
            catch (Exception extry)
            {
                //StreamWriter sw = new StreamWriter();
            }
            return "ok";
        }

        public ActionResult LoadDoanhNghiep()
        {
            var model = db.businesses.Select(x => x).ToList();
            return PartialView("_LoadDoanhNghiep", model);
        }
        public ActionResult LoadThanhVien()
        {
            var model = db.thanh_vien.OrderByDescending(o => o.ngay_tao).Take(4).ToList();//Select(x => x)
            return PartialView("_LoadThanhVien", model);
        }
        [HttpPost]
        public string sendmail(string phone, string name, string email, string notice)
        {
            try
            {
                //customer ct = new customer();
                //ct.date_time = DateTime.Now;
                //ct.name = name;
                //ct.phone = phone;
                //ct.email = email;
                //db.customers.Add(ct);
                //db.SaveChanges();
                string mailuser = "mistermynz@gmail.com";
                string mailpass = "13062015";
                if (Config.Sendmail(mailuser, mailpass, "vnnvh80@gmail.com", "Thiết kế gia phả " + name + ", điện thoại " + phone, "Khách hàng: " + name + ", Điện thoại: " + phone + ", Email: " + email + "<br>" + notice))
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch
            {
                return "0";
            }
        }
    }
}