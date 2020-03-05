using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Martivi.ViewModels.ErrorAndEmpty
{
    /// <summary>
    /// ViewModel for no internet connection page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class NoInternetConnectionPageViewModel : BaseViewModel
    {
        #region Fields

        private string imagePath;

        private string header;

        private string content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="NoInternetConnectionPageViewModel" /> class.
        /// </summary>
        public NoInternetConnectionPageViewModel()
        {
            this.ImagePath = "NoInternet.svg";
            this.Header = "კავშირის პრობლემა";
            this.Content = "სერვერთან დაკავშირება ვერ მოხერხდა. დარწმუნდით, რომ გაქვთ წვდომა ინტერნეტთან";
            this.TryAgainCommand = new Command(this.TryAgain);
        }

        #endregion

        #region Commands

        /// <summary>
        /// Gets or sets the command that is executed when the TryAgain button is clicked.
        /// </summary>
        public ICommand TryAgainCommand { get; set; }

        #endregion

        #region Properties
        private MainViewModel _ViewModel;

        public MainViewModel ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; NotifyPropertyChanged(); }
        }

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
        /// Gets or sets the Header.
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
        /// Gets or sets the Content.
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
        /// Invoked when the Try again button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void TryAgain(object obj)
        {
            ViewModel?.GetCategories();
        }

        #endregion
    }
}
