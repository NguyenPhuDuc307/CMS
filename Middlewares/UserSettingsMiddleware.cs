using CMS.Data.Entities.Systems;
using CMS.Repositories;
using CMS.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace CMS.Middlewares;

public class UserSettingsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UserSettingsMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity!.IsAuthenticated)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userSettingRepository = scope.ServiceProvider.GetRequiredService<IUserSettingRepository>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var userId = userManager.GetUserId(context.User);
                var userSettings = await userSettingRepository.GetUserSettingByUserIdAsync(userId!);

                if (userSettings != null)
                {
                    var userSettingViewModel = new UserSettingViewModel(
                        userSettings.Theme,
                        userSettings.Direction,
                        userSettings.ColorTheme,
                        userSettings.Layout,
                        userSettings.BoxedLayout,
                        userSettings.SidebarType,
                        userSettings.CardBorder);

                    var userSettingsJson = JsonConvert.SerializeObject(userSettingViewModel);

                    // Lưu UserSettings vào cookie
                    context.Response.Cookies.Append("UserSettings", userSettingsJson, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = true, // Chỉ bật Secure trong môi trường production
                        Expires = DateTimeOffset.UtcNow.AddDays(30)
                    });
                }
            }
        }

        await _next(context);
    }
}