using Android.Graphics;
using HttpControl;
using Martivi.Model;
using Martivi.Models.Transaction;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Martivi.Services
{
    public class ApiServices
    {
        HttpClient client = new HttpClient();
        SimpleHttpControl sClient = new SimpleHttpControl();
        UnipayMerchant unipayMerchant = new UnipayMerchant();
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
        public static Stream ResizeImageAndroid(Stream stream, float width, float height = -1)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeStream(stream);
            if (height == -1)
            {
                height = originalImage.Height / (originalImage.Width / width);
            }
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            MemoryStream ms = new MemoryStream();

            resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
            ms.Position = 0;
            return ms;

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
        public async Task<User> GetUser(int userId,string token)
        {
            var response = await sClient.GetResponse(ServerBaseAddress + "Users/" + userId, new Header[] { new Header() { Name = "Authorization", Value = token } });
            
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var ubase = JsonConvert.DeserializeObject<User>(resStr);
                return ubase;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get user failed! \nError Code: " + response.StatusCode + "\n" + resStr);

        }

        public async Task<bool> AddAddress(UserAddress address,string token)
        {
            var json = JsonConvert.SerializeObject(address);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Users/AddAddress",content, new Header[] { new Header() { Name = "Authorization", Value = token } });

            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get addresses failed! \nError Code: " + response.StatusCode + "\n" + resStr);

        }

        public async Task<bool> RemoveAddress(UserAddress address, string token)
        {
            var json = JsonConvert.SerializeObject(address);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Users/RemoveAddress", content, new Header[] { new Header() { Name = "Authorization", Value = token } });

            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get addresses failed! \nError Code: " + response.StatusCode + "\n" + resStr);

        }

        public async Task<List<UserAddress>> GetAddresses(string token)
        {
            var response = await sClient.GetResponse(ServerBaseAddress + "Users/GetAdresses" , new Header[] { new Header() { Name = "Authorization", Value = token } });

            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                var ubase = JsonConvert.DeserializeObject<List<UserAddress>>(resStr);
                return ubase;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get addresses failed! \nError Code: " + response.StatusCode + "\n" + resStr);

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
        public async Task<bool> Chekout(Order order,string token)
        {
            if(order.Payment== PaymentStatus.Paid)
            {
                bool paid = await unipayMerchant.Chekout(order);
                if (!paid) throw new Exception("Payment failed!");
            }
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Api/Orders", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = token } });
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

        public async Task<bool> CancelOrder(Order order, string Token)
        {
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "api/Orders/CancelOrder", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = Token } });
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
        public async Task<bool> UpdateUserProfile(UpdateModel update, string Token)
        {
            var json = JsonConvert.SerializeObject(update);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(ServerBaseAddress + "Users/Update", content, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = Token } });
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
        public async Task<string> UploadFile(Stream stream, string Token)
        {
            var f = new MultipartFormDataContent();
            StreamContent fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            f.Add(fileContent, "file", System.IO.Path.GetFileName("Image"));
            var response = await sClient.GetResponsePost(ServerBaseAddress + "api/Upload", f, RequestHeaders: new Header[] { new Header() { Name = "Authorization", Value = Token } });
            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return resStr;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(response.StatusCode.ToString() + ": " + resStr);
            }
            throw new Exception(response.StatusCode.ToString() + ": " + resStr);
        }
    }
}
