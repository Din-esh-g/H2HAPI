using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewProjectAPI.Models;

namespace NewProjectAPI.Data
{
    public class NewProjectAPIContext : DbContext
    {
        public NewProjectAPIContext (DbContextOptions<NewProjectAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NewProjectAPI.Models.MyJob> MyJob { get; set; }
        public DbSet<NewProjectAPI.Models.Property> Properties { get; set; }
    public DbSet<NewProjectAPI.Models.Users> Users { get; set; }
    public DbSet<NewProjectAPI.Models.Event> Event { get; set; }
    public DbSet<NewProjectAPI.Models.News> News { get; set; }
    public DbSet<NewProjectAPI.Models.Photo> Photos { get; set; }
  }
}
