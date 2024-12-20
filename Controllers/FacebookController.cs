using CMS.Extensions;
using CMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers;

public class FacebookController : Controller
{
    private readonly FacebookService _facebookService;

    public FacebookController(FacebookService facebookService)
    {
        _facebookService = facebookService;
    }

    public async Task<IActionResult> UserLikes()
    {
        // Attempt to retrieve the Facebook access token from ClaimsPrincipal
        var accessToken = "";
        foreach (var claim in HttpContext.User.Claims)
        {
            Console.WriteLine($"{claim.Type}: {claim.Value}");
        }

        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Unable to retrieve Facebook access token. Please log in.");
        }

        // Gọi Facebook API để lấy danh sách bài viết đã thích
        var likes = await _facebookService.GetUserLikesAsync(accessToken);
        return View(likes);
    }
}
