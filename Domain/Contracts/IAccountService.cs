using System.Security.Claims;
using Data.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAccountService
    {
        Task<SignInResult> SignIn(string username, string password, bool rememberMe, bool lockOutPeriod = false);

        Task SignOut();

        Task<IdentityResult> Register(AppUser user, string password);

        Task<AppUser> FindByNameAsync(string username);
    }
}