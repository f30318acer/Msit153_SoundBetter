using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjMusicBetter.Models;
using prjMusicBetter.Models.Daos;
using prjMusicBetter.Models.Dtos;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.Services;
using prjMusicBetter.Models.ViewModels;
using System.Linq;
using System.Net.WebSockets;

namespace prjMusicBetter.Controllers
{
    [Authorize(Roles = "Member")]
    public class MembersController : Controller
    {

        private readonly dbSoundBetterContext _context;
        private readonly UserInfoService _userInfoService;
        private readonly IWebHostEnvironment _environment;
        MemberService _service;
        MemberDao _memberDao;


        public MembersController(dbSoundBetterContext context, UserInfoService userInfoService, IWebHostEnvironment environment)
        {
            _context = context;
            _userInfoService = userInfoService;
            _environment = environment;
            _service = new MemberService(_context, _environment);
            _memberDao = new MemberDao(_context, _environment);

        }
        public IActionResult Index(string display)
        {

            ViewBag.Display = display;


            // 调用MemberWorks方法来获取总数
            TMember member = _userInfoService.GetMemberInfo();
            var memberworkIds = _context.TWorks
                .Where(x => x.FMemberId == member.FMemberId)
                .Select(x => x.FWorkId);
            var totalItems = _context.TWorks.Count(c => memberworkIds.Contains(c.FWorkId));

            // 将总数存储到ViewBag中
            ViewBag.TotalWorks = totalItems;

            // 调用MemberProject方法来获取总数
            TMember member1 = _userInfoService.GetMemberInfo();
            var projectIds = _context.TProjects
                .Where(p => p.FMemberId == member1.FMemberId)
                .Select(p => p.FProjectId);

            var totalItemsProjects = _context.TProjects.Count(p=>projectIds.Contains(p.FProjectId));
            // 将总数存储到ViewBag中
            ViewBag.TotalProjects = totalItemsProjects;

            //追隨
            TMember member2 = _userInfoService.GetMemberInfo();

            var friendsIds = _context.TMemberRelations
                .Where(x => x.FMemberId == member2.FMemberId && x.FMemberRelationStatusId == 1)
                .Select(x => x.FRelationMemberId);

            var totalItemFriends = _context.TMembers.Count(p=>friendsIds.Contains(p.FMemberId));
            ViewBag.TotalFriends = totalItemFriends;

            //黑名單
            TMember member3 = _userInfoService.GetMemberInfo();
            var blackListIds = _context.TMemberRelations
                 .Where(x => x.FMemberId == member3.FMemberId && x.FMemberRelationStatusId == 2)
                 .Select(x => x.FMemberRelationStatusId);
            var totalItemBlackList = _context.TMembers.Count(p => blackListIds.Contains(p.FMemberId));
            ViewBag.TotalBlackList = totalItemBlackList;

            //場地數量
            TMember member4 = _userInfoService.GetMemberInfo();
            var sitesIds = _context.TSites.Where(x => x.FMemberId == member4.FMemberId).Select(x => x.FSiteId);
            var totalItemSites = _context.TSites.Count(p=>sitesIds.Contains(p.FSiteId));
            ViewBag.TotalSites = totalItemSites;

            //優惠券
            TMember member5 = _userInfoService.GetMemberInfo();
            var couponIds = _context.TMemberCoupons.Where(x => x.FMemberId == member5.FMemberId).Select(x => x.FMemberCouponId);
            var totalItemCoupons =_context.TMemberCoupons.Count(p=>couponIds.Contains(p.FMemberCouponId));
            ViewBag.TotalCoupons  = totalItemCoupons;


            var photo = (from m in _context.TMembers
                         where m.FMemberId == member.FMemberId
                         select new FMemberDto
                         {
                             FMemberID = member.FMemberId,
                             FPhotoPath = m.FPhotoPath,
                         }).FirstOrDefault();
            return View(photo);

        }
        public IActionResult MemberInfo()
        {
          

            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Login", "Account");

            }
            FMemberDto mem = (from m in _context.TMembers
                              where m.FMemberId == member.FMemberId
                              select new FMemberDto
                              {
                                  FMemberID = member.FMemberId,
                                  FName = m.FName,
                                  FBirthday = Convert.ToDateTime(m.FBirthday).ToString("yyyy-MM-dd"),
                                  FEmail = m.FEmail,
                                  FPhone = m.FPhone,
                                  FGender = m.FGender ? "男" : "女",
                                  FPassword = m.FPassword,
                                  FPhotoPath = m.FPhotoPath,
                              }).FirstOrDefault();
            ViewBag.MemberId = member.FMemberId; // 將會員ID添加到ViewBag

            return PartialView(mem);
        }
        public IActionResult MemberInfoEdit(int id)
        {
            var dto = _memberDao.GetFMemberById(id);
            if (dto != null)
            {
                FMemberEditVM vm = new FMemberEditVM()
                {
                    FMemberID = dto.fMemberID,
                    FName = dto.fName,
                    FUsername = dto.fUsername,
                    FBirthday = dto.fBirthday,
                    FEmail = dto.fEmail,
                    FGender = dto.fGender,
                    FPhone = dto.fPhone
                };
                return PartialView(vm);
            }
            else
            {
                // 處理 dto 為 null 的情況
                // 比如重定向到錯誤頁面或顯示一個錯誤消息
                return RedirectToAction("ErrorPage"); // 或者 return View("ErrorView");
            }
        }
        [HttpPost]
        public IActionResult MemberInfoEdit(FMemberEditVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode = 500, StatusMessage = errors.FirstOrDefault() };
                return Json(result);
            }
            var member = _context.TMembers.FirstOrDefault(m => m.FMemberId == vm.FMemberID);
            try
            {
                _service.FMemberEdit(vm);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);
            }
            catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);
            }
        }

        public IActionResult CoolPon()
        {
            return View();
        }

        //public async Task<IActionResult> Memberclass(int? id)
        public IActionResult Memberclass()
        {
            TMember member = _userInfoService.GetMemberInfo();
            ViewBag.MemberId = member.FMemberId;
            return View();
        }



        public IActionResult Create()
        {
            ViewData["FPermissionId"] = new SelectList(_context.TMemberPromissions, "FPromissionId", "FPromissionId");
            return View();
        }
        public IActionResult MemberPassword()
        {
            return PartialView();
        }
        [HttpPost]
        public IActionResult MemberPassword(MemberPasswordVM vm)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                result = new ApiResult { StatusCode = 500, StatusMessage = errors.FirstOrDefault() };
                return Json(result);
            }
            try
            {
                int loginMemId = _userInfoService.GetMemberInfo().FMemberId;
                _service.MemberPasswordReset(vm, loginMemId);
                result = new ApiResult { StatusCode = 200, StatusMessage = "編輯資料成功!" };
                return Json(result);

            }
            catch (Exception ex)
            {
                result = new ApiResult { StatusCode = 500, StatusMessage = ex.Message };
                return Json(result);

            }
        }
        public IActionResult MemberMyKeep()
        {
            TMember member = _userInfoService.GetMemberInfo();
            return PartialView(member);
        }

        //個人會員好友名單
        public async Task<IActionResult> Friends(string search, int page = 1, int pageSize = 10)
        {


            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // 假設狀態ID為1代表好友，為2代表黑名單
            var friendIdsQuery = _context.TMemberRelations
           .Where(x => x.FMemberId == member.FMemberId && x.FMemberRelationStatusId == 1)
           .Select(x => x.FRelationMemberId);

            var friendsQuery = _context.TMembers.Where(m => friendIdsQuery.Contains(m.FMemberId));

            if (!String.IsNullOrEmpty(search))
            {
                friendsQuery = friendsQuery.Where(s => s.FName.Contains(search) || s.FEmail.Contains(search) || s.FPhone.Contains(search));
            }

            // 先計算總數量
            var totalItems = await friendsQuery.CountAsync();

            // 應用分頁
            var pagedFriends = await friendsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new FriendsViewModel
            {
                Member = member,
                Friends = pagedFriends,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems,

                PageSize = pageSize
            };


            ViewData["CurrentFilter"] = search;
            return PartialView(viewModel);



        }

        private FriendsViewModel GetPagedFriends(int page, int pageSize)
        {
            // 假設 _context 是您的數據庫上下文
            var query = _context.TMembers.AsQueryable();

            // 計算總數量
            int totalItems = query.Count();

            // 應用分頁
            var friends = query
                .OrderBy(f => f.FName) // 或者根據您的需求選擇合適的排序方式
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new FriendsViewModel
            {
                Friends = friends,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return viewModel;
        }



        //個人會員黑名單
        public async Task<IActionResult> blackList(string search, int page = 1, int pageSize = 10)
        {
          
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var blackListIds = _context.TMemberRelations
            .Where(x => x.FMemberId == member.FMemberId && x.FMemberRelationStatusId == 2)
            .Select(x => x.FRelationMemberId);

            var blackListQuery =  _context.TMembers.Where(m => blackListIds.Contains(m.FMemberId));


            if (!String.IsNullOrEmpty(search))
            {
                blackListQuery = blackListQuery.Where(s => s.FName.Contains(search)
                                  || s.FEmail.Contains(search)
                                  || s.FPhone.Contains(search));
            }
            //先計算總數量
            var totalItems = await blackListQuery.CountAsync();

            //應用分頁
            var pagedBlackList = await blackListQuery.Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

           

            var viewModel = new FriendsViewModel
            {
                Member = member,
                BlackList = pagedBlackList,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems,               
                PageSize = pageSize,

            };
            ViewData["CurrentFilter"] = search;
            return PartialView(viewModel);
        }

        public async Task<IActionResult> MemberCoupon(string keyword)
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Members", "Index");
            }
            var membercouponIds = await _context.TMemberCoupons
                .Where(x => x.FMemberId == member.FMemberId)
                .Select(x => x.FCouponId)
                .ToListAsync();

            var query = _context.TCoupons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => c.FCouponContent.Contains(keyword));
            }

            var memberCoupons = await query.Where(c => membercouponIds.Contains(c.FCouponId))
                .ToListAsync();

            var viewModel = new MemberCouponVM
            {
                Member = member,
                Coupons = memberCoupons// 假设 MemberCouponVM 有一个名为 Coupons 的属性用于存储优惠券列表
            };
            return PartialView(viewModel);
        }

        public async Task<IActionResult> MemberSite(int? siteType)
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Members", "Index");
            }

            IQueryable<TSite> memberSitesQuery = _context.TSites.Where(x => x.FMemberId == member.FMemberId);

            if (siteType.HasValue)
            {
                memberSitesQuery = memberSitesQuery.Where(x => x.FSiteType == siteType.Value);
            }

            var memberSites = await memberSitesQuery.ToListAsync();
            var viewModel = new MemberSiteVM
            {
                Member = member,
                Sites = memberSites
            };
            return PartialView(viewModel);
        }
      


        public async Task<IActionResult> MemberWorks(string search ,int page = 1, int pageSize = 10, string sortBy = "newest")
        {
           
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Members", "Index");// 如果未找到會員，重定向到登入頁面
            }
          
            var memberworkIds = _context.TWorks
                .Where(x=>x.FMemberId==member.FMemberId)
                .Select(x=>x.FWorkId);

            var query = _context.TWorks.AsQueryable();
            if(!string.IsNullOrWhiteSpace(search)) 
            {
                query = query.Where(c => c.FWorkName.Contains(search));
            }

            // 添加排序邏輯
            if (sortBy == "newest")
            {
                query = query.OrderByDescending(c => c.FUpdateTime);
            }
            else if (sortBy == "oldest")
            {
                query = query.OrderBy(c => c.FUpdateTime);
            }


            var memberworks = query.Where(c=>memberworkIds.Contains(c.FWorkId))
                ;// 獲取該會員的所有作品

            //先計算總數量
            var totalItems = await memberworks.CountAsync();

            var pageWorkList = await memberworks.Skip((page-1)*pageSize).ToListAsync();

            var viewModel = new MemberWorksVM
            {
                Member = member,
               Works = pageWorkList,
                CurrentPage=page,
                TotalPages = (int)Math.Ceiling((double)totalItems/pageSize),
                TotalItems = totalItems,
                PageSize = pageSize,
                 // 假設 MemberWorksVM 有一個名為 Works 的屬性用來儲存作品列表
            };

            return PartialView(viewModel);
        }

        public async Task<IActionResult> MemberProject(string search, int statusId = 0,int page = 1, int pageSize = 10)
        {
            TMember member = _userInfoService.GetMemberInfo();
            if (member == null)
            {
                return RedirectToAction("Members", "Index");
            }

            var projectIds = _context.TProjects
                .Where(p => p.FMemberId == member.FMemberId)
                .Select(x=>x.FProjectId);

            var query = _context.TProjects.AsQueryable();

            if(!string.IsNullOrWhiteSpace(search))
            {
               query=query.Where(c =>c.FName.Contains(search));
            }

            if (statusId > 0)
            {
                query = query.Where(p => p.FProjectStatusId == statusId);
            }

            var memberProjects = query.Where(c =>projectIds.Contains(c.FProjectId));

            var totalItems = await memberProjects.CountAsync();

            var pageProjectList = await memberProjects.Skip((page - 1) * pageSize).ToListAsync();

            var viewModel = new MemberProjectVM
            {
                Member = member,
                Project = pageProjectList,
                CurrentPage=page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                TotalItems = totalItems,
                PageSize = pageSize,
            };
            return PartialView(viewModel);
        }  
      
    }
}



