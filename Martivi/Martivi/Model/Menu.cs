using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
    public class Menu
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<SubMenu> subMenus { get; set; }
    }
    public class SubMenu
    {
        public int subMenuId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string price { get; set; }
        public string image { get; set; }
    }
}
