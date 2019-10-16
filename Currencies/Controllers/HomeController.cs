using Currencies.Data;
using Currencies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Currencies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController( ApplicationDbContext context )
        {
            this._db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrenciesForDate( String dateString )
        {
            Boolean isDate = DateTime.TryParse( dateString, out DateTime date );
            if ( date == null || !isDate || date.Date > DateTime.Now.Date )
            {
                return NotFound( "Указана некорректная дата" );
            }

            List<CurrencyValue> currencies = this._db.CurrencyValues.Where( cv => cv.Date.Date == date.Date ).ToList();

            if ( currencies.Count > 0 )
            {
                return Json( GetLinkedListOfCurrencies( currencies ) );
            }

            currencies = GetLinkedListOfCurrencies( await Utils.GetCurrenciesFromInternet( forDate: date ) );

            this._db.CurrencyValues.AddRange( currencies );
            await this._db.SaveChangesAsync();

            return Json( currencies );
        }

        private List<CurrencyValue> GetLinkedListOfCurrencies( List<CurrencyValue> currencies )
        {
            foreach ( CurrencyValue currency in currencies )
            {
                CurrencyInfo savedInfo = this._db.CurrencyInfos.FirstOrDefault( ci => ci.CharCode == currency.CurrencyInfoId );
                if ( savedInfo != null )
                {
                    currency.CurrencyInfo = savedInfo;
                    currency.CurrencyInfoId = savedInfo.CharCode;
                }
            }
            return currencies;
        }
    }
}
