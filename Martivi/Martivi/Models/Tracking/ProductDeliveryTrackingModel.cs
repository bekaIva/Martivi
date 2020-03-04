using System.Runtime.Serialization;
using Martivi.Model;
using Syncfusion.XForms.ProgressBar;
using Xamarin.Forms.Internals;

namespace Martivi.Models.Tracking
{
    public class ProductDeliveryTrackingModel:PropertyChangedBase
    {
       


        #region Properties

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title status.
        /// </summary>
        public string TitleStatus { get; set; }

        

        /// <summary>
        /// Gets or sets the progress value.
        /// </summary>
        public double ProgressValue { get; set; }

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>
      

        /// <summary>
        /// Gets or sets the step status.
        /// </summary>

        private StepStatus _Status;

        public StepStatus Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }

        private OrderStatus _OrderStatus;

        public OrderStatus OrderStatus
        {
            get { return _OrderStatus; }
            set 
            {
                _OrderStatus = value; 
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public string OrderDate { get; set; }

        #endregion
    }
}
