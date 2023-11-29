﻿using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.ViewModels
{
    public class FMemberVM
    {
        public string? fPhotoPath {  get; set; }
        public IFormFile? Photo {  get; set; }
        [Display(Name = "使用者姓名")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        public string? fUsername { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage ="{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string fName {  get; set; }
        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Password)]
        public string fPassword { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.EmailAddress)]
        public string fEmail { get; set; }

        [Display(Name = "電話")]
        [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "{0}必須為數字")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Text)]
        public string fPhone { get; set; }

        public int fGender { get; set; }

        public string? fCreationTime { get; set; }

        [Display(Name = "生日")]
        [Required(ErrorMessage = "{0}是必填欄位!")]
        [DataType(DataType.Date)]
        public string fBirthday { get; set; }
    }
}
