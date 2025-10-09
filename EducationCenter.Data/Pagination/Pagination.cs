using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Data.Pagination
{
    public static class Pagination
    {
        public static async Task<List<T>> GetPagedAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
        {
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
