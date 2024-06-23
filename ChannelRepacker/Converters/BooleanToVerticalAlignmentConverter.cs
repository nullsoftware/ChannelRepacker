using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NullSoftware.Converters
{
    public class BooleanToVerticalAlignmentConverter : OneWayConverter<bool, VerticalAlignment>
    {
        protected override VerticalAlignment Convert(bool value, object parameter)
        {
            if (value)
                return VerticalAlignment.Stretch;
            else
                return VerticalAlignment.Center;
        }
    }
}
