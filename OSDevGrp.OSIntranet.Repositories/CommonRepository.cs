using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class CommonRepository : RepositoryBase, ICommonRepository
    {
        #region Private variables

        private readonly IConverter _commonModelConverter = new CommonModelConverter();

        #endregion

        #region Constructor

        public CommonRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<ILetterHead>> GetLetterHeadsAsync()
        {
            return Task.Run(() => GetLetterHeads());
        }

        public Task<ILetterHead> GetLetterHeadAsync(int number)
        {
            return Task.Run(() => GetLetterHead(number));
        }

        public Task<ILetterHead> CreateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return Task.Run(() => CreateLetterHead(letterHead));
        }

        public Task<ILetterHead> UpdateLetterHeadAsync(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return Task.Run(() => UpdateLetterHead(letterHead));
        }

        public Task<ILetterHead> DeleteLetterHeadAsync(int number)
        {
            return Task.Run(() => DeleteLetterHead(number));
        }

        private IEnumerable<ILetterHead> GetLetterHeads()
        {
            return Execute(() =>
                {
                    using (CommonContext context = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        return context.LetterHeads.AsParallel()
                            .Select(letterHeadModel => 
                            {
                                using (CommonContext subContext = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                                {
                                    letterHeadModel.Deletable = CanDeleteLetterHead(subContext, letterHeadModel.LetterHeadIdentifier);
                                }

                                return _commonModelConverter.Convert<LetterHeadModel, ILetterHead>(letterHeadModel);
                            })
                            .OrderBy(letterHead => letterHead.Number)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ILetterHead GetLetterHead(int number)
        {
            return Execute(() =>
                {
                    using (CommonContext context = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        LetterHeadModel letterHeadModel = context.LetterHeads.Find(number);
                        if (letterHeadModel == null)
                        {
                            return null;
                        }

                        letterHeadModel.Deletable = CanDeleteLetterHead(context, letterHeadModel.LetterHeadIdentifier);

                        return  _commonModelConverter.Convert<LetterHeadModel, ILetterHead>(letterHeadModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ILetterHead CreateLetterHead(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return Execute(() =>
                {
                    using (CommonContext context = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        LetterHeadModel letterHeadModel = _commonModelConverter.Convert<ILetterHead, LetterHeadModel>(letterHead);

                        context.LetterHeads.Add(letterHeadModel);

                        context.SaveChanges();

                        return GetLetterHead(letterHead.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ILetterHead UpdateLetterHead(ILetterHead letterHead)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead));

            return Execute(() =>
                {
                    using (CommonContext context = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        LetterHeadModel letterHeadModel = context.LetterHeads.Find(letterHead.Number);
                        if (letterHeadModel == null)
                        {
                            return null;
                        }

                        letterHeadModel.Name = letterHead.Name;
                        letterHeadModel.Line1 = letterHead.Line1;
                        letterHeadModel.Line2 = letterHead.Line2;
                        letterHeadModel.Line3 = letterHead.Line3;
                        letterHeadModel.Line4 = letterHead.Line4;
                        letterHeadModel.Line5 = letterHead.Line5;
                        letterHeadModel.Line6 = letterHead.Line6;
                        letterHeadModel.Line7 = letterHead.Line7;
                        letterHeadModel.CompanyIdentificationNumber = letterHead.CompanyIdentificationNumber;

                        context.SaveChanges();

                        return GetLetterHead(letterHead.Number);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ILetterHead DeleteLetterHead(int number)
        {
            return Execute(() =>
                {
                    using (CommonContext context = new CommonContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        LetterHeadModel letterHeadModel = context.LetterHeads.Find(number);
                        if (letterHeadModel == null)
                        {
                            return null;
                        }

                        if (CanDeleteLetterHead(context, letterHeadModel.LetterHeadIdentifier) == false)
                        {
                            return GetLetterHead(letterHeadModel.LetterHeadIdentifier);
                        }

                        context.LetterHeads.Remove(letterHeadModel);

                        context.SaveChanges();

                        return null;
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteLetterHead(CommonContext context, int letterHeadIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return false;
        }

        #endregion
    }
}