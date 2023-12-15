using Microsoft.AspNetCore.Mvc.Rendering;

namespace prjMusicBetter.Models.ViewModels
{
    public class CommentListViewModel
    {
        public List<TComment> Comments { get; set; }

        public List<TArticle> Articles { get; set; }

        public int SelectedFArticleId { get; set; }

        public SelectList FArticleIdOptions { get; set; }
    }
}
