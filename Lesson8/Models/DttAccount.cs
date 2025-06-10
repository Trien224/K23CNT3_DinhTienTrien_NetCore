using System;
using System.ComponentModel.DataAnnotations;

namespace Lesson8.Models
{
    public class DttAccount
    {      
        [Key]
        [Display(Name = "Mã")] 
        public int DttId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [MinLength(2, ErrorMessage = "Họ và tên phải có ít nhất 2 ký tự.")]
        [MaxLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string DttName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DttBirthDay { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string DttEmail { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đủ 10 số.")]
        public string DttPhone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DttAddress { get; set; }
       
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        [DataType(DataType.Password)]
        public string DttPassword { get; set; }
       
        [Display(Name = "Facebook")]
        public string DttStatus { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string DttAvatar { get; set; }  // Lưu tên file hoặc đường dẫn ảnh
    }
}
