using CMS.Data.Entities.Systems;
using CMS.Extensions;
using CMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers;

public class UserSettingController : Controller
{
    private readonly IUserSettingRepository _userSettingRepository;

    public UserSettingController(IUserSettingRepository userSettingRepository)
    {
        _userSettingRepository = userSettingRepository;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateUserSetting([FromBody] UserSetting model)
    {
        var userId = User.GetUserId();
        var userSetting = await _userSettingRepository.GetUserSettingByUserIdAsync(userId);

        if (userSetting == null)
        {
            userSetting = new UserSetting();

            await _userSettingRepository.CreateUserSettingAsync(userSetting);
        }
        else
        {
            userSetting.Layout = model.Layout;
            userSetting.SidebarType = model.SidebarType;
            userSetting.BoxedLayout = model.BoxedLayout;
            userSetting.Direction = model.Direction;
            userSetting.Theme = model.Theme;
            userSetting.ColorTheme = model.ColorTheme;
            userSetting.CardBorder = model.CardBorder;

            await _userSettingRepository.UpdateUserSettingAsync(userSetting);
        }

        return Ok(new { success = true, message = "User settings updated successfully." });
    }

}