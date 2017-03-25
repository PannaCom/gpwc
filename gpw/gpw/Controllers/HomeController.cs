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

        [Authorize]
        public ActionResult Search(string keyword, int? pg)
        {
            int pageSize = 25;
            if (pg == null) pg = 1;
            int pageNumber = (pg ?? 1);
            ViewBag.pg = pg;
            if (keyword == null) keyword = "";
            
            string sql = "SELECT t1.Id as id, ho_ten as ho_ten, dia_chi as dia_chi, hinh_anh as hinh_anh, Email as email, PhoneNumber as phone_number from users t1 join thong_tin_user t2 on t1.Id = t2.user_id";

            if (keyword != null && keyword != "")
            {
                ViewBag.keyword = keyword;
                sql += " where t1.Email like '%" + keyword + "%'" + " or t1.PhoneNumber like '%" + keyword + "%'";
            }

            var data = db.Database.SqlQuery<ListUser>(sql).ToList();

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
            var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x=>x.id).Select(x => x).Take(10).ToList();
            return PartialView("_LoadNewHot", model);
        }

        public ActionResult LoadNewHot2()
        {
            var model = (from s in db.news where s.isHot == 1 orderby s.id descending select s).Take(3).ToList();
            //var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x => x.id).Select(x => x).Take(7).ToList();
            return PartialView("_LoadNewHot2", model);
        }

        public ActionResult LoadNewTopHot2()
        {
            var model = (from s in db.news where s.isHot == 1 orderby s.id descending select s).Skip(3).Take(3).ToList();

            //var model = db.news.Where(x => x.isHot == 1).OrderByDescending(x => x.id).Select(x => x).Skip(7).Take(3).ToList();
            return PartialView("_LoadNewTopHot2", model);
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

        public ActionResult LoadNewInCat(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.id).Select(x => x).ToList().Take(8);// && x.isHot == 0
            return PartialView("_LoadNewInCat", model.ToList());
        }

        public ActionResult LoadNewInCat2(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.id).Select(x => x).ToList().Skip(1).Take(8);
            return PartialView("_LoadNewInCat2", model.ToList());// && x.isHot == 0
        }

        public ActionResult LoadNewInCat_top(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id).OrderByDescending(x => x.id).Select(x => x).First();// && x.isHot == 0
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
                    var p4 = (from q in db.news select q).OrderByDescending(o => o.id).ToList();
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
            var model = db.thanh_vien.OrderByDescending(o=>o.id).Select(x => x).Take(4).ToList();
            return PartialView("_LoadThanhVien", model);
        }
    }
}