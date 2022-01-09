using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class MailDbContext : DbContext
    {
        public DbSet<Letter> Letters { get; set; }
        public MailDbContext(DbContextOptions<MailDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
