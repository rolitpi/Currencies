using Currencies.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Currencies.Models;

namespace Currencies.Controllers
{
    public class HomeController: Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController( ApplicationDbContext context )
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrenciesForDate(string dateString)
        {
            var isDate = DateTime.TryParse( dateString, out var date );
            if ( date == null || !isDate || date.Date > DateTime.Now.Date )
            {
                return NotFound( "Указана некорректная дата" );
            }

            var currencies = _db.CurrencyValues.Where( cv => cv.Date.Date == date.Date ).ToList();

            if ( currencies.Count > 0 )
            {
                return Json( GetLinkedListOfCurrencies( currencies ) );
            }

            currencies = GetLinkedListOfCurrencies( await Utils.GetCurrenciesFromInternet( forDate: date ) );

            _db.CurrencyValues.AddRange( currencies );
            await _db.SaveChangesAsync();

            return Json( currencies );
        }

        private List<CurrencyValue> GetLinkedListOfCurrencies( List<CurrencyValue> currencies )
        {
            foreach ( var currency in currencies )
            {
                var savedInfo = _db.CurrencyInfos.FirstOrDefault( ci => ci.CharCode == currency.CurrencyInfoId );
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
