using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    //登入畫面上所顯示的欄位
    public class LoginVM
    {
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
