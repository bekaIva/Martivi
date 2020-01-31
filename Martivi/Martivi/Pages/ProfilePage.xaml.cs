using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Martivi.Model;
using Martivi.ViewModels;
using System;
using System.Collections.Generic;
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
            base.OnAppearing();
            FirstName = mv.CurrentUser.FirstName;
            LastName = mv.CurrentUser.LastName;
            Phone = mv.CurrentUser.Phone;
            Address = mv.CurrentUser.UserAddress;
            UserName = mv.CurrentUser.Username;
            Password = string.Empty;
            ProfileImageUrl = mv.CurrentUser.ProfileImageUrl;
        }
        private async void UpdateClicked(object sender, EventArgs e)
        {
            try
            {
                if (UserUpdating) return;
                UserUpdating = true;
              var res = await  mv.UpdateUser(new MartiviSharedLib.Models.Users.UpdateModel(){FirstName=FirstName,LastName=LastName,Password=Password,Phone=Phone,UserAddress=Address,Username= UserName,ProfileImageUrl=ProfileImageUrl });
            if(res)
                {
                    DisplayAlert("", "მონაცემები წარმატებით განახლდა", "Ok");
                }
            }

            catch(ImgurException ime)
            {
                DisplayAlert("შეცდომა", "ფოტოს ატვირთვა არ შესრულდა.", "Ok");
            }
            catch(Exception ee) 
            {
                DisplayAlert("შეცდომა", ee.Message,"Ok");
            }
            finally { UserUpdating = false; }
        }

        private async void PickImageTapped(object sender, EventArgs e)
        {
            var ds = DependencyService.Get<IPhotoPickerService>();
            Stream stream = await ds.GetImageStreamAsync();
            await UploadImage(stream);

        }
        public async Task UploadImage(Stream iStream)
        {
            try
            {
                var client = new ImgurClient("d45226de4e13c51", "30e2cdb6de8061b1e003e6a250e3241147029fdb");
                var endpoint = new ImageEndpoint(client);
                IImage image;
                
                    image = await endpoint.UploadImageStreamAsync(iStream);
                ProfileImageUrl = image.Link;
                
            }
            catch (ImgurException imgurEx)
            {
            }
        }
    }
}