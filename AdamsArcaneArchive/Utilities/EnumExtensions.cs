using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AdamsArcaneArchive.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            ?.GetMember(enumValue.ToString())
                            ?.FirstOrDefault()
                            ?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? enumValue.ToString();
        }
    }
}
