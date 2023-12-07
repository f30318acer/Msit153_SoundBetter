using Microsoft.EntityFrameworkCore.Metadata.Internal;
using prjMusicBetter.Models.ViewModels;

namespace prjMusicBetter.Models.Services
{
    public class MemberDataService
    {
        private readonly dbSoundBetterContext _context;

        public MemberDataService(dbSoundBetterContext context)
        {
            _context = context;
        }
        public List<ChartDataModel> GetMemberStatistics()
        {
            // 假设您的 TMember 实体中有一个创建时间的属性（例如 FCreationTime）
            // 下面的代码将按年对会员进行分组，并计算每年的会员数量
            var statistics = _context.TMembers
                .AsEnumerable() // 如果数据量很大，可能需要优化这部分代码
                .GroupBy(m => m.FCreationTime.Year)
                .Select(group => new ChartDataModel
                {
                    Label = group.Key.ToString(), // 年份作为标签
                    Value = group.Count() // 该年的会员数量
                })
                .ToList();

            return statistics;
        }
    }
}
