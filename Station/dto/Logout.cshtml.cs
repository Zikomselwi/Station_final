﻿//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//public class LogoutModel : PageModel
//{
//    private readonly SignInManager<IdentityUser> _signInManager;
//    private readonly ILogger<LogoutModel> _logger;

//    public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
//    {
//        _signInManager = signInManager;
//        _logger = logger;
//    }

//    public async Task<IActionResult> Logout()
//    {
//        await _signInManager.SignOutAsync();
//        return RedirectToAction("Login");
//    }
//}
