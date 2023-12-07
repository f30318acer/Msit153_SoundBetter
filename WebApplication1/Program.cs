using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using prjMusicBetter.Models;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.EFModels;
using prjMusicBetter.Models.infra;
using prjMusicBetter.Models.ViewModels;
using CoreMVC_SignalR_Chat.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;




var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
//AspNetCore.Authentication �Τ����Ҿާ@������UDI
builder.Services.AddHttpContextAccessor();
//�ۭq�Τ�n�J��T�ާ@���UDI
builder.Services.AddScoped<UserInfoService>();

//�إ߸�Ʈw�s���ݭn�S�O�[�o�@�q
builder.Services.AddDbContext<dbSoundBetterContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbSoundBetterConnection")
));

//�إ߲�ѫǳs���ݭn�S�O�[�o�@�q
builder.Services.AddSignalR();


//�n�Jcookie �n�[�o�@�q�~��ϥ�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //���n�J�ɷ|�۰ʲ���즹����
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

app.UseRouting();


//================= AspNetCore.Authentication �Τ�n�J���Ҿާ@����ϥ�=====
//���涶�Ǥ����A�ˤ��M���ҥ\��|�L�k���`�u�@�C
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//================= AspNetCore.Authentication �Τ�n�J���Ҿާ@����ϥ�======



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // ... ��L���I�t�m ...
});
app.UseStaticFiles();


//app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
