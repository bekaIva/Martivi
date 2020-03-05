using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Martivi.ViewModels.ErrorAndEmpty
{
    /// <summary>
    /// ViewModel for empty cart page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class EmptyCartPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="EmptyCartPageViewModel" /> class.
        /// </summary>
        public EmptyCartPageViewModel()
        {
            this.ImagePath = "EmptyCart.svg";
            this.Header = "კალათა ცარიელია";
            this.Content = "კალათში არ გაქვთ არცერთი ნივთი";
            this.GoBackCommand = new Command(this.GoBack);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the Go back button is clicked.
        /// </summary>
        public ICommand GoBackCommand { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ImagePath.
        /// </summary>
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public string Header
        {
            get
            {
                return this.header;
            }

            set
            {
                this.header = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }

            set
            {
                this.content = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Go back button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void GoBack(object obj)
        {
            if(Shell.Current.Navigation.NavigationStack.Count>1)
            {
                Shell.Current.Navigation.PopAsync();
            }
            else
            {
                Shell.Current.GoToAsync($"//home/cb/category");
            }
        }

        #endregion
    }
}
