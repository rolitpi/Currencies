using System;
using System.ComponentModel.DataAnnotations;

namespace Currencies.Models
{
    public class CurrencyValue
    {
        [Key]
        public Int32 Id { get; set; }

        public Single Value { get; set; }

        public DateTime Date { get; set; }

        public String CurrencyInfoId { get; set; }

        public CurrencyInfo CurrencyInfo { get; set; }

        public CurrencyValue()
        {
            this.CurrencyInfo = new CurrencyInfo();
        }
    }
}
