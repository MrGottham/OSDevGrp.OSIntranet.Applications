using System;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.TestHelpers
{
    public class ErrorCodeAttributeTestHelper
    {
        public ErrorCodeAttribute GetErrorCodeAttribute(ErrorCode errorCode)
        {
            FieldInfo fieldInfo = errorCode.GetType().GetField(Convert.ToString(errorCode), BindingFlags.Static | BindingFlags.Public);
            if (fieldInfo == null)
            {
                throw new MissingFieldException($"Unable to find a field named '{errorCode}' in '{errorCode.GetType().Name}'", Convert.ToString(errorCode));
            }

            return fieldInfo.GetCustomAttributes<ErrorCodeAttribute>().SingleOrDefault();
        }
    }
}
