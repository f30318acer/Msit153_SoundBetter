using System.ComponentModel.DataAnnotations;

namespace prjMusicBetter.Models.Dtos.Comment
{
    public class CommentDto
    {   
        public int FCommentId { get; set; }

        public int FArticleId { get; set; }

        public int FMemberId { get; set; }

        public DateTime FCommentTime { get; set; }

        public string FName { get; set; }

        public string FPhotoPath { get; set; }



        [Required]
        public string FCommentContent { get; set; }
    }
}
