using System;
using System.ComponentModel;

namespace PureActive.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                )
                ? value.ToString()
                : attribute.Description;
        }

        public static string ToEnumString(this string enumStr)
        {
            return (string.IsNullOrWhiteSpace(enumStr) ? null : enumStr.Trim().Replace(" ", "_"));
        }

        public static string FromEnumValue(this Enum value)
        {
            return (value.ToString().Replace("_", " "));
        }
    }
}
