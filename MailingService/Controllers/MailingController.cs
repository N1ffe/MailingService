using DAL;
using DAL.Interfaces;
using MailingService.Filters;
using MailingService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MailingService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Authentication.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MailingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    [Authorize]
    public class MailingController : ControllerBase
    {
        private readonly ILetterService letterService;
        private readonly IMailRepository mailRepository;
        private readonly SmtpSettings smtpSettings;
        private readonly UserManager<User> userManager;
        public MailingController(ILetterService letterService, IMailRepository mailRepository, IOptionsSnapshot<SmtpSettings> smtpSettings, UserManager<User> userManager)
        {
            this.letterService = letterService;
            this.mailRepository = mailRepository;
            this.smtpSettings = smtpSettings.Value;
            this.userManager = userManager;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateLetter([FromForm] LetterModel letter)
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            MailMessage message = new MailMessage
            {
                From = new MailAddress(user.Email, $"{user.FirstName} {user.LastName}"),
                Subject = letter.Subject,
                Body = letter.Body,
                IsBodyHtml = smtpSettings.IsBodyHtml
            };
            var emailAddress = new EmailAddressAttribute();
            List<string> invalid = new List<string>();
            string errorMessage = "";
            foreach (string address in letter.To)
            {
                if (emailAddress.IsValid(address))
                    message.To.Add(address);
                else
                    invalid.Add(address);
            }
            if (invalid.Any())
            {
                errorMessage = string.Join(", ", invalid);
                errorMessage = "Could not send letter to: " + errorMessage + ". Reason: invalid email address.";
            }
            if (!message.To.Any())
                return BadRequest(errorMessage);
            SmtpClient client = new SmtpClient
            {
                Host = smtpSettings.Host,
                Port = smtpSettings.Port,
                EnableSsl = smtpSettings.EnableSsl,
                UseDefaultCredentials = smtpSettings.UseDefaultCredentials,
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password)
            };
            await letterService.CreateLetter(client, message);
            return Ok(errorMessage);
        }
        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            return Ok(await mailRepository.GetInbox(new MailAddress(user.Email)));
        }
        [HttpGet("sent")]
        public async Task<IActionResult> GetSent()
        {
            var user = userManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
            return Ok(await mailRepository.GetSent(new MailAddress(user.Email)));
        }
    }
}
