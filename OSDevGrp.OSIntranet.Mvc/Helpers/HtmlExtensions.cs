using System;
using System.ComponentModel.DataAnnotations;
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

            DisplayAttribute displayAttribute = (DisplayAttribute) enumValueMemberInfo.GetCustomAttributes(typeof(DisplayAttribute)).FirstOrDefault();
            if (displayAttribute == null)
            {
                return new HtmlString(enumValue.ToString());
            }

            return new HtmlString(string.IsNullOrWhiteSpace(displayAttribute.Name) == false ? displayAttribute.Name : enumValue.ToString());
        }
    }
}