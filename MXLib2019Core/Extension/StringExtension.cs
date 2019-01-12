using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MXLib2019.Extension
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
