using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailingService.Models
{
    public class RoleModel
    {
        [Required]
        public string Name { get; set; }
    }
}
