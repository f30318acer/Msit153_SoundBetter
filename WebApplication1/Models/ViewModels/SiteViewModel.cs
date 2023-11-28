using prjMusicBetter.Models;

namespace prjMusicBetter.Models.ViewModels
{
    internal class SiteViewModel
    {
        public TSite? TSite { get; set; }
        public TSitePicture? TSitePicture { get; set; }
        public virtual TCity? FCity { get; set; }
        public virtual TMember? FMember { get; set; }
    }
}