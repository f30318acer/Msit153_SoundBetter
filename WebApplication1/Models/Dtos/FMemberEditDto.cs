namespace prjMusicBetter.Models.Dtos

{
    public class FMemberEditDto
    {
        public int fMemberID { get; set; }
        public string? fPhotoPath { get; set; }
        public string fUsername { get; set; }
        public string fName { get; set; }
        public string fPassword { get; set; }
        public string fPhone { get; set; }
        public string fEmail { get; set; }
        public string? fGender { get; set; }
        public string? fBirthday { get; set; }
        public string fIntroduction {  get; set; }
    }
}
