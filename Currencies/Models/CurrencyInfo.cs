using System;
using System.ComponentModel.DataAnnotations;

namespace Currencies.Models
{
    public class CurrencyInfo
    {
        [Key]
        public String CharCode { get; set; }

        public Int32 Nominal { get; set; }

        public Int32 NumCode { get; set; }

        public String Name { get; set; }
    }
}
