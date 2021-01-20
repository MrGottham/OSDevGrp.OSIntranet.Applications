using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Accounting
{
    public abstract class InfoBase<T> : AuditableBase, IInfo<T> where T : IInfo
    {
        #region Public constants

        public const short MinYear = 1950;
        public const short MaxYear = 2199;
        public const short MinMonth = 1;
        public const short MaxMonth = 12;

        #endregion

        #region Constructor

        protected InfoBase(short year, short month)
        {
            if (year < MinYear || year > MaxYear)
            {
                throw new ArgumentException($"Year should be between {MinYear} and {MaxYear}.", nameof(year));
            }

            if (month < MinMonth || month > MaxMonth)
            {
                throw new ArgumentException($"Month should be between {MinMonth} and {MaxMonth}.", nameof(month));
            }

            Year = year;
            Month = month;
            FromDate = new DateTime(Year, Month, 1, 0, 0, 0, DateTimeKind.Local);
            ToDate = new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month), 0, 0, 0, DateTimeKind.Local);
        }

        #endregion

        #region Properties

        public short Year { get; }

        public short Month { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime StatusDate { get; private set; }

        public bool IsMonthOfStatusDate => StatusDate.Year == Year && StatusDate.Month == Month;

        public bool IsLastMonthOfStatusDate => FirstDateOfMonth(StatusDate).AddDays(-1).Year == Year && FirstDateOfMonth(StatusDate).AddDays(-1).Month == Month;

        public bool IsYearToDateOfStatusDate => StatusDate.Year == Year && StatusDate.Month >= Month;

        public bool IsLastYearOfStatusDate => FirstDateOfMonth(StatusDate).AddYears(-1).Year == Year;

        public bool Deletable { get; private set; }

        #endregion

        #region Methods

        public Task<T> CalculateAsync(DateTime statusDate)
        {
            return Task.Run(() =>
            {
                if (statusDate.Date == StatusDate)
                {
                    return AlreadyCalculated();
                }

                StatusDate = statusDate.Date;

                return Calculate(StatusDate);
            });
        }

        public void AllowDeletion()
        {
            Deletable = true;
        }

        public void DisallowDeletion()
        {
            Deletable = false;
        }

        public override int GetHashCode()
        {
            return $"{Year}-{Month}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is IInfo info)
            {
                return info.Year == Year && info.Month == Month;
            }

            return false;
        }

        protected abstract T Calculate(DateTime statusDate);

        protected abstract T AlreadyCalculated();

        protected DateTime ResolveCalculationToDate(DateTime statusDate)
        {
            return statusDate.Date < ToDate ? statusDate.Date : ToDate;
        }

        private static DateTime FirstDateOfMonth(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1).Date;
        }

        #endregion
    }
}