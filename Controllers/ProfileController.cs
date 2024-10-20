using CMS.Authorization;
using CMS.Data.Entities.Systems;
using CMS.Extensions;
using CMS.ViewModels.Systems;
using CMS.Repositories;
using CMS.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly SignInManager<User> _signInManager;
    public ProfileController(IUserRepository userRepository, IRoleRepository roleRepository, SignInManager<User> signInManager)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    [Route("profile")]
    public async Task<IActionResult> Index()
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        return View(user);
    }
}