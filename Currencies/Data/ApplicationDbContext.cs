using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Currencies.Models;

namespace Currencies.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base( options )
        {

        }

        public DbSet<CurrencyInfo> CurrencyInfos { get; set; }

        public DbSet<CurrencyValue> CurrencyValues { get; set; }
    }
}
