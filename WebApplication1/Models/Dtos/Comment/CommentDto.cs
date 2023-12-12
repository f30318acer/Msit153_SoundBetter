namespace prjMusicBetter.Models.Dtos.Comment
{
    public class CommentDto
    {
        public int ArticleId { get; set; }
        public int MemberId { get; set; }
        public string Content { get; set; }
    }
}
