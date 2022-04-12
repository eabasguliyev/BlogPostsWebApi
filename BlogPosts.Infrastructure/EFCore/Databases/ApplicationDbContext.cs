using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlogPosts.Domain.Commons.EntityBase;
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().ToTable(nameof(Authors));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();

            var neededEntries = entries
                    .Where(i => i.State == EntityState.Added ||
                                i.State == EntityState.Modified)
                    .ToList();

            foreach (var item in neededEntries)
            {
                var obj = item as IEntity;

                if (obj != null)
                {
                    if (obj.CreateDate == DateTime.MinValue)
                    {
                        obj.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        obj.ModifyDate = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
