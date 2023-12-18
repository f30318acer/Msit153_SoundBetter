using prjMusicBetter.Models;

namespace prjMusicBetter.Models.ViewModels
{
    internal class SiteViewModel
    {
        public TSite? TSite { get; set; }
        
        public string SiteTypeText
        {
            get
            {
                var siteTypeMapping = new Dictionary<int, string>
                {
                    { 1, "音樂學校" },
                    { 2, "錄音室" },
                };

                return siteTypeMapping.GetValueOrDefault(TSite?.FSiteType ?? 0, "未知");
            }
        }
    }
}