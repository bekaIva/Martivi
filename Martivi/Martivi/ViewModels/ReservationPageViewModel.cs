using Martivi.Model;
using Martivi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Martivi.ViewModels
{
    public class ReservationPageViewModel:PropertyChangedBase
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }


        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; OnPropertyChanged(); }
        }

        private string _TotalPeople;

        public string TotalPeople
        {
            get { return _TotalPeople; }
            set { _TotalPeople = value; OnPropertyChanged(); }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged(); }
        }

        private TimeSpan _Time;
        public TimeSpan Time
        {
            get 
            {
                return _Time;
            }
            set 
            {
                _Time = value; OnPropertyChanged();
            }
        }
        public ApiServices Service { get; set; } = new ApiServices();
        public ReservationPageViewModel()
        {
            
        }
        public async Task<bool> Reserve()
        {
            Reservation reservation = new Reservation() {Name=Name,Email=Email,Phone=Phone,TotalPeople=TotalPeople,Date=Date.ToString(),Time=Time.ToString() };
            return await Service.ReserveTable(reservation);
          
        }
    }
}
