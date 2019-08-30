using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Models
{
    public class CurrencyValue
    {
        [Key]
        public int Id { get; set; }

        public float Value { get; set; }

        public DateTime Date { get; set; }

        public string CurrencyInfoId { get; set; }

        public CurrencyInfo CurrencyInfo { get; set; }

        public CurrencyValue()
        {
            CurrencyInfo = new CurrencyInfo();
        }
    }
}
