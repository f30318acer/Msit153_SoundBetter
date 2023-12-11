namespace prjMusicBetter.Models.ViewModels
{
    public class FriendsViewModel
    {
        public TMember Member { get; set; }
        public List<TMember> Friends { get; set; }
        public List<TMember> BlackList { get; set; }

    }
}