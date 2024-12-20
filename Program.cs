using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Data.Entities.Systems;
using CMS.Repositories;
using CMS.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using CMS.Utils.ConfigOptions.VNPay;
using CMS.Utils.ConfigOptions.GoogleCloud;
using CMS.Utils.ConfigOptions.Email;
using Serilog;
using static CMS.Utils.Constants.SystemConstants;
using CMS.Authorization;
using Microsoft.AspNetCore.Identity;
using CMS.Helpers;
using CMS.Middlewares;
using CMS.Hubs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

int optionDatabases = 1;

switch (optionDatabases)
{
    case 1:
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
        }
        break;
    case 2:
        {
            var connectionString = builder.Configuration.GetConnectionString("SQLServerConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
        break;
}

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

services.AddControllersWithViews();

// Cấu hình Razor Pages cho Identity
RouteRazerPage();

#region Authorization

AddAuthorizationPolicies();

#endregion

services.AddAuthentication()
.AddGoogle(googleOptions =>
{
    var googleClientId = configuration.GetSection("Authentication:Google:ClientId").Value;
    var googleClientSecret = configuration.GetSection("Authentication:Google:ClientSecret").Value;

    if (googleClientId != null && googleClientSecret != null)
    {
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
        googleOptions.ClaimActions.MapJsonKey("image", "picture");
        googleOptions.Scope.Add("email");
        googleOptions.SaveTokens = true;

        googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
            return Task.CompletedTask;
        };
    }
})
.AddFacebook(facebookOptions =>
{
    var facebookAppId = configuration.GetSection("Authentication:Facebook:AppId").Value;
    var facebookAppSecret = configuration.GetSection("Authentication:Facebook:AppSecret").Value;

    if (facebookAppId != null && facebookAppSecret != null)
    {
        facebookOptions.AppId = facebookAppId;
        facebookOptions.AppSecret = facebookAppSecret;
        facebookOptions.Scope.Add("public_profile");
        facebookOptions.Scope.Add("email");
        facebookOptions.Scope.Add("user_likes");
        facebookOptions.SaveTokens = true;

        facebookOptions.Events.OnTicketReceived = async context =>
        {
            var accessToken = context.Properties!.GetTokenValue("access_token");
            if (accessToken != null)
            {
                context.Properties!.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = accessToken } });
            }

            await Task.CompletedTask;
        };
    }
});

services.AddSignalR();
AddScoped();

services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePages(appError =>
{
    appError.Run(async context =>
    {
        using (var scope = app.Services.CreateScope()) // Tạo scope mới
        {
            var response = context.Response;
            var code = response.StatusCode;

            var _viewRenderService = scope.ServiceProvider.GetRequiredService<IViewRenderService>();

            var htmlContent = await _viewRenderService.RenderToStringAsync("_StatusCodePages", code);

            await response.WriteAsync(htmlContent);
        }
    });
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

CustomMiddleware();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chatHub");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    var serviceProvider = scope.ServiceProvider;
    try
    {
        Log.Information("Seeding data...");
        var dbInitializer = serviceProvider.GetService<DbInitializer>();
        if (dbInitializer != null)
            dbInitializer.Seed()
                         .Wait();
    }
    catch (Exception ex)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();


void AddAuthorizationPolicies()
{
    services.AddAuthorization(options =>
    {
        options.AddPolicy(Policies.RequireAdmin, policy => policy.RequireRole(Roles.Administrator));
        options.AddPolicy(Policies.RequireManager, policy => policy.RequireRole(Roles.Manager));
    });
}

void AddScoped()
{
    services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();

    services.AddTransient<DbInitializer>();
    services.AddTransient<UnitOfWork>();
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddScoped<IRoleRepository, RoleRepository>();
    services.AddScoped<IPermissionRepository, PermissionRepository>();
    services.AddScoped<IFunctionRepository, FunctionRepository>();
    services.AddScoped<IUserSettingRepository, UserSettingRepository>();

    services.AddTransient<IEmailSender, EmailSenderService>();
    services.AddTransient<IVNPayService, VNPayService>();
    services.AddTransient<IStorageService, FileStorageService>();
    services.Configure<GoogleCloudStorageSettings>(configuration.GetSection("GoogleCloudStorageSettings"));
    services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
    services.Configure<VNPayConfigOptions>(builder.Configuration.GetSection("VnPay"));
    services.AddTransient<IViewRenderService, ViewRenderService>();
    services.AddTransient<ICacheService, DistributedCacheService>();
    services.AddTransient<ICloudStorageService, CloudStorageService>();
    services.AddTransient<IFileValidator, FileValidator>();
    services.AddHttpClient<FacebookService>();
}

void RouteRazerPage()
{
    services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/access-denied";
    });
    services.AddRazorPages(options =>
    {
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "login");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "logout");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", "access-denied");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "register");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ForgotPassword", "forgot-password");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ResetPassword", "reset-password");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ConfirmEmail", "confirm-email");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/ResetPasswordConfirmation", "reset-password-confirmation");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/LogoutConfirmation", "logout-confirmation");

        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "manager/change-password");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/DeletePersonalData", "manager/delete-personal-data");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Disable2fa", "manager/disable2fa");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/DownloadPersonalData", "manager/download-personal-data");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Email", "manager/email");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/EnableAuthenticator", "manager/enable-authenticator");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ExternalLogins", "manager/external-logins");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/GenerateRecoveryCodes", "manager/generate-recovery-codes");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "manager");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/PersonalData", "manager/personal-data");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ResetAuthenticator", "manager/reset-authenticator");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/SetPassword", "manager/set-password");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ShowRecoveryCodes", "manager/show-recovery-codes");
        options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/TwoFactorAuthentication", "manager/two-factor-authentication");
    });
}

void CustomMiddleware()
{
    app.UseMiddleware<UserSettingsMiddleware>();
}
