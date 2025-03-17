using Microsoft.EntityFrameworkCore;
using SecureCommSvc.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommSvc.Infrastructure.Context
{
    public class SecureConnDbContext:DbContext
    {

        public SecureConnDbContext(DbContextOptions<SecureConnDbContext> options)
          : base(options)
        {
           // Database.EnsureCreated();


        }

        public DbSet<User> users { get; set; }
        public DbSet<Chat> chats { get; set; }
      
    }
}
