using Humanizer;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.ViewModels;


namespace prjMusicBetter.Models.Services
{
    public class MemberService
    {
        private readonly dbSoundBetterContext _context;
        private readonly IWebHostEnvironment _environment;
        MemberDao _dao;
        public MemberService(dbSoundBetterContext Context, IWebHostEnvironment environment)
        {
            _context = Context;
            _environment = environment;
            _dao = new MemberDao(_context,_environment);
        }
       
    }
}
