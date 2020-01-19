using Martivi.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Martivi.Services
{
    public class ApiServices
    {
        HttpClient client = new HttpClient();
        public ApiServices()
        {
        }
        public async Task<List<Category>> GetCategories()
        {
            var res = await client.GetStringAsync("https://martiviapi.azurewebsites.net/api/Categories");
            List<Category> menu = JsonConvert.DeserializeObject<List<Category>>(res);
            return menu;
        }
        public async Task<bool> Chekout(Order reservation)
        {
            var json = JsonConvert.SerializeObject(reservation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://martiviapi.azurewebsites.net/api/Reservations", content);
            return response.IsSuccessStatusCode;
        }
    }
}
