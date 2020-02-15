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
    public DbSet<NewProjectAPI.Models.Like> Likes { get; set; }
  
    public DbSet<NewProjectAPI.Models.Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

      builder.Entity<Like>()
        .HasKey(k => new { k.LikerId, k.LikeeId });

      builder.Entity<Like>()
        .HasOne(u => u.Likee)
        .WithMany(u => u.Likers)
        .HasForeignKey(u => u.LikeeId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Like>()
     .HasOne(u => u.Liker)
     .WithMany(u => u.Likees)
     .HasForeignKey(u => u.LikerId)
     .OnDelete(DeleteBehavior.Restrict);
      //For Message
      builder.Entity<Message>()
    .HasOne(u => u.Sender)
    .WithMany(m => m.MessagesSent)
    .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Message>()
        .HasOne(u => u.Recipient)
        .WithMany(m => m.MessagesReceived)
        .OnDelete(DeleteBehavior.Restrict);

   
    }


    }
}
