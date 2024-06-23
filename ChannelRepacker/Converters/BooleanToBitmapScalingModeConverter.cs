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
    public class BooleanToBitmapScalingModeConverter : OneWayConverter<bool, BitmapScalingMode>
    {
        protected override BitmapScalingMode Convert(bool value, object parameter)
        {
            if (value)
                return BitmapScalingMode.HighQuality;
            else
                return BitmapScalingMode.NearestNeighbor;
        }
    }
}
