using System;
using AutoMapper;

namespace OSDevGrp.OSIntranet.Core
{
    public class NullableDateTimeResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, DateTime?>
    {
        #region Privat variables

        private readonly Func<TSource, DateTime?> _valueGetter;

        #endregion

        #region Constructor

        public NullableDateTimeResolver(Func<TSource, DateTime?> valueGetter)
        {
            NullGuard.NotNull(valueGetter, nameof(valueGetter));

            _valueGetter = valueGetter;
        }

        #endregion

        #region Methods

        public DateTime? Resolve(TSource src, TDestination dest, DateTime? destMember, ResolutionContext context)
        {
            NullGuard.NotNull(src, nameof(src))
                .NotNull(dest, nameof(dest))
                .NotNull(context, nameof(context));

            return _valueGetter(src);
        }

        #endregion
    }
}