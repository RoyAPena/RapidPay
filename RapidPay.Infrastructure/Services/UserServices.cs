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

        public async Task<string> CreateUserAsync(string userName, string password)
        {
            var user = new IdentityUser { UserName = userName };
            await _userManager.CreateAsync(user, password);
            return user.Id;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent: false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task<bool> UsernameExists(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            return user != null;
        }

        public async Task<bool> ValidatePassword(string password)
        {
            var passwordValidator = new PasswordValidator<IdentityUser>();
            var validationResults = await passwordValidator.ValidateAsync(_userManager, new IdentityUser(), password);

            return validationResults.Succeeded;
        }
    }
}