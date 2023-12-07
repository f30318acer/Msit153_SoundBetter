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
      
    }
}
