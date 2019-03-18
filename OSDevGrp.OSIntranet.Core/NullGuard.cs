using System;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Core
{
    public class NullGuard : INullGuard
    {
        INullGuard INullGuard.NotNull(object value, string argumentName)
        {
            return Validate(value, argumentName, v => v == null);
        }

        INullGuard INullGuard.NotNullOrEmpty(string value, string argumentName)
        {
            return Validate(value, argumentName, v => string.IsNullOrEmpty(value));
        }

        INullGuard INullGuard.NotNullOrWhiteSpace(string value, string argumentName)
        {
            return Validate(value, argumentName, v => string.IsNullOrWhiteSpace(value));
        }

        private INullGuard Validate<T>(T value, string argumentName, Func<T, bool> validateCallback)
        {
            if (string.IsNullOrWhiteSpace(argumentName))
            {
                throw new ArgumentNullException(nameof(argumentName));
            }
            if (validateCallback == null)
            {
                throw new ArgumentNullException(nameof(validateCallback));
            }

            if (validateCallback(value))
            {
                throw new ArgumentNullException(argumentName);
            }

            return this;
        }

        public static INullGuard NotNull(object value, string argumentName)
        {
            return Create().NotNull(value, argumentName);
        }

        public static INullGuard NotNullOrEmpty(string value, string argumentName)
        {
            return Create().NotNullOrEmpty(value, argumentName);
        }

        public static INullGuard NotNullOrWhiteSpace(string value, string argumentName)
        {
            return Create().NotNullOrWhiteSpace(value, argumentName);
        }

        private static INullGuard Create()
        {
            return new NullGuard();
        }
    }
}
