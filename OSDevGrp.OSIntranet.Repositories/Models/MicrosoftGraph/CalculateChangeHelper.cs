using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
    internal static class CalculateChangeHelper
    {
        internal static string CalculateChange(this string target, string source)
        {
            return CalculateChange(target, source, string.IsNullOrWhiteSpace, string.Empty, (t, s) => string.CompareOrdinal(t, s) == 0);
        }

        internal static DateTime? CalculateChange(this DateTime? target, DateTime? source)
        {
            return CalculateChange(target, source, value => value.HasValue == false, null, (t, s) => t == s);
        }

        internal static List<string> CalculateChange(this List<string> targetCollection, List<string> sourceCollection, int knownItems)
        {
            NullGuard.NotNull(targetCollection, nameof(targetCollection))
                .NotNull(sourceCollection, nameof(sourceCollection));

            return CalculateChange(targetCollection, sourceCollection, knownItems, string.IsNullOrWhiteSpace);
        }

        internal static T CalculateChange<T>(this T target, T source, Predicate<T> isNullOrEmpty, T emptyValue, Func<T, T, bool> comparer)
        {
            NullGuard.NotNull(isNullOrEmpty, nameof(isNullOrEmpty))
                .NotNull(comparer, nameof(comparer));

            if (isNullOrEmpty(target) && isNullOrEmpty(source))
            {
                return default(T);
            }

            if (isNullOrEmpty(target) == false && isNullOrEmpty(source))
            {
                return target;
            }

            if (isNullOrEmpty(target) && isNullOrEmpty(source) == false)
            {
                return emptyValue;
            }

            if (comparer(target, source))
            {
                return default(T);
            }

            return target;
        }

        internal static T CalculateChange<T>(this T target, T source, Predicate<T> isNull, Predicate<T> isNullOrEmpty, T emptyValue, Func<T, T, T> calculateModel)
        {
            NullGuard.NotNull(isNull, nameof(isNull))
                .NotNull(isNullOrEmpty, nameof(isNullOrEmpty))
                .NotNull(calculateModel, nameof(calculateModel));

            if (isNull(target) && isNullOrEmpty(source))
            {
                return default(T);
            }

            if (isNull(target) == false && isNullOrEmpty(source))
            {
                return isNullOrEmpty(target) == false ? target : default(T);
            }

            if (isNull(target))
            {
                return emptyValue;
            }

            return calculateModel(target, source);
        }

        internal static List<T> CalculateChange<T>(this List<T> targetCollection, List<T> sourceCollection, int knownItems, Predicate<T> isNullOrEmpty)
        {
            NullGuard.NotNull(targetCollection, nameof(targetCollection))
                .NotNull(sourceCollection, nameof(sourceCollection))
                .NotNull(isNullOrEmpty, nameof(isNullOrEmpty));

            List<T> result = targetCollection.Where(value => isNullOrEmpty(value) == false).ToList();
            if (result.Count > 0)
            {
                if (sourceCollection.Count > 0 && sourceCollection.Count <= knownItems)
                {
                    return result;
                }

                if (sourceCollection.Count > knownItems)
                {
                    result.AddRange(sourceCollection.Skip(knownItems).Where(value => isNullOrEmpty(value) == false).ToArray());
                    return result;
                }

                return result;
            }

            if (sourceCollection.Count > 0 && sourceCollection.Count <= knownItems)
            {
                return result;
            }

            if (sourceCollection.Count > knownItems)
            {
                result.AddRange(sourceCollection.Skip(knownItems).Where(value => isNullOrEmpty(value) == false).ToArray());
                return result;
            }

            return result;
        }
    }
}
