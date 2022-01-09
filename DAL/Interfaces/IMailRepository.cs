using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IMailRepository
    {
        Task CreateLetter(Letter letter);
        Task<IEnumerable<Letter>> GetAll();
        Task<IEnumerable<Letter>> GetInbox(MailAddress address);
        Task<IEnumerable<Letter>> GetSent(MailAddress address);
    }
}
