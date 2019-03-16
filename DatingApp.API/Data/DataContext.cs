

using DatingApp.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext: IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole
                    , IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        //Db Context method can be re-used when we derived from Db Context class Name
        
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
        }


        public DbSet<Value>  Values{ get; set; }
        //public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);
            builder.Entity<UserRole>(userRole =>{
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.role)
                    .WithMany(r => r.userRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                
                userRole.HasOne(ur => ur.user)
                    .WithMany(u => u.userRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            builder.Entity<Like>().HasKey(k => new {k.LikeeId, k.LikerId});

            builder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Liker)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likee)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);

           builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(u=> u.MsgSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(u => u.MsgReceived)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}