using Martivi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Martivi.ViewModels
{
    public class SubMenuViewModel:PropertyChangedBase
    {
		private Menu _SelectedMenu;

		public Menu SelectedMenu
		{
			get { return _SelectedMenu; }
			set { _SelectedMenu = value;
				OnPropertyChanged();}
		}
		private ObservableCollection<SubMenu> _SubMenus;

		public ObservableCollection<SubMenu> SubMenus
		{
			get { return _SubMenus; }
			set { _SubMenus = value; OnPropertyChanged(); }
		}

	}
}
