using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    public class FMemberVM
    {
        //畫面上顯示甚麼這裡就會出現甚麼
        //MemberDao 那裏也要出現資料，但是有些不會在註冊畫面顯示的則會在那邊寫死
        public string? fPhotoPath {  get; set; }
        public IFormFile? Photo {  get; set; }

        [Display(Name = "用戶者姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string fUsername { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage ="{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string fName {  get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string fPassword { get; set; }

        //[Display(Name = "確認密碼")]
        //[Required(ErrorMessage = "尚未{0}!")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "{0}必須與密碼一致")]
        //public string ComfirmPassword { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.EmailAddress)]
        public string fEmail { get; set; }

        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string fPhone { get; set; }

        public string fGender { get; set; }
        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Date)]
        public string fBirthday { get; set; }
        
    }
}
