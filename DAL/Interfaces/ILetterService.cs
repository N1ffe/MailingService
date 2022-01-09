using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ILetterService
    {
        Task CreateLetter(SmtpClient client, MailMessage message);
    }
}
