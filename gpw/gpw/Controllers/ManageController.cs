using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using gpw.Models;
using System.IO;
using gpw.helpers;
using Newtonsoft.Json;

namespace gpw.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private gpwEntities db = new gpwEntities();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        //public ActionResult Index()
        //{
        //    //ViewBag.StatusMessage =
        //    //    message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //    //    : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //    //    : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
        //    //    : message == ManageMessageId.Error ? "An error has occurred."
        //    //    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
        //    //    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
        //    //    : "";

        //    var userId = User.Identity.GetUserId();
        //    var userInfo = db.thong_tin_user.Where(x => x.user_id == userId).FirstOrDefault();

        //    //var model = new IndexViewModel
        //    //{
        //    //    HasPassword = HasPassword(),
        //    //    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
        //    //    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
        //    //    Logins = await UserManager.GetLoginsAsync(userId),
        //    //    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
        //    //};
        //    return View(userInfo);
        //}

        [AllowAnonymous]
        public string getHocVan(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.hoc_van where q.ten_truong.Contains(keyword) orderby q.ten_truong ascending select q.ten_truong).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        [AllowAnonymous]
        public string getNgheNghiep(string keyword)
        {
            if (keyword == null) keyword = "";
            var p = (from q in db.nghe_nghiep where q.ten_nghe_nghiep.Contains(keyword) orderby q.ten_nghe_nghiep ascending select q.ten_nghe_nghiep).ToList().Distinct();
            return JsonConvert.SerializeObject(p);
        }

        //[HttpPost, ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        //public ActionResult CapNhatThongTin(thong_tin_user model, string ngay_sinh2)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        TempData["error"] = "Vui lòng kiểm tra lại các trường";
        //        return RedirectToAction("Index");
        //    }
        //    var userId = User.Identity.GetUserId();

        //    //string sql = "update thong_tin_user set ho_ten = N'" + model.ho_ten + "', biet_danh = N'" + model.biet_danh + "', gioi_tinh = N'" + model.gioi_tinh + "', hoc_van = N'" + model.hoc_van + "', dia_chi = N'" + model.dia_chi + "', ngay_sinh = '" + model.ngay_sinh.ToString() + "', nghe_nghiep = N'" + model.nghe_nghiep + "', trang_thai = '1', quyen_han = '" + model.quyen_han + "', ngay_tao = '" + DateTime.Now + "', hinh_anh = '" + model.hinh_anh + "', cq_ctac = N'" + model.cq_ctac + "', lon = '" + model.lon + "', lat = '" + model.lat + "', so_cmt = '"+ model.so_cmt +"' where user_id = '" + userId + "'";

        //    //var updateInfo = db.Database.ExecuteSqlCommand(sql);
        //    var userInfo = db.thong_tin_user.Where(x => x.user_id == userId).FirstOrDefault();
        //    userInfo.ho_ten = model.ho_ten ?? null;
        //    userInfo.biet_danh = model.biet_danh ?? null;
        //    userInfo.cq_ctac = model.cq_ctac ?? null;
        //    userInfo.dia_chi = model.dia_chi ?? null;
        //    userInfo.gioi_tinh = model.gioi_tinh ?? null;
        //    userInfo.hinh_anh = model.hinh_anh ?? null;
        //    userInfo.hoc_van = model.hoc_van ?? null;
        //    userInfo.lat = model.lat ?? null;
        //    userInfo.lon = model.lon ?? null;
        //    string dateTime = ngay_sinh2 != null ? ngay_sinh2 : null;
        //    DateTime? dt = new DateTime();
        //    if (dateTime != null)
        //    {
        //        dt = Convert.ToDateTime(dateTime);
        //    }
        //    userInfo.ngay_sinh = ngay_sinh2 != null ? dt : userInfo.ngay_sinh;
        //    userInfo.ngay_tao = DateTime.Now;
        //    userInfo.nghe_nghiep = model.nghe_nghiep ?? null;
        //    userInfo.quyen_han = model.quyen_han ?? null;
        //    userInfo.so_cmt = model.so_cmt ?? null;
        //    userInfo.trang_thai = model.trang_thai ?? null;
        //    db.Entry<thong_tin_user>(userInfo).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    TempData["update"] = "Cập nhật thành công";
        //    return RedirectToAction("Index");
        //}



        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        // 1. Khởi tạo doành họ, tên dòng họ
        // 2. Update thông tin thành viên
        public ActionResult uptv()
        {
            return View();
        }

        [AllowAnonymous]
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

        // 


        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}