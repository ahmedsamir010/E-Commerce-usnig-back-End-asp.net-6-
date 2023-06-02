using AdminPanal.Helpers;
using API_01.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Identity;

namespace AdminPanal.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [RequireAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return RedirectToAction(nameof(Index));
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password,true,false);
            if (!result.Succeeded || !await _userManager.IsInRoleAsync(user,"Admin"))
            {
                ModelState.AddModelError(string.Empty, "Yao are not Authorize");
            
                return RedirectToAction(nameof(Login));
            }
            else
            {
                return RedirectToAction("Index" , "Home");
            }
        }  
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }


    }
}
