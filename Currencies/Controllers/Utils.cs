using Currencies.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Currencies.Controllers
{
    public static class Utils
    {
        private const String UrlString = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";

        public static async Task<List<CurrencyValue>> GetCurrenciesFromInternet( DateTime forDate )
        {
            List<CurrencyValue> loadedCurrencies = new List<CurrencyValue>();
            HttpResponseMessage result = await TryToConnectToServer(forDate);
            if ( result == null )
            {
                return loadedCurrencies;
            }

            System.IO.Stream stream = await result.Content.ReadAsStreamAsync();
            XmlElement root = GetXMLRootFromStream(stream);
            if ( root == null )
            {
                return loadedCurrencies;
            }
            loadedCurrencies = GetCurrencyValuesFromXmlRoot( root, forDate );

            return loadedCurrencies;
        }

        private static List<CurrencyValue> GetCurrencyValuesFromXmlRoot( XmlElement root, DateTime forDate )
        {
            List<CurrencyValue> result = new List<CurrencyValue>();
            foreach ( XmlNode node in root )
            {
                CurrencyValue newCurrency = new CurrencyValue()
                {
                    Date = forDate.Date
                };
                foreach ( XmlNode childnode in node.ChildNodes )
                {
                    ParseCurrencyValueFromNode( ref newCurrency, childnode );
                }
                result.Add( newCurrency );
            }
            return result;
        }

        private static async Task<HttpResponseMessage> TryToConnectToServer( DateTime date )
        {
            using ( HttpClient http = new HttpClient() )
            {
                DateTime selectedDate = date.Date;
                String str = UrlString + $"{ selectedDate.Day.ToString().PadLeft(2, '0') }/{ selectedDate.Month.ToString().PadLeft(2, '0') }/{ selectedDate.Year }";
                HttpResponseMessage result = await http.GetAsync( str );
                return result.IsSuccessStatusCode ? result : null;
            }
        }

        private static XmlElement GetXMLRootFromStream( System.IO.Stream stream )
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load( stream );
            XmlElement root = xmlDoc.DocumentElement;
            if ( root.InnerText.Contains( "Error in parameters" ) )
            {
                return null;
            }
            return root;
        }

        private static void ParseCurrencyValueFromNode( ref CurrencyValue newCurrency, XmlNode childnode )
        {
            String nodeValue = childnode.InnerText;
            Int32.TryParse( nodeValue, out Int32 intValue );
            Single.TryParse( nodeValue, out Single floatValue );
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
    }
}
