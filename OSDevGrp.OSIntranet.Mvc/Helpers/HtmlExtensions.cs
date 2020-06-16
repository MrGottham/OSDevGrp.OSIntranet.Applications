using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSDevGrp.OSIntranet.Core;

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

        public static SelectListItem SelectListItemFor<T>(this T enumValue, bool isSelected) where T : Enum
        {
            return new SelectListItem(Convert.ToString(enumValue.EnumDisplayNameFor()), Convert.ToString(enumValue), isSelected);
        }

        public static IEnumerable<SelectListItem> SelectListFor<T>(this IEnumerable<T> enumValueCollection, T selectedEnumValue) where T : Enum
        {
            NullGuard.NotNull(enumValueCollection, nameof(enumValueCollection));

            return enumValueCollection.Select(enumValue => enumValue.SelectListItemFor(enumValue.Equals(selectedEnumValue))).ToArray();
        }

        public static IEnumerable<SelectListItem> SelectListFor<T>(this T selectedEnumValue) where T : Enum
        {
            return Enum.GetValues(selectedEnumValue.GetType())
                .Cast<T>()
                .SelectListFor(selectedEnumValue);
        }
    }
}