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
using System.Net.Mail;
using System.Net;
using prjMusicBetter.Models.Services;
using Stripe;






var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
//AspNetCore.Authentication �Τ����Ҿާ@������UDI
builder.Services.AddHttpContextAccessor();
//�ۭq�Τ�n�J��T�ާ@���UDI
builder.Services.AddScoped<UserInfoService>();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();



//�إ߸�Ʈw�s���ݭn�S�O�[�o�@�q
builder.Services.AddDbContext<dbSoundBetterContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbSoundBetterConnection")
));

//Chart.js
builder.Services.AddScoped<MemberDataService>();

//�إ߲�ѫǳs���ݭn�S�O�[�o�@�q
builder.Services.AddSignalR();

// �W�[Session �W�L1�p�ɷ|�Q�M��
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});


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

//Stripe
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

//================= AspNetCore.Authentication �Τ�n�J���Ҿާ@����ϥ�=====
//���涶�Ǥ����A�ˤ��M���ҥ\��|�L�k���`�u�@�C
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
//================= AspNetCore.Authentication �Τ�n�J���Ҿާ@����ϥ�======

// �ϥ�Session
app.UseSession();


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
