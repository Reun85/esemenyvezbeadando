using System;
using System.Security.Principal;
using System.Windows.Navigation;

namespace RobotPigs.WPF.View
{
    public class Field : ViewModelBase
    {
        private readonly String[] chars = { "⇑", "⇒", "⇓", "⇐","" };
        private Int32 _data = 4;
        private int _background = 0;
        private int _color = 0;


        public Int32 Data { 
            get { return _data; }
            set { _data = value; OnPropertyChanged(nameof(Content)); }
        }
        public String Content 
        {
            get { return chars[_data]; }
        }
        public Int32 X { get; set; }

        public Int32 Y { get; set; }
        public static Int32 FontSize { get; set; }
        public Int32 Background { get => _background; set { _background = value;OnPropertyChanged(nameof(Background)); } }
        public Int32 ForeColor { get => _color; set { _color = value; OnPropertyChanged(nameof(ForeColor)); } }
    
    }
}
