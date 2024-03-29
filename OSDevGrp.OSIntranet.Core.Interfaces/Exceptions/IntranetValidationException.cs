﻿using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using System;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Exceptions
{
	public class IntranetValidationException : IntranetBusinessException
    {
        #region Private variables

        private Type _validatingType;
        private string _validatingField;

        #endregion

        #region Constructors

        public IntranetValidationException(ErrorCode errorCode, string message)
            : base(errorCode, message)
        {
        }

        public IntranetValidationException(ErrorCode errorCode, string message, Exception innerException)
            : base(errorCode, message, innerException)
        {
        }

        #endregion

        #region Properties

        public Type ValidatingType
        {
            get => _validatingType;
            set => _validatingType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string ValidatingField
        {
            get => _validatingField;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _validatingField = value;
            }
        }

        #endregion
    }
}