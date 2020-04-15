using HttpControl;
using Martivi.Model;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Martivi.Services
{
    public class ChekoutData
    {
        public string Status { get; set; }
        public string MerchantOrderID { get; set; }
    }

    public class CheckoutResult
    {
        public int Errorcode { get; set; }
        public string Message { get; set; }
        public ChekoutData Data { get; set; }
    }

    public class CheckoutData
    {
        public string Checkout { get; set; }
        public string UnipayOrderHashID { get; set; }
    }

    public class CreateOrderResult
    {
        public int Errorcode { get; set; }
        public string Message { get; set; }
        public CheckoutData Data { get; set; }
    }
    public class Transaction
    {
        public string MerchantID { get; set; }
        public string Hash { get; set; }
        public string TransactionID { get; set; }
    }

    public class CreateOrderModel
    {
        public string OrderName { get; set; }
        public string MerchantID { get; set; }
        public string MerchantUser { get; set; }
        public string MerchantOrderID { get; set; }
        public string OrderPrice { get; set; }
        public string OrderCurrency { get; set; }
        public List<string> Items { get; set; } = new List<string>();
        public string BackLink { get; set; }
        public string Mlogo { get; set; }
        public string Mslogan { get; set; }
        public string Language { get; set; }
        public string Hash { get; set; }
    }
    
    public class UnipayMerchant
    {
        private static ISettings AppSettings =>
    CrossSettings.Current;
        public static string ServerBaseAddress
        {
            get => AppSettings.GetValueOrDefault(nameof(ServerBaseAddress), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ServerBaseAddress), value);
        }
        public static string CheckoutBackLink
        {
            get => AppSettings.GetValueOrDefault(nameof(CheckoutBackLink), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(CheckoutBackLink), value);
        }
        SimpleHttpControl sClient = new SimpleHttpControl();

        byte[] getLogo()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var ress= assembly.GetManifestResourceNames();
            using (var stream = assembly.GetManifestResourceStream("Martivi.Resources.ic_launcher.png"))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
                // TODO: use the buffer that was read
            }
        }
        CreateOrderModel GenerateOrderCreateRequest(Order order)
        {
            CreateOrderModel om = new CreateOrderModel();
            om.MerchantID = "7603109";
            om.MerchantUser = order.User.Username;
            om.MerchantOrderID = order.OrderId.ToString();
            om.BackLink = Convert.ToBase64String(Encoding.ASCII.GetBytes(CheckoutBackLink));
            om.Mlogo = Convert.ToBase64String(getLogo());
            om.Mslogan = "შეუკვეთე მარტივად";
            om.Language = "GE";
            om.OrderCurrency = "GEL";
            om.OrderName = "Order #:" + order.OrderId;
            om.OrderPrice = order.OrderedProducts.Sum((p) => { return p.TotalPrice; }).ToString();
            string PasswordStr = "{85DAED23-CA16-4CA9-8673-1E1C2E506526}|{" + order.User.Username + "}|{" + order.OrderId.ToString() + "}|{" + om.OrderPrice + "}|{" + om.OrderName + "}";
            om.Hash = CalculateMD5Hash(PasswordStr);

            om.Items.AddRange(order.OrderedProducts.Select((p) => { return "0|0|"+p.Name+"|"+p.Description; }));
            return om;
        }
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public async Task<bool> Chekout(Order order)
        {
            string createOrderAddress = "https://api.unipay.com/checkout/createorder";
            var json = JsonConvert.SerializeObject(GenerateOrderCreateRequest(order));
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(createOrderAddress, content,useCloudFlareBypass:false);

            string resStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(resStr);
            }
            throw new Exception("Get addresses failed! \nError Code: " + response.StatusCode + "\n" + resStr);
        }
    }
}
