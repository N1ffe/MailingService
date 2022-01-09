using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Interfaces
{
    public interface IUserService
    {
        Task Register(Register register);
        Task<User> Login(Login login);
    }
}
