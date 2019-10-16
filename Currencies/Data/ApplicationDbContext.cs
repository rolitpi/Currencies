using Currencies.Models;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base( options )
        {

        }

        public DbSet<CurrencyInfo> CurrencyInfos { get; set; }

        public DbSet<CurrencyValue> CurrencyValues { get; set; }
    }
}
