using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    public class FMemberEditVM
    {
        public int FMemberID { get; set; }
        public string? PhotoPath { get; set; }
        public IFormFile? Photo { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string FUsername { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string FName { get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string FPassword { get; set; }
        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string FPhone { get; set; }
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.EmailAddress)]
        public string FEmail { get; set; }
        public string? FGender { get; set; }
        public string? FBirthday { get; set; }
    }
}
