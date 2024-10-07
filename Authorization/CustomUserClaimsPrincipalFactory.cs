using System.Security.Claims;
using CMS.Data.Entities.Systems;
using CMS.Repositories;
using CMS.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CMS.Models.Systems;

namespace CMS.Authorization;
public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    private readonly UserManager<User> _userManager;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserSettingRepository _userSettingRepository;

    public CustomUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IPermissionRepository permissionRepository,
        IUserSettingRepository userSettingRepository,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _userManager = userManager;
        _permissionRepository = permissionRepository;
        _userSettingRepository = userSettingRepository;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var permissions = await _permissionRepository.GetPermissionsByRolesAsync(roles.ToList());
        var userSettings = await _userSettingRepository.GetUserSettingByUserIdAsync(user.Id);
        if (userSettings != null)
        {
            var userSettingViewModel = new UserSettingViewModel(userSettings.Theme, userSettings.Direction, userSettings.Color, userSettings.Layout, userSettings.BoxLayout, userSettings.Sidebar, userSettings.CardBorder);
            identity.AddClaim(new Claim(SystemConstants.Claims.UserSettings, JsonConvert.SerializeObject(userSettingViewModel)));
        }
        // Thêm claim Permissions vào ClaimsIdentity
        identity.AddClaim(new Claim(SystemConstants.Claims.Permissions, JsonConvert.SerializeObject(permissions)));
        identity.AddClaim(new Claim(SystemConstants.Claims.FirstName, user.UserName!));
        identity.AddClaim(new Claim(SystemConstants.Claims.Roles, JsonConvert.SerializeObject(roles)));

        return identity;
    }
}
