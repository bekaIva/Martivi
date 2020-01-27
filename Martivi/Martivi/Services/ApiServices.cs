using HttpControl;
using Martivi.Model;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
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
        SimpleHttpControl sClient = new SimpleHttpControl();
        private static ISettings AppSettings =>
    CrossSettings.Current;
        public static string ServerBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(ServerBaseAddress), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ServerBaseAddress), value);
        }
        public ApiServices()
        {
        }
        public async Task<List<Category>> GetCategories()
        {
            var res = await client.GetStringAsync(ServerBaseAddress+"api/Categories");
            List<Category> menu = JsonConvert.DeserializeObject<List<Category>>(res);
            return menu;
        }
      
        public async Task<UserBase> Register(RegisterModelBase regModel)
        {
            var json = JsonConvert.SerializeObject(regModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ServerBaseAddress + "Users/register", content);
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                return JsonConvert.DeserializeObject<UserBase>(resStr);
            }
            if(response.StatusCode== System.Net.HttpStatusCode.BadRequest)
            {
                throw new RegistrationException(resStr);
            }
            throw new RegistrationException("რეგისტრაცია არ შესრულდა! \nშეცდომის კოდი: " + response.StatusCode + "\n" + resStr);

        }
        public async Task<UserBase> GetUser(int userId,string token)
        {
            var response = await sClient.GetResponse(ServerBaseAddress + "Users/" + userId, new Header[] { new Header() { Name = "Authorization", Value = token } });
            
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var ubase = JsonConvert.DeserializeObject<UserBase>(resStr);
                return ubase;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get user failed! \nError Code: " + response.StatusCode + "\n" + resStr);

        }
        public async Task<List<ChatMessage>> GetChatMessages(string token)
        {
            
            var response = await sClient.GetResponse(ServerBaseAddress + "api/Chat/GetChat", new Header[] { new Header() { Name = "Authorization", Value = token } });

            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var chatmessages = JsonConvert.DeserializeObject<List<ChatMessage>>(resStr);
                return chatmessages;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get user failed! \nError Code: " + response.StatusCode + "\n" + resStr);

        }
        public async Task<User> Authenticate(AuthenticateModelBase authModel)
        {
            var json = JsonConvert.SerializeObject(authModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Users/authenticate", content);
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                return JsonConvert.DeserializeObject<User>(resStr);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new AuthenticateException("ავტორიზაცია არ შესრულდა! \nშეცდომის კოდი: " + response.StatusCode + "\n" + resStr);
        }
        public async Task SendMessage(ChatMessage message,string token)
        {
            var json = JsonConvert.SerializeObject(message);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "api/Chat", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = token } });
            string resStr = await response.Content.ReadAsStringAsync();
            
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            if (response.IsSuccessStatusCode) return;
            throw new Exception("Message send Failed! \nError Code: " + response.StatusCode + "\n" + resStr);
        }
        public async Task<bool> Chekout(Order order)
        {
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Api/Orders", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = order.User.Token } });
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            if (response.IsSuccessStatusCode) return true;
            throw new Exception("Message send Failed! \nError Code: " + response.StatusCode + "\n" + resStr);
        }

        public async Task<bool> DeleteOrder(Order order,string Token)
        {
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "api/Orders/DeleteOrder", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = Token } });
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                return true;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(response.StatusCode.ToString() + ": " + resStr);
            }

            throw new Exception(response.StatusCode.ToString() + ": " + resStr);
        }

        public async Task<List<Order>> LoadOrders(int UserId,string Token)
        {
            var json = JsonConvert.SerializeObject(new User() {UserId=UserId });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "api/Orders/GetOrders", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = Token } });
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var orders = JsonConvert.DeserializeObject<List<Order>>(resStr);
                return orders;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(response.StatusCode.ToString()+": "+ resStr);
            }

            throw new Exception(response.StatusCode.ToString() + ": " + resStr);
        }
    }
}
