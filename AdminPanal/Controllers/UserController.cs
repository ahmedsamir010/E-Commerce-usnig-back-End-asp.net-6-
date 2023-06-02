using AdminPanal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace AdminPanal.Controllers
{
	[Authorize(Roles ="Admin")]
	public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Roles = roles.ToList()
                };

                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var model = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var rolesToRemove = new List<string>();
            var rolesToAdd = new List<string>();

            if (model.Roles != null)
            {
                foreach (var role in model.Roles)
                {
                    if (role.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        rolesToAdd.Add(role.Name);
                    }
                    else if (!role.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        rolesToRemove.Add(role.Name);
                    }
                }
            }

            // Add roles to the user
            foreach (var role in rolesToAdd)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            // Remove roles from the user
            foreach (var role in rolesToRemove)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            return RedirectToAction("Index");
        }

    }
}
