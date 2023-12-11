namespace prjMusicBetter.Models.ViewModels
{
    public class FriendsViewModel
    {
        public TMember Member { get; set; }
        public List<TMember> Friends { get; set; }
        public List<TMember> BlackList { get; set; }


		// 分頁相關的屬性
		public int CurrentPage { get; set; }  // 當前頁碼
		public int TotalPages { get; set; }   // 總頁數
		public int PageSize { get; set; }     // 每頁顯示的項目數量

		public bool HasPreviousPage => CurrentPage > 1;           // 是否有上一頁
		public bool HasNextPage => CurrentPage < TotalPages;       // 是否有下一頁

		// 可選：計算總項目數量，如果您從數據庫中能直接獲得此數據，則此屬性不是必需的
		public int TotalItems { get; set; }    // 總項目數量
	}
}