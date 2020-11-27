using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Web.Repositories
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Societe> Societes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPlaning> PostPlanings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostPlaningConfiguration());
            modelBuilder.ApplyConfiguration(new GroupUserConfiguration());
        }

        public void Reset()
        {
            try
            {
                foreach (Group group in Groups)
                {
                    Entry(group).State = EntityState.Detached;
                }
                foreach (User user in Users)
                {
                    Entry(user).State = EntityState.Detached;
                }
                foreach (Post post in Posts)
                {
                    Entry(post).State = EntityState.Detached;
                }
                foreach (PostPlaning post in PostPlanings)
                {
                    Entry(post).State = EntityState.Detached;
                }
                foreach (var societe in Societes)
                {
                    Entry(societe).State = EntityState.Detached;
                }
                SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }

}
