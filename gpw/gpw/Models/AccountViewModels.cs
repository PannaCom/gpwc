using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gpw.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Địa chỉ email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập chính xác địa chỉ email của thành viên.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhớ mật khẩu")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage="Vui lòng nhập họ tên.")]        
        public string ho_ten { get; set; }

        [Required(ErrorMessage="Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập chính xác địa chỉ email.")]
        [Display(Name = "Địa chỉ email")]
        public string Email { get; set; }

        [Required(ErrorMessage="Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "{0} phải có ít nhất là {2} kí tự độ dài.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không giống.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string dia_chi { get; set; }

        public string so_cmt { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nghề nghiệp")]
        public string nghe_nghiep { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập học vấn")]
        public string hoc_van { get; set; }

        public string hinh_anh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên thường gọi")]
        public string biet_danh { get; set; }

        public Nullable<int> doi_thu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới tính")]
        public string gioi_tinh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public string ngay_sinh { get; set; }

        //public Nullable<int> quyen_han { get; set; }
        //public string user_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại địa chỉ")]
        public Nullable<double> lon { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại địa chỉ")]
        public Nullable<double> lat { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập cơ quan công tác")]
        public string cq_ctac { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public partial class thanh_vien_model
    {
        public long id { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]        
        public string ho_ten { get; set; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]   
        public string dia_chi { get; set; }

        [Display(Name = "Số chứng minh thư")]
        public string so_cmt { get; set; }

        [Display(Name = "Nghề nghiệp")]
        [Required(ErrorMessage = "Vui lòng nhập nghề nghiệp.")]   
        public string nghe_nghiep { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [Required(ErrorMessage = "Vui lòng nhập ảnh đại diện.")]   
        public string hinh_anh { get; set; }

        [Display(Name = "Tên thường gọi")]
        //[Required(ErrorMessage = "Vui lòng nhập tên thường gọi.")]   
        public string biet_danh { get; set; }

        [Display(Name = "Học vấn")]
        [Required(ErrorMessage = "Vui lòng nhập học vấn.")]
        public string hoc_van { get; set; }

        [Display(Name = "Trình độ")]
        [Required(ErrorMessage = "Vui lòng nhập trình độ.")]
        public string trinh_do { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Vui lòng nhập giới tính.")]
        public string gioi_tinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        public string ngay_sinh { get; set; }
        public Nullable<double> lon { get; set; }
        public Nullable<double> lat { get; set; }

        [Display(Name = "Cơ quan công tác")]
        [Required(ErrorMessage = "Vui lòng nhập cơ quan công tác.")]
        public string cq_ctac { get; set; }

        [Required(ErrorMessage="Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage="Địa chỉ e-mail không đúng.")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu thành viên")]
        public string pass { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận lại mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("pass", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string so_dien_thoai { get; set; }
    }

    public class thanh_vien_login_model
    {
        [Required(ErrorMessage="Vui lòng nhập địa chỉ email.")]
        [Display(Name = "Địa chỉ email")]
        [EmailAddress(ErrorMessage = "Vui lòng nhập chính xác địa chỉ email của thành viên.")]
        public string Email { get; set; }

        [Required(ErrorMessage="Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        
    }

    public class quanhethanhvienModel
    {
        public long id { get; set; }
        [Display(Name = "Tên thành viên")]
        [Required(ErrorMessage = "Vui lòng nhập tên thành viên.")]
        public string ten_thanh_vien { get; set; }

        public string ten_quan_he { get; set; }
        public Nullable<long> thanh_vien_id { get; set; }

        [Display(Name = "Quê quán")]
        [Required(ErrorMessage = "Vui lòng nhập quê quán.")]
        public string que_quan { get; set; }

        public Nullable<double> qq_lon { get; set; }
        public Nullable<double> qq_lat { get; set; }
    }
}
