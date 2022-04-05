using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogPosts.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogPosts.Infrastructure.EFCore.Databases
{
    public class ApplicationDbContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().ToTable(nameof(Authors));
        }
    }
}
