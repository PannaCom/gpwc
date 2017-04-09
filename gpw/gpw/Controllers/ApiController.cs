using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gpw.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using gpw.helpers;
using Newtonsoft.Json;
namespace gpw.Controllers
{
    public class ApiController : Controller
    {
        private gpwEntities db = new gpwEntities();
        // GET: Api
        public ActionResult Index()
        {
            return View();
        }
        //Đăng ký thành viên với tên, email, số điện thoại, pass
        //Trả về id thành viên, lưu id thành viên này vào máy mobile để dùng sau này, đặt tên là user_id
        [HttpPost]
        public string Reg(string name,string email, string phone,string pass)
        {
            try
            {
                thanh_vien tv = new thanh_vien();
                tv.ho_ten = name;
                tv.email = email;
                tv.so_dien_thoai = phone;
                string passHash = pass != null ? configs.Encrypt(pass) : null;
                tv.pass = passHash;
                db.thanh_vien.Add(tv);
                db.SaveChanges();
                return tv.id.ToString();

            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        //Đăng nhập, nếu thành công trả về id của user(user_id) và số điện thoại với định dạng id_sốđiệnthoại
        //Tách id và số điện thoại ra lưu vào máy để dùng sau này 
        // Trả về 0 nếu không đăng nhập được, trả về -1 nếu lỗi code lập trình
        [HttpPost]
        public string login(string email, string pass)
        {
            try
            {
                string passHash = pass != null ? configs.Encrypt(pass) : null;
                if (db.thanh_vien.Any(o => o.email == email && o.pass == passHash))
                {
                    var tv=db.thanh_vien.Where(o => o.email == email && o.pass == passHash).FirstOrDefault();
                    return tv.id.ToString()+"_"+tv.so_dien_thoai;
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        //Khi chọn Export, nếu đăng nhập rồi thì hiện ra hộp inputbox để user nhập tên cây gia phả này
        //Lưu ý: Nếu đã có group_id rồi thì hỏi user bạn muốn export ra bản mới hay ghi đè lên cái cũ, nếu ghi đè lên thì chuyển luôn sang step 2, nếu là mới thì gọi cái hàm này để có group_id mới
        //Gửi lên server tên cây gia phả này đi kèm với user_id của user (Đã save ở hàm login hoặc hàm reg trước đó)
        //Trả về là id của Group cây gia phả này, lưu id này về máy, đặt tên là group_id, trả về -1 nếu lỗi code lập trình
        [HttpPost]
        public string export_step1(long user_id, string name)
        {
            try
            {
                //string query = "update giapha_des set status=1 where user_id=" + user_id;
                //db.Database.ExecuteSqlCommand(query);
                giapha_des gpd = new giapha_des();
                gpd.giapha_name = name;
                gpd.thanhvien_id = user_id;
                gpd.date_time = DateTime.Now;
                gpd.status = 0;
                db.giapha_des.Add(gpd);
                db.SaveChanges();
                return gpd.id.ToString();

            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        //Ở bước 2 export, sau khi user đã đặt tên cho cây gia phả này rồi, thì user sẽ có 2 id là user_id(của user) và group_id(của gia phả)
        //Gửi toàn bộ cây gia phả theo cấu trúc ví dụ như sau:
        //{"user_id":121,"TreeItem":[{"id":"1","name":"Nguyễn Văn A","title":"Cụ tổ","parent_id":0,"phone":"value","image":"value"},{"id":"3","name":"Nguyễn văn B","title":"","parent_id":"1","phone":"value","image":"value"},{"id":"5","name":"Nguyễn văn C","title":"","parent_id":"1","phone":"value","image":"value"},{"id":"7","name":"Nguyễn văn C1","title":"","parent_id":"5","phone":"value","image":"value"},{"id":"9","name":"Nguyễn văn C2","title":"","parent_id":"5","phone":"value","image":"value"}]}
        //trong đó user_id là id của user đã được save ở máy, name là tên node, title chính là note, phone là số điện thoại của node, 
        //Trả về =1 nếu ok, =0 nếu lỗi
        [HttpPost]
        public string export_step2(string TreeItem, long? user_id, long? group_id)
        {
            try
            {
                string query = "update user_family_tree set status=1 where user_id=" + user_id + " and group_id=" + group_id;
                db.Database.ExecuteSqlCommand(query);
                dynamic StudList = JsonConvert.DeserializeObject(TreeItem);
                var stud = StudList.TreeItem;
                foreach (var detail in stud)
                {
                    var name = detail["name"];
                    var title = detail["title"];
                    var phone = detail["phone"];
                    var image = detail["image"];
                    int id = 0, parent_id = 0;
                    bool result = Int32.TryParse((string)detail["id"], out id);
                    result = Int32.TryParse((string)detail["parent_id"], out parent_id);
                    user_family_tree uft = new user_family_tree();
                    uft.id_node = id;
                    uft.name_node = name;
                    uft.phone_node = phone;
                    uft.image_node = image;
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
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        //Hàm này lấy về danh sách các cây gia phả của user id, sẽ hỏi user chọn cây gia phả nào để import?
        //User chọn cây gia phả nào thì lấy id cây gia phả đó ra, đặt tên là group_id để gọi tiếp hàm ở step 2 để lấy về toàn bộ dữ liệu cây gia phả đó, nếu lỗi trả về -1
        [HttpPost]
        public string import_step1(long? user_id)
        {
            try
            {
                var p = db.giapha_des.Where(o => o.thanhvien_id == user_id && o.status==0).ToList();

                return JsonConvert.SerializeObject(p);
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
        //Hàm này lấy về cây gia phả của user_id và group_id
        //Trả về json là toàn bộ dữ liệu cây gia phả dưới dạng json, nếu lỗi trả về -1
        [HttpPost]
        public string import_step2(long? user_id,long? group_id)
        {
            try
            {
                //var p = db.giapha_des.Where(o => o.thanhvien_id == user_id).FirstOrDefault();
                //long group_id = p.id;
                var rs = (from q in db.user_family_tree where q.user_id == user_id && q.group_id == group_id && q.status == 0 select q).ToList();
                return JsonConvert.SerializeObject(rs);
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }
    }
}