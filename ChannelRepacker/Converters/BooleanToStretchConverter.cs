using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NullSoftware.Converters
{
    internal class BooleanToStretchConverter : OneWayConverter<bool, Stretch>
    {
        protected override Stretch Convert(bool value, object parameter)
        {
            if (value)
                return Stretch.Uniform;
            else
                return Stretch.None;
        }
    }
}
