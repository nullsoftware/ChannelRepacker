using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NullSoftware.Converters
{
    public class EnumToBooleanConverter : OneWayConverter<Enum, bool>
    {
        protected override bool Convert(Enum value, object parameter)
        {
            return value.Equals(parameter);
        }
    }
}
