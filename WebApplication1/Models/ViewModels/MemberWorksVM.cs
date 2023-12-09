using prjMusicBetter.Models;

namespace prjMusicBetter.Models.ViewModels
{
    internal class MemberWorksVM
    {
        public TMember Member { get; set; }
        public List<TWork> Works { get; set; }
    }
}