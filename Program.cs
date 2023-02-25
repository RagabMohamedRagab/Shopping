using Bookstore.IRepositories;
using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Utilities.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
#region Identity
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddDbContext<ApplicationDbContext>(option =>
                         option.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddMvc(option =>
{
    var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
    option.Filters.Add(new AuthorizeFilter(policy));
})
.AddXmlSerializerFormatters().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    PreventDuplicates = true,
    CloseButton = true,
    ShowDuration=10
});
builder.Services.AddAuthentication().AddGoogle(option =>
{
    option.ClientId = "832775448183-o2gcjqfprp16l2pntl9equrapp55m8em.apps.googleusercontent.com";
    option.ClientSecret = "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";
})
 .AddFacebook(option =>
    {
        option.AppId = "1391050401708955";
        option.AppSecret = "4c6ac5655e29738e8948c94c81fccbd1";
    });
builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireUppercase = false;
    options.Password.RequiredUniqueChars = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
});
#endregion 

#region regsiter Services
builder.Services.AddTransient(typeof(IGeneralRepsitory<>), typeof(GeneralRepsitory<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IMailingService, MailingService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movie}/{action=Index}/{id?}");

app.Run();

