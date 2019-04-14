using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;

namespace OSDevGrp.OSIntranet.Mvc.Helpers
{
    public static class HtmlExtensions
    {
        public static HtmlString EnumDisplayNameFor(this Enum enumValue)
        {
            Type enumValueType = enumValue.GetType();
            MemberInfo enumValueMemberInfo = enumValueType.GetMember(enumValue.ToString()).First();

            DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute) enumValueMemberInfo.GetCustomAttributes(typeof(DisplayNameAttribute)).FirstOrDefault();
            if (displayNameAttribute == null)
            {
                return new HtmlString(enumValue.ToString());
            }

            return new HtmlString(string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) == false ? displayNameAttribute.DisplayName : enumValue.ToString());
        }
    }
}