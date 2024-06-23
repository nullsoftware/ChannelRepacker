using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NullSoftware.Converters
{
    public class BooleanToHorizontalAlignmentConverter : OneWayConverter<bool, HorizontalAlignment>
    {
        protected override HorizontalAlignment Convert(bool value, object parameter)
        {
            if (value)
                return HorizontalAlignment.Stretch;
            else
                return HorizontalAlignment.Center;
        }
    }
}
