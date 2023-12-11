using prjMusicBetter.Models;

namespace prjMusicBetter.Models.ViewModels
{
    public class MemberWorksVM
    {
        public TMember Member { get; set; }
        public List<TWork> Works { get; set; }  
    }
}