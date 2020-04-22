using System;
using System.ComponentModel;
using System.Linq;

namespace PrinterQueue.Models
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
