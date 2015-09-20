using System;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;

namespace CostEffectiveCode.Extensions
{
    [PublicAPI]
    public static class EnumHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof (T), false);
            return (attributes.Length > 0) ? (T) attributes[0] : null;
        }

        [CanBeNull]
        public static string GetDescription(this Enum member)
        {
            if (member.GetType().IsEnum == false)
                throw new ArgumentOutOfRangeException(nameof(member), "member is not enum");

            FieldInfo fieldInfo = member.GetType().GetField(member.ToString());

            if (fieldInfo == null)
                return null;

            var attributes = (DescriptionAttribute[]) fieldInfo
                .GetCustomAttributes<DescriptionAttribute>(false);

            return attributes.Length > 0 ? attributes[0].Description : member.ToString();
        }
    }
}
