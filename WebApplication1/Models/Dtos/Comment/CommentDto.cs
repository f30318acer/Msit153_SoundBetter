using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.Dtos.Comment
{
    public class CommentDto
    {
        public int FArticleId { get; set; }

        public string MemberId { get; set; }
        
        [Required]
        public string FCommentContent { get; set; }
    }
}
