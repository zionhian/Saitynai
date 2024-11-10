using Microsoft.AspNetCore.Identity;
using SaitynaiBackend.Auth;
using SaitynaiBackend.Data.Models;

namespace SaitynaiBackend.Data
{
    public class AuthDbSeeder
    {
        private readonly UserManager<StoreUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthDbSeeder(UserManager<StoreUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }
        private async Task AddAdminUser()
        {
            var newAdminUser = new StoreUser()
            {
                //Discriminator = "administrator",
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            var existingAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if (existingAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "SecurePassword123!");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, UserStoreRoles.All);
                }
            }
        }
        private async Task AddDefaultRoles()
        {
            foreach (var role in UserStoreRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
