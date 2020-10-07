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
        public DbSet<Societe> Societes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostPlaning> PostPlanings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostPlaningConfiguration());
        }
    }

}
