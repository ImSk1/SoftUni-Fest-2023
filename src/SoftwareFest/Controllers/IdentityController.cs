using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareFest.Models;
using SoftwareFest.ViewModels;
using SofwareFest.Infrastructure;

namespace SoftwareFest.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;

        public IdentityController
            (UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, 
            ILogger<IdentityController> logger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                _logger.LogError($"Login failed for user {user.UserName}.");

                return View(model);
            }

            _logger.LogInformation($"User {user.UserName} logged in successfully.");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {0}", string.Join(", ", result.Errors));

                return View(model);
            }

            _logger.LogInformation("User {0} registered successfully.", model.Email);

            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Signing out {0}", User?.Identity?.Name);

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
