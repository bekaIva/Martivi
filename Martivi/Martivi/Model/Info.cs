using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
   public class Info
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
    }
    public class Setting
    {
        public int SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
