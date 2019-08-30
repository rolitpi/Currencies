using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Currencies.Models;

namespace Currencies.Controllers
{
    public static class Utils
    {
        private static readonly string UrlString = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";

        public static async Task<List<CurrencyValue>> GetCurrenciesFromInternet( DateTime forDate )
        {
            var selectedDate = forDate.Date;
            List<CurrencyValue> loadedCurrencies = new List<CurrencyValue>();
            using (HttpClient http = new HttpClient())
            {
                string str = UrlString + $"{ selectedDate.Day.ToString().PadLeft(2, '0') }/{ selectedDate.Month.ToString().PadLeft(2, '0') }/{ selectedDate.Year }";
                HttpResponseMessage result = await http
                    .GetAsync( str );
                if (result.IsSuccessStatusCode)
                {
                    var stream = await result.Content.ReadAsStreamAsync();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load( stream );
                    XmlElement root = xmlDoc.DocumentElement;
                    if ( root.InnerText.Contains( "Error in parameters" ) )
                    {
                        return loadedCurrencies;
                    }
                    foreach( XmlNode node in root )
                    {
                        var newCurrency = new CurrencyValue()
                        {
                            Date = selectedDate
                        };
                        foreach (XmlNode childnode in node.ChildNodes)
                        {
                            var nodeValue = childnode.InnerText;
                            int.TryParse( nodeValue, out int intValue );
                            float.TryParse( nodeValue, out float floatValue );
                            switch ( childnode.Name )
                            {
                                case "NumCode":
                                    newCurrency.CurrencyInfo.NumCode = intValue;
                                    break;
                                case "CharCode":
                                    newCurrency.CurrencyInfo.CharCode = nodeValue.ToUpper();
                                    newCurrency.CurrencyInfoId = nodeValue.ToUpper();
                                    break;
                                case "Nominal":
                                    newCurrency.CurrencyInfo.Nominal = intValue;
                                    break;
                                case "Name":
                                    newCurrency.CurrencyInfo.Name = nodeValue;
                                    break;
                                case "Value":
                                    newCurrency.Value = floatValue;
                                    break;
                            }
                        }
                        loadedCurrencies.Add( newCurrency );
                    }
                }
            }

            return loadedCurrencies;
        }
    }
}
