using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace prjMusicBetter.Models.Dtos

{
    public class FMemberEditDto
    {
        public int FMemberID { get; set; }
        public string? FPhotoPath { get; set; }
        public string FUsername { get; set; }
        public string FName { get; set; }
        public string FPassword { get; set; }
        public string FPhone { get; set; }
        public string FEmail { get; set; }
        public string? FGender { get; set; }
        public string? FBirthday { get; set; }
        public string? FCreationTime {  get; set; }
        public string FIntroduction { get; set; }
        public int? FPermissionId { get; set; }

    }
}
