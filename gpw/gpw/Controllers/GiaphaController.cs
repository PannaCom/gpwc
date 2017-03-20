using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gpw.Models;
using Newtonsoft.Json;
using gpw.helpers;
using System.Net;
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
            public string name_node { get; set; }
            public long? id_node { get; set; }
            public long? parent_id_node { get; set; }
            public string title { get; set; }
            public int status { get; set; }
        }

        public ActionResult Share(long? user_id, long? group_id,string dir)
        {
            long? max_id = -1;
            if (user_id == null) user_id = -1;
            if (group_id == null) group_id = 1;
            var p = (from q in db.user_family_tree where q.status == 0 && q.user_id == user_id && q.group_id == group_id select q).ToList();
            NodeItem[] NI = new NodeItem[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                NodeItem niit = new NodeItem();
                niit.name_node = p[i].name_node;
                niit.id_node = p[i].id_node;
                niit.parent_id_node = p[i].parent_id_node;
                niit.title = p[i].title;
                niit.status = 0;
                NI[i] = niit;
                if (p[i].id_node > max_id) max_id = p[i].id_node;
            }
            if (dir == null || dir == "") dir = "t2b";
            ViewBag.dir = dir;
            ViewBag.allTree = allTree(ref NI);
            ViewBag.max_id = max_id;
            ViewBag.user_id = user_id;
            ViewBag.group_id = group_id;
            ViewBag.title_des = db.giapha_des.Find(group_id).giapha_name;
            ViewBag.des = db.giapha_des.Find(group_id).des;
            ViewBag.image = "http://vietgiapha.com/images/logo.png";
            ViewBag.url = Config.domain + "/giapha/share?user_id=" + user_id + "&group_id=" + group_id;
            //ViewBag.title = "Tạo cây gia phả - Phả hệ phả đồ " + ViewBag.title_des;

            return View();
        }
        public ActionResult Tree(long? user_id,long? group_id,string dir)
        {
            var thanhvien_id = configs.getCookie("thanhvien_id");
            long? _id = Convert.ToInt32(thanhvien_id);
            if (_id != user_id) return new HttpStatusCodeResult(HttpStatusCode.NoContent);

            long? max_id = -1;
            if (user_id == null) user_id = -1;
            if (group_id == null) group_id = 1;
            var p = (from q in db.user_family_tree where q.status==0 && q.user_id == user_id && q.group_id==group_id select q).ToList();
            NodeItem[] NI=new NodeItem[p.Count];
            for (int i = 0; i < p.Count; i++)
            {
                NodeItem niit = new NodeItem();
                niit.name_node = p[i].name_node;
                niit.id_node = p[i].id_node;
                niit.parent_id_node = p[i].parent_id_node;
                niit.title = p[i].title;
                niit.status = 0;
                NI[i] = niit;
                if (p[i].id_node > max_id) max_id = p[i].id_node;
            }
            if (dir == null || dir == "") dir = "t2b";
            ViewBag.dir = dir;
            ViewBag.allTree = allTree(ref NI);
            ViewBag.max_id = max_id;
            ViewBag.user_id = user_id;
            ViewBag.group_id = group_id;
            ViewBag.title_des = db.giapha_des.Find(group_id).giapha_name;
            ViewBag.des = db.giapha_des.Find(group_id).des;
            ViewBag.image = "http://vietgiapha.com/images/logo.png";
            ViewBag.url = Config.domain + "/giapha/share?user_id=" + user_id + "&group_id=" + group_id;
            return View();
        }
        public string allTree(ref NodeItem[] NI)
        {
            string temp = "";
           
            for (int i = 0; i < NI.Length; i++)
            if (NI[i].status==0)
            {
                
                temp +="{";
                temp += "'id': '" + NI[i].id_node + "', 'name': '" + NI[i].name_node + "', 'title': '" + NI[i].title + "',\r\n";
                NI[i].status = 1;
                temp += findAllChild(ref NI, NI[i]) + "},";
               
            }
            //temp += "";
            while (temp.EndsWith(",") || temp.EndsWith(";"))
            {
                temp = temp.Substring(0, temp.Length - 1);
            }
            return temp;
        }
        public string findAllChild(ref NodeItem[] NI,NodeItem item)
        {
            string temp2 = "";
            bool found = false;
            for (int j = 0; j < NI.Length; j++)
                if (NI[j].status == 0 && NI[j].parent_id_node == item.id_node)
                {
                    temp2 += "{'id': '" + NI[j].id_node + "', 'name': '" + NI[j].name_node + "', 'title': '" + NI[j].title + "',\r\n";
                    NI[j].status = 1;
                    found = true;
                    temp2 += findAllChild(ref NI, NI[j]) + "},";
                }
            if (found) temp2 = "'children': [" + temp2 + "]\r\n";
            return temp2;
        }
        [Serializable]
        public class csItem {
            public int id { get; set; }
            public string name { get; set; }
        }
        [Serializable]
        public class csTreeItem
        {
            public long user_id {get;set;}
            public csItem[] TreeItem { get; set; }
        }
        [HttpPost]
        public string UpdateNode(int? id,string name, string title,int? parent_id,long? user_id,long? group_id)
        {
            var thanhvien_id = configs.getCookie("thanhvien_id");
            long? _id = Convert.ToInt32(thanhvien_id);
            if (_id != user_id) return "0";
            //try {
            var found = db.user_family_tree.Any(o => o.status == 0 && o.id_node == id && o.user_id == user_id && o.group_id == group_id);
            if (found) { 
                string query = "update user_family_tree set name_node=N'" + name + "',title=N'" + title + "' where user_id=" + user_id + " and group_id=" + group_id + " and id_node=" + id;
                db.Database.ExecuteSqlCommand(query);
            }
            else
            {
                user_family_tree uft = new user_family_tree();
                uft.id_node = id;
                uft.name_node = name;
                uft.title = title;
                uft.parent_id_node = parent_id;
                uft.user_id = user_id;
                uft.group_id = group_id;
                uft.status = 0;
                uft.date_time = DateTime.Now;
                db.user_family_tree.Add(uft);
                db.SaveChanges();
            }
            
            return "1";
            //}
            //catch (Exception ex)
            //{
            //    return "0";
            //}
        }
        [HttpPost]
        public string DelNode(int? parent_id,int? id_node, long? user_id, long? group_id)
        {
            try
            {
                var thanhvien_id = configs.getCookie("thanhvien_id");
                long? _id = Convert.ToInt32(thanhvien_id);
                if (_id != user_id) return "0";
                string query = "update user_family_tree set status=1 where status=0 and user_id=" + user_id + " and group_id=" + group_id + " and parent_id_node=" + parent_id + " and id_node=" + id_node;
                db.Database.ExecuteSqlCommand(query);
                DelAllNode(id_node, user_id, group_id);
                return "1";
            }catch(Exception ex){
                return "0";
            }
        }
        public void DelAllNode(int? id_node, long? user_id, long? group_id)
        {
            
                bool found = db.user_family_tree.Any(o=>o.status==0 && o.user_id == user_id && o.group_id == group_id && o.parent_id_node == id_node);
                if (found)
                {
                    var p = db.user_family_tree.Where(o => o.status == 0 && o.user_id == user_id && o.group_id == group_id && o.parent_id_node == id_node).ToList();
                    string query = "update user_family_tree set status=1 where status=0 and user_id=" + user_id + " and group_id=" + group_id + " and parent_id_node=" + id_node;
                    db.Database.ExecuteSqlCommand(query);
                    for (int i = 0; i < p.Count; i++) {
                        DelAllNode(p[i].id_node, user_id, group_id);
                    }
                }
               
            
        }
        [HttpPost]
        public string DelTree(long? user_id, long? group_id)
        {
            try{
                string query = "update user_family_tree set status=1 where status=0 and user_id=" + user_id + " and group_id=" + group_id;
                db.Database.ExecuteSqlCommand(query);
                query = "update giapha_des set status=1 where thanhvien_id=" + user_id + " and id=" + group_id;
                db.Database.ExecuteSqlCommand(query);
                return "1";
            }catch(Exception ex){
                return "0";
            }
        }
        [HttpPost]
        public string AddNew(string TreeItem, long? user_id, long? group_id)
        {
            try
            {
                var thanhvien_id = configs.getCookie("thanhvien_id");
                long? _id = Convert.ToInt32(thanhvien_id);
                if (_id != user_id) return "0";

                dynamic StudList = JsonConvert.DeserializeObject(TreeItem);

                var stud = StudList.TreeItem;
                foreach (var detail in stud)
                {
                    var name = detail["name"];
                    var title = detail["title"];
                    int id = 0, parent_id=0;
                    bool result = Int32.TryParse((string)detail["id"], out id);
                    result = Int32.TryParse((string)detail["parent_id"], out parent_id);
                    var found = db.user_family_tree.Any(o => o.status == 0 && o.id_node == id && o.parent_id_node == parent_id && o.user_id == user_id && o.group_id == group_id);
                    if (found)
                    {
                        string query = "update user_family_tree set name_node=N'" + name + "',title=N'" + title + "' where user_id=" + user_id + " and group_id=" + group_id + " and id_node=" + id;
                        db.Database.ExecuteSqlCommand(query);
                    }
                    else
                    {
                        user_family_tree uft = new user_family_tree();
                        uft.id_node = id;
                        uft.name_node = name;
                        uft.title = title;
                        uft.parent_id_node = parent_id;
                        uft.user_id = user_id;
                        uft.group_id = group_id;
                        uft.status = 0;
                        uft.date_time = DateTime.Now;
                        db.user_family_tree.Add(uft);
                        db.SaveChanges();
                    }

                }
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
    }
}