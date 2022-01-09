using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interfaces
{
    public interface IRoleService
    {
        Task SetRoles(UserRoles userRoles);
        Task CreateRole(string name);
        Task<bool> RoleExists(string name);
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<IEnumerable<string>> GetUserRoles(string username);
    }
}
