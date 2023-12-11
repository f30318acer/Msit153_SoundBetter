using prjMusicBetter.Models;

namespace prjMusicBetter.Models.ViewModels
{
    public class MemberProjectVM
    {
        public TMember Member { get; set; }
        public List<TProject> Project { get; set; }
    }
}