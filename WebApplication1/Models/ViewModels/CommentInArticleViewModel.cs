using prjMusicBetter.Models.Dtos.Comment;

namespace prjMusicBetter.Models.ViewModels
{
    public class CommentInArticleViewModel
    {

        public TArticle article { get; set; }

        public CommentDto commentDto { get; set; }
    }
}
