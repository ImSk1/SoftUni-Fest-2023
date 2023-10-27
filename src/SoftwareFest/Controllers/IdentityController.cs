﻿using SoftwareFest.Services.Contracts;

namespace SoftwareFest.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SoftwareFest.Models;
    using SoftwareFest.ViewModels;
    using SofwareFest.Infrastructure;

    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;

        public IdentityController (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, 
            ILogger<IdentityController> logger, 
            IMapper mapper,
            IBusinessService businessService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _businessService = businessService;
        }

        [HttpGet("/login")]
        public IActionResult Login()
            => View();

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
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

        [HttpGet("/register/business")]
        public IActionResult BusinessRegister() 
            => View();
        [HttpGet("/register/client")]
        public IActionResult ClientRegister()
            => View();

        [HttpPost("/register/business")]
        public async Task<IActionResult> BusinessRegister(BusinessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await Register(model);
            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {0}", string.Join(", ", result.Errors));

                return View(model);
            }
            _logger.LogInformation("User {0} registered successfully.", model.Email);

            await _businessService.CreateBusiness(model);

            _logger.LogInformation("Business {0} created successfully.", model.BusinessName);

            return RedirectToAction(nameof(Login));
        }

        [NonAction]
        public async Task<IdentityResult> Register(UserViewModel model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Signing out {0}", User?.Identity?.Name);
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
