using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailingService.Models
{
    public class UserRolesModel
    {
        [Required]
        public string Username { get; set; }
        [Required, MinLength(1)]
        public string[] Roles { get; set; }
    }
}
