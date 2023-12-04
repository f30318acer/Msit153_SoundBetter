using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    public class MemberPasswordVM
    {
        public int fMemberID { get; set; }
        public string? fPassword { get; set; }
        [Display(Name = "舊密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string? OldfPassword { get; set; }
        [Display(Name = "新密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string? NewfPassword { get; set; }
        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "尚未{0}!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "{0}必須與新密碼一致")]
        public string? ConfirmPassword { get; set; }
    }
}
