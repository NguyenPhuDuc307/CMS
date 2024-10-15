using CMS.Data.Entities.Systems;
using CMS.Extensions;
using CMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers;

public class UserSettingController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSettingRepository _userSettingRepository;

    public UserSettingController(IUserRepository userRepository, IUserSettingRepository userSettingRepository)
    {
        _userRepository = userRepository;
        _userSettingRepository = userSettingRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateUserSetting([FromBody] UserSetting model)
    {
        var userSetting = await _userSettingRepository.GetUserSettingByUserIdAsync(User.GetUserId());

        if (userSetting != null)
        {
            userSetting.Theme = model.Theme;
            userSetting.Layout = model.Layout;
            userSetting.SidebarType = model.SidebarType;
            await _userSettingRepository.UpdateUserSettingAsync(userSetting);
        }

        return Ok(new { success = true, message = "User settings updated successfully." });
    }

}