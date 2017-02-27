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
            var model = db.news.Where(x => x.cat_id == cat_id && x.isHot == 0).OrderByDescending(x => x.id).Select(x => x).ToList().Take(8);
            return PartialView("_LoadNewInCat", model.ToList());
        }

        public ActionResult LoadNewInCat2(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id && x.isHot == 0).OrderByDescending(x => x.id).Select(x => x).ToList().Skip(1).Take(8);
            return PartialView("_LoadNewInCat2", model.ToList());
        }

        public ActionResult LoadNewInCat_top(int cat_id)
        {
            var model = db.news.Where(x => x.cat_id == cat_id && x.isHot == 0).OrderByDescending(x => x.id).Select(x => x).First();
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


    }
}