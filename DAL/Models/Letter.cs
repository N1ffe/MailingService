using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Letter
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SenderAddress { get; set; }
        public string SenderDisplayName { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
