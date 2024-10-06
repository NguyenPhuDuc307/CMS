using System.Security.Claims;
using CMS.Data;
using CMS.Data.Entities.Systems;
using CMS.Repositories;
using CMS.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CMS.Authorization;
public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly IPermissionRepository _permissionRepository;

    public CustomUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ApplicationDbContext context,
        IPermissionRepository permissionRepository,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _permissionRepository = permissionRepository;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var permissions = await _permissionRepository.GetPermissionsByRolesAsync(roles.ToList());

        // Thêm claim Permissions vào ClaimsIdentity
        identity.AddClaim(new Claim(SystemConstants.Claims.Permissions, JsonConvert.SerializeObject(permissions)));
        identity.AddClaim(new Claim(SystemConstants.Claims.FirstName, user.UserName!));
        identity.AddClaim(new Claim(SystemConstants.Claims.Roles, JsonConvert.SerializeObject(roles)));

        return identity;
    }
}
