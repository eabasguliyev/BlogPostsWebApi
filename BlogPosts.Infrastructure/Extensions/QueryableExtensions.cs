using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlogPosts.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> query, string includeProperties) where T : class
        {
            var properties = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var property in properties)
            {
                query = query.Include(property);

            }

            return query;
        }
    }
}
