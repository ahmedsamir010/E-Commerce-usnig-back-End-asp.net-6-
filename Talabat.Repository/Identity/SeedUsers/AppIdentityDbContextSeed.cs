using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.SeedUsers
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            // Check if there are any existing users
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName = "Ahmed Samir",
                        PhoneNumber = "01030094711",
                        UserName = "user1@example.com",
                        Email = "user1@example.com",
                    },
                    new AppUser
                    {
                        DisplayName = "Ahmed Samir",
                        PhoneNumber = "01030094711",
                        UserName = "user2@example.com",
                        Email = "user2@example.com",
                    }
                    // Add more users as needed
                };

                foreach (var user in users)
                {
                    var existingUser = await userManager.FindByEmailAsync(user.Email);
                    if (existingUser == null)
                    {
                        var result = await userManager.CreateAsync(user, "YourPassword123!"); // Set a secure password for each user

                        if (!result.Succeeded)
                        {
                            // Failed to create the user, handle the error
                            foreach (var error in result.Errors)
                            {
                                // Log or handle the error appropriately
                            }
                        }
                    }
                }
            }
        }
    }
}
