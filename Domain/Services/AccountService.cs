using Data.Models.Entities;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        /// <summary>
        /// Used for user sign-in
        /// </summary>
        /// <param name="username">holds the user username</param>
        /// <param name="password">holds the user password</param>
        /// <param name="rememberMe">holds the remember me data</param>
        /// <param name="lockOutPeriod">holds the lockout period</param>
        /// <returns>SignInResult</returns>
        public async Task<SignInResult> SignIn(string username, string password, bool rememberMe, bool lockOutPeriod = false)
        {
            return await _signInManager.PasswordSignInAsync(username, password, rememberMe, false);
        }

        /// <summary>
        /// Used to sign-out user
        /// </summary>
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Used to Register a user
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="password">holds the user password</param>
        /// <returns>IndentityResult</returns>
        public async Task<IdentityResult> Register(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Used to find user by email
        /// </summary>
        /// <param name="username">Holds the user username</param>
        /// <returns>User data</returns>
        public async Task<AppUser> FindByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// Used to reset user password
        /// </summary>
        /// <param name="user">holds the user data</param>
        /// <param name="code">holds the user code</param>
        /// <param name="password">holds the user password</param>
        /// <returns>IdentityResult</returns>
        public async Task<IdentityResult> ResetPassword(AppUser user, string code, string password)
        {
            return await _userManager.ResetPasswordAsync(user, code, password);
        }
    }
}