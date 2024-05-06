using Microsoft.EntityFrameworkCore;
using StroyOnline_API.Models;
using System.Collections.Generic;

namespace StroyOnline_API.Data
{
    public class StroyOnlineDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        public StroyOnlineDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
