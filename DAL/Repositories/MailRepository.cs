using DAL.Context;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class MailRepository : IMailRepository
    {
        private readonly MailDbContext dbContext;
        public MailRepository(MailDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task CreateLetter(Letter letter)
        {
            dbContext.Add(letter);
            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Letter>> GetAll()
        {
            return await Task.Run(() => dbContext.Letters);
        }
        public async Task<IEnumerable<Letter>> GetInbox(MailAddress address)
        {
            return await Task.Run(() => dbContext.Letters.Where(l => l.Recipients.Contains(address.Address)));
        }
        public async Task<IEnumerable<Letter>> GetSent(MailAddress address)
        {
            return await Task.Run(() => dbContext.Letters.Where(l => l.SenderAddress.Contains(address.Address)));
        }
    }
}
