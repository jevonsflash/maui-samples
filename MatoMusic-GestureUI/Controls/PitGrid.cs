using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoMusic.Controls
{
    public class PitGrid : Grid
    {

        public PitGrid()
        {
            IsEnable = true;
        }

        public static readonly BindableProperty IsEnableProperty =
            BindableProperty.Create("IsEnable", typeof(bool), typeof(CircleSlider), true, propertyChanged: (bindable, oldValue, newValue) =>
            {
                var obj = (PitGrid)bindable;
                obj.Opacity = obj.IsEnable ? 1 : 0.8;

            });

        public bool IsEnable
        {
            get { return (bool)GetValue(IsEnableProperty); }
            set { SetValue(IsEnableProperty, value); }
        }




        public string PitName { get; set; }

    }
}
