using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gpw.Models;

namespace gpw.Controllers
{
    public class GiaphaController : Controller
    {
        // GET: Giapha
        private gpwEntities db = new gpwEntities();
        public ActionResult Index()
        {

            return View();
        }
        public class NodeItem
        {
            public string name1 { get; set; }
            public string name2 { get; set; }
            public long? id1 { get; set; }
            public long? id2 { get; set; }
        }
        public ActionResult Tree()
        {
            var p = (from q in db.gia_pha where q.thanh_vien_id == -1 select q).ToList();
            NodeItem[] NI=new NodeItem[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                NodeItem niit = new NodeItem();
                niit.id1 = p[i].tv_id_1;
                niit.id2 = p[i].tv_id_2;
                niit.name1 = p[i].name1;
                niit.name2 = p[i].name2;
                NI[i] = niit;
            }

            return View();
        }
    }
}