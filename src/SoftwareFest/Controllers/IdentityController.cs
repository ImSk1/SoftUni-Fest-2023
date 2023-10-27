﻿using SoftwareFest.MailSending;
using SoftwareFest.Services.Contracts;

namespace SoftwareFest.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using SoftwareFest.Models;
    using SoftwareFest.ViewModels;
    using SofwareFest.Infrastructure;
    using Stripe;
    using System.Text;

    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IdentityController> _logger;
        private readonly IMapper _mapper;
        private readonly IBusinessService _businessService;
        private readonly IClientService _clientService;
        private readonly IMailSender _mailSender;
        public IdentityController (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context, 
            ILogger<IdentityController> logger, 
            IMapper mapper,
            IBusinessService businessService,
            IClientService clientService,
            IMailSender mailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _businessService = businessService;
            _clientService = clientService;
            _mailSender = mailSender;
        }

        [HttpGet("/login")]
        public IActionResult Login()
            => View();

        [HttpPost("/login")]
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

        [HttpGet("/register/business")]
        public IActionResult BusinessRegister() 
            => View();
        [HttpGet("/register/client")]
        public IActionResult ClientRegister()
            => View();

        [HttpPost("/register/client")]
        public async Task<IActionResult> ClientRegister(ClientViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await Register(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {0}", string.Join(", ", result.Errors));

                return View(model);
            }

            await SendEmailConfirmation(user);

            _logger.LogInformation("User {0} registered successfully.", model.Email);

            await _clientService.CreateClient(model);

            _logger.LogInformation("Client Profile {0} created successfully.", model.FirstName + " " + model.LastName);

            return RedirectToAction(nameof(Login));
        }

        [HttpPost("/register/business")]
        public async Task<IActionResult> BusinessRegister(BusinessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await Register(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {0}", string.Join(", ", result.Errors));

                return View(model);
            }

            await SendEmailConfirmation(user);
            _logger.LogInformation("User {0} registered successfully.", model.Email);

            await _businessService.CreateBusiness(model);

            _logger.LogInformation("Business {0} created successfully.", model.BusinessName);

            return RedirectToAction(nameof(Login));
        }

        [NonAction]
        public async Task<IdentityResult> Register(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }
        [NonAction]
        public async Task SendEmailConfirmation(ApplicationUser user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            string callbackUrl = Url.Action(nameof(ConfirmEmail), "Identity", new { user.Id, token }, Request.Scheme)!;

            await _mailSender.SendEmailAsync(new MailMessage() { To = user.Email, Subject = "Email Confirmation for PaySol", Content = callbackUrl });
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Signing out {0}", User?.Identity?.Name);
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string? id, string? token)
        {
            if (id == null || token == null)
            {
                return RedirectToAction(nameof(Login), "Identity");
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            IdentityResult result = await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(id), code);

            TempData["StatusMessage"] = result.Succeeded ? "Thank you for confirming your email." : "An error occurred while trying to confirm your email";

            return RedirectToAction("Login", "Identity");
        }
    }
}
