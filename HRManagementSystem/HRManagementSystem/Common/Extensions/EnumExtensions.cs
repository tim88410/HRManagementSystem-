using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HRManagementSystem.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DisplayAttribute>();
            return attr?.Name ?? value.ToString();
        }

        public static T? FromDisplayName<T>(string displayName) where T : struct, Enum
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                var field = typeof(T).GetField(value.ToString()!);
                var attr = field?.GetCustomAttribute<DisplayAttribute>();
                if (attr?.Name == displayName)
                    return (T)value;
            }
            return null;
        }
    }
}
