using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Conver22.Models.Currencies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Conver22.Models
{
    class Converter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">адрес, по которому мы будем забирать json с валютами</param>
        /// <returns></returns>
        public async Task<List<Currency>> GetCurrenciesAsync(string url)
        {
            var result = new List<Currency>();

            using (var httpClient = new HttpClient())
            {
                var jsonString = await httpClient.GetStringAsync(url).ConfigureAwait(false);
                try
                {
                    var currenciesString = JObject.Parse(jsonString).GetValue("Valute").ToString();
                    var currencies = JObject.Parse(currenciesString);
                    foreach (KeyValuePair<string, JToken> currency in currencies)
                    {
                        result.Add(JsonConvert.DeserializeObject<Currency>(currency.Value.ToString()));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }

            return result;
        }
    }
}
