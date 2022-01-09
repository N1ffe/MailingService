using Authentication.Interfaces;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IRoleService roleService;
        public UserService(UserManager<User> userManager, IRoleService roleService)
        {
            this.userManager = userManager;
            this.roleService = roleService;
        }
        public async Task Register(Register register)
        {
            var result = await userManager.CreateAsync(new User
            {
                UserName = register.Username,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
            }, register.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
            if (!await roleService.RoleExists("user"))
                await roleService.CreateRole("user");
            await roleService.SetRoles(new UserRoles { Username = register.Username, Roles = new string[] { "user" } });
        }
        public async Task<User> Login(Login login)
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == login.Username);
            if (user == null)
                throw new Exception($"User not found: '{login.Username}'.");
            if (await userManager.CheckPasswordAsync(user, login.Password))
                return user;
            else
                return null;
        }
    }
}
