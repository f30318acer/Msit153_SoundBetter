using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
//AspNetCore.Authentication 用戶驗證操作機制註冊DI
builder.Services.AddHttpContextAccessor();
//自訂用戶登入資訊操作註冊DI
builder.Services.AddScoped<UserInfoService>();

//建立資料庫連接需要特別加這一段
builder.Services.AddDbContext<dbSoundBetterContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbSoundBetterConnection")
));

//登入cookie 要加這一段才能使用
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時會自動移轉到此網頁
    option.LoginPath = new PathString("/Home/Login");

    option.AccessDeniedPath = new PathString("/Home/NoRole");

    option.ExpireTimeSpan = TimeSpan.FromDays(1);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//================= AspNetCore.Authentication 用戶登入驗證操作機制使用=====
//執行順序不能顛倒不然驗證功能會無法正常工作。
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//================= AspNetCore.Authentication 用戶登入驗證操作機制使用======



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
