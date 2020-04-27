using Martivi.Models.Tracking;
using Martivi.Models.Transaction;
using Newtonsoft.Json;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Martivi.Model
{
    public enum OrderStatus
    {
        Accepted,
        Canceled,
        Completed
    }
    public enum PaymentStatus
    {
        NotPaid,
        PENDING,
        PROCESS,
        ON_HOLD,
        COMPLETED,
        CANCELED,
        NOT_FINISHED,
        SAVED,
        PREPARED,
        CLEARED,
        DENIED,
        EXPIRED,
        FAILED,
        REFUNDED,
        DECLINED,
        RETURNED
    }

    public class Order : PropertyChangedBase
    {
        private OrderStatus _Status;
        public OrderStatus Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }
        private bool _IsSeen;

        public bool IsSeen
        {
            get { return _IsSeen; }
            set { _IsSeen = value; OnPropertyChanged(); }
        }

        public PaymentStatus Payment { get; set; }
        public int OrderId { get; set; }
        public User User { get; set; }
        public UserAddress OrderAddress { get; set; }
        public virtual List<Product> OrderedProducts { get; set; }
        public string Hash { get; set; }
        public string TransactionID { get; set; }
        private long _OrderTimeTicks;

        public long OrderTimeTicks
        {
            get { return _OrderTimeTicks; }
            set { _OrderTimeTicks = value; OnPropertyChanged(); }
        }


        [JsonIgnore]
        public DateTime OrderTime
        {
            get
            {
                DateTime t = new DateTime(OrderTimeTicks, DateTimeKind.Local);
                return t;
            }
            set
            {
                OrderTimeTicks = value.Ticks;

            }
        }


    }


    //public class Order:PropertyChangedBase
    //{
    //    public Order()
    //    {
    //        //case OrderStatus.Accepted: return "შესრულების მოლოდინში" ;
    //        //           case OrderStatus.Completed: return "შეკვეთა შესრულებულია";
    //        //           case OrderStatus.Canceled: return "შეკვეთა გაუქმებულია";
    //        ProductDeliveryTrackings = new ObservableCollection<ProductDeliveryTrackingModel>();
    //        ProductDeliveryTrackings.Add(new ProductDeliveryTrackingModel() {Status= StepStatus.NotStarted,OrderStatus= OrderStatus.Accepted});
    //        ProductDeliveryTrackings.Add(new ProductDeliveryTrackingModel() {Status= StepStatus.NotStarted,OrderStatus= OrderStatus.Completed});
    //    }

    //    private ObservableCollection<ProductDeliveryTrackingModel> productDeliveryTrackings;


    //    /// <summary>
    //    /// Gets or sets the property that has been bound with bindable layout, which displays the collection of delivery tracking.
    //    /// </summary>
    //    /// 

    //    public ObservableCollection<ProductDeliveryTrackingModel> ProductDeliveryTrackings
    //    {
    //        get
    //        {
    //            return productDeliveryTrackings;
    //        }

    //        set
    //        {
    //            productDeliveryTrackings = value;
    //            OnPropertyChanged();
    //        }
    //    }


    //    private OrderStatus _Status;

    //    public OrderStatus Status
    //    {
    //        get { return _Status; }
    //        set 
    //        {
    //            _Status = value; OnPropertyChanged();
    //            switch(value)
    //            {
    //                case OrderStatus.Accepted:
    //                    {
    //                        ProductDeliveryTrackings[0].Status = StepStatus.Completed;
    //                        ProductDeliveryTrackings[1].Status = StepStatus.InProgress;
    //                        ProductDeliveryTrackings[0].OrderDate = OrderTime.ToString();
    //                        ProductDeliveryTrackings[0].Title = "შეკვეთა მიღებულია";
    //                        ProductDeliveryTrackings[0].TitleStatus = "მიმდინარეობს მიწოდება";
    //                        break;
    //                    }
    //                case OrderStatus.Canceled:
    //                    {
    //                        ProductDeliveryTrackings[0].OrderDate = string.Empty;
    //                        ProductDeliveryTrackings[0].Title = "შეკვეთა გაუქმებულია";
    //                        ProductDeliveryTrackings[0].Status = StepStatus.NotStarted;
    //                        ProductDeliveryTrackings[1].Status = StepStatus.NotStarted;
    //                        break;
    //                    }
    //                case OrderStatus.Completed:
    //                    {
    //                        ProductDeliveryTrackings[0].Title = "შეკვეთა მიღებულია";
    //                        ProductDeliveryTrackings[0].TitleStatus = "მიმდინარეობს მიწოდება";

    //                        ProductDeliveryTrackings[1].Title = "შეკვეთა შესრულებულია";
    //                        ProductDeliveryTrackings[1].TitleStatus = "მიმდინარეობს მიწოდება";
    //                        ProductDeliveryTrackings[1].Status = StepStatus.Completed;
    //                        break;
    //                    }
    //            }
    //        }
    //    }

    //            //switch(value)
    //            //{
    //            //    case OrderStatus.Accepted:
    //            //        {

    //            //            Title = "შეკვეთა მიღებულია";
    //            //            TitleStatus = "მიმდინარეობს მიწოდება";
    //            //            break;
    //            //        }
    //            //    case OrderStatus.Canceled:
    //            //        {
    //            //            Title = "შეკვეთა გაუქმებულია";
    //            //            TitleStatus = "";
    //            //            break;
    //            //        }
    //            //    case OrderStatus.Completed:
    //            //        {
    //            //            Title = "შეკვეთა გაუქმებულია";
    //            //            break;
    //            //        }
    //            //}

    //    public PaymentStatus Payment { get; set; }
    //    public int OrderId { get; set; }
    //    public User User { get; set; }
    //    public UserAddress OrderAddress { get; set; }
    //    public virtual List<Product> OrderedProducts { get; set; }

    //    private long _OrderTimeTicks;

    //    public long OrderTimeTicks
    //    {
    //        get { return _OrderTimeTicks; }
    //        set { _OrderTimeTicks = value; OnPropertyChanged(); ProductDeliveryTrackings[0].OrderDate = OrderTime.ToString(); }
    //    }


    //    [JsonIgnore]
    //    public DateTime OrderTime
    //    {
    //        get 
    //        {
    //            DateTime t = new DateTime(OrderTimeTicks, DateTimeKind.Local);
    //            return t;
    //        }
    //        set 
    //        {
    //            OrderTimeTicks = value.Ticks; 

    //        }
    //    }


    //}
}
