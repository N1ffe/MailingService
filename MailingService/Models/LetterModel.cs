using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MailingService.Models
{
    public class LetterModel
    {
        [Required]
        public string[] To { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
