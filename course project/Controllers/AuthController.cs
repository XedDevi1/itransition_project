using course_project.Constants;
using course_project.Dtos;
using course_project.Models;
using course_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace course_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthController(SignInManager<User> signInManager,
            TokenService tokenService,
            RefreshTokenService refreshTokenService,
            RoleManager<IdentityRole<int>> roleManager,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _roleManager = roleManager;
        }

        [HttpPost("singup")]
        public async Task<IActionResult> SingUpAsync(SingUpDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                UserName = dto.Username,
            };

            var singUpResult = await _userManager.CreateAsync(user, dto.Password);
            if (!singUpResult.Succeeded)
            {
                return BadRequest(singUpResult.Errors);
            }

            return Ok();
        }

        [HttpPost("singin")]
        public async Task<ActionResult<SingInResultDto>> SingInAsync(SingInDto dto)
        {
            var normalizedSingInInfo = dto.Username.ToUpper();
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.NormalizedUserName == normalizedSingInInfo ||
                                            u.NormalizedEmail == normalizedSingInInfo);

            if (user == null)
            {
                return Unauthorized();
            }

            var singInResult = await _signInManager
                .CheckPasswordSignInAsync(user, dto.Password, false);
            if (!singInResult.Succeeded)
            {
                return Unauthorized();
            }

            var token = _tokenService.CreateTokenAsync(user);

            var result = new SingInResultDto
            {
                AccessToken = await _tokenService.CreateTokenAsync(user),
                RefreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user),
            };

            return result;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUserAsync()
        {
            var adminRole = new IdentityRole<int>()
            {
                Name = AuthConstants.Role.Admin
            };

            await _roleManager.CreateAsync(adminRole);

            var adminUser = new User()
            {
                Name = "Admin",
                Surname = "Admin",
                UserName = "admin",
                Email = "admin@gmail.com"
            };

            await _userManager.CreateAsync(adminUser, "admin_pa$$w0Rd");
            await _userManager.AddToRoleAsync(adminUser, AuthConstants.Role.Admin);

            return Ok();
        }
    }
}
