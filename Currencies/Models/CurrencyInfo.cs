using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Models
{
    public class CurrencyInfo
    {
        [Key]
        public string CharCode { get; set; }

        public int Nominal { get; set; }

        public int NumCode { get; set; }

        public string Name { get; set; }
    }
}
