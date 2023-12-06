using prjMusicBetter.Models;

namespace prjMusicBetter.Controllers
{
    internal class FriendsViewModel
    {
        public TMember Member { get; set; }
        public List<int> Friends { get; set; }
    }
}