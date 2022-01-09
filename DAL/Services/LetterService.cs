using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LetterService : ILetterService
    {
        private readonly IMailRepository mailRepository;
        public LetterService (IMailRepository mailRepository)
        {
            this.mailRepository = mailRepository;
        }
        public async Task CreateLetter(SmtpClient client, MailMessage message)
        {
            await client.SendMailAsync(message);
            await mailRepository.CreateLetter(new Letter { 
                Date = DateTime.UtcNow,
                SenderAddress = message.From.Address,
                SenderDisplayName = message.From.DisplayName,
                Recipients = string.Join(", ", message.To),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsBodyHtml
            });
        }
    }
}
