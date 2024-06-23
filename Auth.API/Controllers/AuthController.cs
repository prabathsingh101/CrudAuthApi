using Auth.API.Data;
using Auth.API.Models.Domain;
using Auth.API.Models.DTO;
using Auth.API.Repositories.Abstract;
using Auth.API.Repositories.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenService _tokenService;
        private readonly IUserService userService;

        public AuthController(DatabaseContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService, IUserService userService
            )
        {
            this._context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._tokenService = tokenService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModelDto model)
        {
            var status = new StatusDto();
            // check validations
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "please pass all the valid fields";
                return Ok(status);
            }
            // lets find the user
            var user = await userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                status.StatusCode = 0;
                status.Message = "invalid username";
                return Ok(status);
            }
            // check current password
            if (!await userManager.CheckPasswordAsync(user, model.CurrentPassword))
            {
                status.StatusCode = 0;
                status.Message = "invalid current password";
                return Ok(status);
            }

            // change password here
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "Failed to change password";
                return Ok(status);
            }
            status.StatusCode = 1;
            status.Message = "Password has changed successfully";
            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDto model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = _tokenService.GetToken(authClaims);
                var refreshToken = _tokenService.GetRefreshToken();
                var tokenInfo = _context.TokenInfo.FirstOrDefault(a => a.Usename == user.UserName);
                if (tokenInfo == null)
                {
                    var info = new TokenInfo
                    {
                        Usename = user.UserName,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiry = DateTime.Now.AddHours(12)
                    };
                    _context.TokenInfo.Add(info);
                }

                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.RefreshTokenExpiry = DateTime.Now.AddHours(12);
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok(new LoginResponseDto
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Token = token.TokenString,
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                    StatusCode = 1,
                    Message = "Logged in"
                });

            }
            //login failed condition

            return Ok(
                new LoginResponseDto
                {
                    StatusCode = 0,
                    Message = "Invalid Username or Password",
                    Token = "",
                    Expiration = null
                });
        }

        [HttpPost]
        [Route("Registration-User")]
        public async Task<IActionResult> Registration([FromBody] RegistrationModelDto model)
        {
            var status = new StatusDto();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Please pass all the required fields";
                return Ok(status);
            }
            // check if user exists
            //var userExists = await userManager.FindByNameAsync(model.Username);
            //if (userExists != null)
            //{
            //    status.StatusCode = 0;
            //    status.Message = "Username has already taken.";
            //    return Ok(status);
            //}
            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                status.StatusCode = 0;
                status.Message = "Email-id has already taken.";
                return Ok(status);
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = model.Email,
                Name = model.Name
            };
            // create a user here
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return Ok(status);
            }

            // add roles here
            // for admin registration UserRoles.Admin instead of UserRoles.Roles
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await roleManager.RoleExistsAsync(UserRoles.User))
                await userManager.AddToRoleAsync(user, UserRoles.User);


            //This condition will be used on the providing role by UI
            //if (model.Roles != null && model.Roles.Any())
            //{
            //    foreach (var role in model.Roles)
            //    {
            //        await roleManager.CreateAsync(new IdentityRole(role));
            //        await userManager.AddToRoleAsync(user, role);
            //    }
            //}

            status.StatusCode = 1;
            status.Message = "Sucessfully registered";
            return Ok(status);

        }

        // after registering admin we will comment this code, because i want only one admin in this application
        [HttpPost]
        [Route("Registration-Admin")]
        public async Task<IActionResult> RegistrationAdmin([FromBody] RegistrationModelDto model)
        {
            if (ModelState.IsValid)
            {
                var status = new StatusDto();
                if (!ModelState.IsValid)
                {
                    status.StatusCode = 0;
                    status.Message = "Please pass all the required fields";
                    return Ok(status);
                }
                // check if user exists
                //var userExists = await userManager.FindByNameAsync(model.Username);
                //if (userExists != null)
                //{
                //    status.StatusCode = 0;
                //    status.Message = "Username has already taken";
                //    return Ok(status);
                //}
                var emailExists = await userManager.FindByEmailAsync(model.Email);
                if (emailExists != null)
                {
                    status.StatusCode = 0;
                    status.Message = "Email has already taken";
                    return Ok(status);
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    Name = model.Name
                };
                // create a user here
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    status.StatusCode = 0;
                    status.Message = "User creation failed";
                    return Ok(status);
                }

                // add roles here
                // for admin registration UserRoles.Admin instead of UserRoles.Roles
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                status.StatusCode = 1;
                status.Message = "Sucessfully registered";
                return Ok(status);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Refresh")]
        public IActionResult Refresh(RefreshTokenRequestDto tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var user = _context.TokenInfo.SingleOrDefault(u => u.Usename == username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GetToken(principal.Claims);
            var newRefreshToken = _tokenService.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            return Ok(new RefreshTokenRequestDto()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            });
        }

        //revoken is use for removing token enntry
        [HttpPost, Authorize]
        [Route("Revoke")]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.Identity.Name;
                var user = _context.TokenInfo.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest();
                user.RefreshToken = null;
                _context.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("GetName")]
        [Authorize(Roles ="Admin")]
        public IActionResult GetName()
        {
            var userName = userService.GetMyName();
            return Ok(userName);
        }

        [HttpGet]
        [Route("GetRoleName")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoleName()
        {
            var userName = userService.GetRoleName();
            return Ok(userName);
        }

    }
}
