using Microsoft.AspNetCore.Identity;
using RapidPay.Application.Abstractions;

namespace RapidPay.Infrastructure.Services
{
    internal class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserServices(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded;
        }
    }
}