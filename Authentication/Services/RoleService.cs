using Authentication.Interfaces;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task SetRoles(UserRoles userRoles)
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == userRoles.Username);
            var roles = roleManager.Roles.ToList().Where(r => userRoles.Roles.Contains(r.Name)).Select(r => r.Name).ToList();
            var result = await userManager.AddToRolesAsync(user, roles);
            if (!result.Succeeded)
                throw new Exception(string.Join(';', result.Errors.Select(x => x.Description)));
        }
        public async Task CreateRole(string name)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(name));
            if (!result.Succeeded)
                throw new Exception($"Cannot create role: {name}.");
        }
        public async Task<bool> RoleExists(string name)
        {
            return await roleManager.FindByNameAsync(name) != null;
        }
        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await Task.Run(() => roleManager.Roles);
        }
        public async Task<IEnumerable<string>> GetUserRoles(string username)
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == username);
            return await userManager.GetRolesAsync(user);
        }
    }
}
