using Microsoft.Identity.Client;

namespace prjMusicBetter.Models.EFModels
{
    public partial class Members
    {
        public Members() 
        {
            new HashSet<Collect>();
        }

        public int fMemberID {  get; set; }
        public string fUsername { get; set; }
        public string fName { get; set; }
        public string fPassword {  get; set; }
        public string fPhone { get; set; }
        public string fEmail { get; set; }
        public int fGender { get; set;}
        public DateTime? fBirthday { get; set; }
        public DateTime? fCreationTime {get; set; }
        public string fIntroduction {  get; set;}
        public int fPermissionID {  get; set; }
        public string fPhotoPath {  get; set; }
    }
}
