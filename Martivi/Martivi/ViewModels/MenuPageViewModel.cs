using Martivi.Model;
using Martivi.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Martivi.ViewModels
{
    class MenuPageViewModel:PropertyChangedBase
    {
        #region Properties
        public ApiServices Services { get; set; }
        private ObservableCollection<Model.Menu> _Menus;
        public ObservableCollection<Model.Menu> Menus
        {
            get { return _Menus; }
            set { _Menus = value; OnPropertyChanged(); }
        }
        #endregion
        #region Constructors
        public MenuPageViewModel()
        {
            Services = new ApiServices();
            Menus = new ObservableCollection<Menu>();
            Initialize();

        }
        #endregion
        #region Methods
        public async Task Initialize()
        {
            var menus = await Services.GetMenu();
            foreach (var menu in menus)
            {
                Menus.Add(menu);
            }
        }
        #endregion
    }
}
