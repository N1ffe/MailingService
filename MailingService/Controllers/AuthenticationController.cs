using Authentication.Interfaces;
using MailingService.Models;
using Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailingService.Filters;
using Microsoft.Extensions.Options;
using MailingService.Helpers;
using WebApiDemo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MailingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly JwtSettings jwtSettings;
        private readonly SmtpSettings smtpSettings;
        private readonly UserManager<User> userManager;
        public AuthenticationController(IUserService userService, IRoleService roleService, IOptionsSnapshot<JwtSettings> jwtSettings, IOptionsSnapshot<SmtpSettings> smtpSettings, UserManager<User> userManager)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.jwtSettings = jwtSettings.Value;
            this.smtpSettings = smtpSettings.Value;
            this.userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm]RegisterModel register)
        {
            await userService.Register(new Register
            {
                Username = register.Username,
                Password = register.Password,
                Email = register.Username + smtpSettings.SenderAddress,
                FirstName = register.FirstName,
                LastName = register.LastName
            });
            return Created(string.Empty, string.Empty);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm]LoginModel login)
        {
            var user = await userService.Login(new Login
            {
                Username = login.Username,
                Password = login.Password
            });
            if (user == null)
                return BadRequest();
            var roles = await roleService.GetUserRoles(user.UserName);
            return Ok(JwtHelper.GenerateJwt(user, roles, jwtSettings));
        }
        [HttpPost("createRole")]
        [Authorize]
        public async Task<IActionResult> CreateRole([FromForm]RoleModel role)
        {
            await roleService.CreateRole(role.Name);
            return Created(string.Empty, string.Empty);
        }
        [HttpGet("roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await roleService.GetRoles());
        }
        [HttpGet("userRoles")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles()
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            return Ok(await roleService.GetUserRoles(user.UserName));
        }
        [HttpGet("userRoles/{username}")]
        [Authorize]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            return Ok(await roleService.GetUserRoles(username));
        }
        [HttpPost("setRoles")]
        [Authorize]
        public async Task<IActionResult> SetRoles([FromForm]UserRolesModel userRoles)
        {
            await roleService.SetRoles(new UserRoles
            {
                Username = userRoles.Username,
                Roles = userRoles.Roles
            });
            return Ok();
        }
    }
}
