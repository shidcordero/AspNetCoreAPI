using API.Extensions;
using API.Utilities;
using Data.Models.Entities;
using Data.ViewModels.Account;
using Data.ViewModels.Common;
using Domain.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        /// <summary>
        /// Post request for User Login
        /// </summary>
        /// <param name="loginViewModel">Login user data</param>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]LoginViewModel loginViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _accountService.SignIn(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe);
                    if (result == SignInResult.Success)
                    {
                        var user = await _accountService.FindByNameAsync(loginViewModel.Username);
                        var authClaims = new[]
{
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                            };

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecureKey")));

                        var token = new JwtSecurityToken(
                            issuer: "http://dotnetdetail.net",
                            audience: "http://dotnetdetail.net",
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    } 
                    else
                    {
                        ModelState.AddModelError(new ValidationResult("Incorrect Username & Password"));
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            return BadRequest(ModelState.GetErrors());
        }

        /// <summary>
        /// Post request for Registration
        /// </summary>
        /// <param name="registerViewModel">Register user data</param>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new AppUser
                    {
                        UserName = registerViewModel.Username,
                        Email = registerViewModel.Email,
                        FirstName = registerViewModel.FirstName,
                        LastName = registerViewModel.LastName
                    };

                    var result = await _accountService.Register(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        return Ok("Registered successfully!");
                    }

                    ModelState.AddIdentityErrors(result);
                }
            }
            catch (Exception ex)
            {
                var exceptionMessage = await Helpers.GetErrors(ex);
                ModelState.AddModelError(new ValidationResult(exceptionMessage));
            }

            ModelState.AddModelError(new ValidationResult("Invalid register attempt."));
            return BadRequest(ModelState.GetErrors());
        }


    }
}