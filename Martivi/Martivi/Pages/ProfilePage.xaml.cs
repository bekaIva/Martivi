using Martivi.Model;
using Martivi.Models.Transaction;
using Martivi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        private ObservableCollection<UserAddress> _UserAddresses;

        public ObservableCollection<UserAddress> UserAddresses
        {
            get { return _UserAddresses; }
            set { _UserAddresses = value; OnPropertyChanged(); }
        }



        private bool _UserUpdating;

        public bool UserUpdating
        {
            get { return _UserUpdating; }
            set { _UserUpdating = value; OnPropertyChanged(); }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; OnPropertyChanged(); }
        }


        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; OnPropertyChanged(); }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; OnPropertyChanged(); }
        }

        private string _Phone;

        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; OnPropertyChanged(); }
        }

        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(); }
        }

        private string _ProfileImageUrl;

        public string ProfileImageUrl
        {
            get { return _ProfileImageUrl; }
            set { _ProfileImageUrl = value; OnPropertyChanged(); }
        }


        MainViewModel mv;
        public ProfilePage()
        {
            InitializeComponent();
            mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                UserAddresses = new ObservableCollection<UserAddress>();
                if (mv.CurrentUser?.UserAddresses != null)
                {
                    foreach (var a in mv.CurrentUser.UserAddresses)
                    {
                        UserAddresses.Add(a);
                    }
                }

                FirstName = mv.CurrentUser.FirstName;
                LastName = mv.CurrentUser.LastName;
                Phone = mv.CurrentUser.Phone;
                UserName = mv.CurrentUser.Username;
                Password = string.Empty;
                ProfileImageUrl = mv.CurrentUser.ProfileImageUrl;
            }
            catch (Exception)
            {

            }
        }
        private async void UpdateClicked(object sender, EventArgs e)
        {
            try
            {
                await UpdateProfile();
            }
            catch(Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
        }
        public async Task UpdateProfile()
        {
            try
            {
                if (UserUpdating) return;
                UserUpdating = true;
                var res = await mv.UpdateUser(new MartiviSharedLib.Models.Users.UpdateModel() { UserAddresses = mv.CurrentUser.UserAddresses, FirstName = FirstName, LastName = LastName, Password = Password, Phone = Phone, Username = UserName, ProfileImageUrl = ProfileImageUrl });
                if (res)
                {
                    DisplayAlert("", "მონაცემები წარმატებით განახლდა", "Ok");
                }
            }

            catch (Exception ee)
            {
                throw ee;
            }
            finally { UserUpdating = false; }
        }
        private async void PickImageTapped(object sender, EventArgs e)
        {
            try
            {
                var ds = DependencyService.Get<IPhotoPickerService>();
                Stream stream = await ds.GetImageStreamAsync();
                
                if (stream != null)
                {
                    var s = Services.ApiServices.ResizeImageAndroid(stream, 400);
                    ProfileImageUrl = Services.ApiServices.ServerBaseAddress + "Images/" + await mv.Services.UploadFile(s, "Bearer " + mv.Token);
                    await UpdateProfile();
                }

            }
            catch (Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }

        }
    }
}