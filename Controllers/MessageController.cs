using CMS.Data.Entities.Systems;
using CMS.Extensions;
using CMS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers;

[Authorize]
public class MessageController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly SignInManager<User> _signInManager;

    public MessageController(IUserRepository userRepository, IRoleRepository roleRepository, SignInManager<User> signInManager)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _signInManager = signInManager;
    }

    [Route("message")]
    public async Task<IActionResult> Index()
    {
        var user = await _userRepository.GetUserByIdAsync(User.GetUserId());
        return View(user);
    }
}