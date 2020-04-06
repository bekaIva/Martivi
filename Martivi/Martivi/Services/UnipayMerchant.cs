using HttpControl;
using Martivi.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

    public class Data
    {
        public string Checkout { get; set; }
        public string UnipayOrderHashID { get; set; }
    }

    public class CreateOrderResult
    {
        public int Errorcode { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }
    public class Transaction
    {
        public string MerchantID { get; set; }
        public string Hash { get; set; }
        public string TransactionID { get; set; }
    }

    public class CreateOrderModel
    {
        public string MerchantID { get; set; }
        public string MerchantUser { get; set; }
        public string MerchantOrderID { get; set; }
        public string OrderPrice { get; set; }
        public string OrderCurrency { get; set; }
        public List<string> Items { get; set; }
        public string BackLink { get; set; }
        public string Mlogo { get; set; }
        public string Mslogan { get; set; }
        public string Language { get; set; }
        public string Hash { get; set; }
    }
    
    public class UnipayMerchant
    {
        SimpleHttpControl sClient = new SimpleHttpControl();
         CreateOrderModel CreateOrder(Order o)
        {
            CreateOrderModel com = new CreateOrderModel();

            return com;
        }
        public async Task<bool> Chekout(Order order)
        {
            string createOrderAddress = "https://api.unipay.com/checkout/createorder";
            var json = JsonConvert.SerializeObject(address);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await sClient.GetResponsePost(createOrderAddress, content);

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
