using Martivi.ViewModels;
using MartiviSharedLib.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Views.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecoverPage : ContentPage
    {
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }

        private int _Code;

        public int Code
        {
            get { return _Code; }
            set { _Code = value; OnPropertyChanged(); }
        }

        private string _NewPassword;

        public string NewPassword
        {
            get { return _NewPassword; }
            set { _NewPassword = value; OnPropertyChanged(); SetColor(); }
        }

        private string _ConfirmPassword;

        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { _ConfirmPassword = value; OnPropertyChanged(); SetColor(); }
        }

        private bool _RequestProcessing;

        public bool RequestProcessing
        {
            get { return _RequestProcessing; }
            set { _RequestProcessing = value; OnPropertyChanged(); }
        }

        MainViewModel mv;
        public PasswordRecoverPage()
        {
            InitializeComponent();
            mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
        }


        void SetColor()
        {
            try
            {
                ConfirmPasswordBorder.BorderColor = ConfirmPassword?.Equals(NewPassword) ?? false ? Color.FromHex("#ced2d9") : Color.FromHex("#FF4A4A");
            }
            catch
            { }
        }

        private async void Next(object sender, EventArgs e)
        {
            try
            {
                if (RequestProcessing) DisplayAlert("შეცდომა","გთხოვთ დაიცადოთ სანამ მიმდინარე პროცესი დასრულდება","Ok");
                RequestProcessing = true;
                if (string.IsNullOrEmpty(Email)) throw new Exception("გთხოვთ შეიყვანოთ მომხმარებლის ელ-ფოსტა");
                var res = await mv.Services.RequestPasswordRecoveryCode(new RequestPasswordRecoveryCodeModel() { Username = Email });
                switch (res.Error)
                {
                    case Result.PasswordChanged:
                        break;
                    case Result.CodeSent:
                        DisplayAlert("შესრულდა", res.Message, "ok"); tabView.SelectedIndex = 1;
                        break;

                    default:
                        DisplayAlert("შეცდომა", res.Message, "ok");
                        break;
                }
                RequestProcessing = false;

            }
            catch (Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "ok");
            }
            finally
            {
                RequestProcessing = false;
            }
        }

        private async void ChangeClicked(object sender, EventArgs e)
        {
            try
            {
                if (RequestProcessing) DisplayAlert("შეცდომა", "გთხოვთ დაიცადოთ სანამ მიმდინარე პროცესი დასრულდება", "Ok");
                RequestProcessing = true;
                if (Code < 100000 || Code > 999999) throw new Exception("კოდი უნდა იყოს 6 ნიშნა ციფრი");
                if (!string.Equals(NewPassword, ConfirmPassword, StringComparison.Ordinal)) throw new Exception("გამეორება არ ემთხვევა ახალ პაროლს");

                if (string.IsNullOrEmpty(NewPassword)) throw new Exception("გთხოვთ შეიყვანოთ ახალი პაროლი");
                if (string.IsNullOrEmpty(NewPassword)) throw new Exception("გთხოვთ დაადასტუროთ ახალი პაროლი");
                var res = await mv.Services.RecoverPassword(new PasswordChangeRequestModel()
                { Code = Code, NewPassword = ConfirmPassword, Username = Email });
                if (string.IsNullOrEmpty(Email)) throw new Exception("გთხოვთ შეიყვანოთ მომხმარებლის ელ-ფოსტა");
                switch (res.Error)
                {
                    case Result.PasswordChanged:
                        {
                            DisplayAlert("შესრულდა", res.Message, "ok"); tabView.SelectedIndex = 1;
                            try
                            {
                                Navigation.PopAsync();
                            }
                            catch
                            { }
                            break;
                        }
                    case Result.CodeSent:

                        break;

                    default:
                        DisplayAlert("შეცდომა", res.Message, "ok");
                        break;
                }
            }
            catch (Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "ok");
            }
            finally
            {
                RequestProcessing = false;
            }
        }
    }
}