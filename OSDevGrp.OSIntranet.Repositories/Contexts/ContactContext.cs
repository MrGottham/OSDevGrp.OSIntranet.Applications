﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class ContactContext : RepositoryContextBase
    {
        #region Constructors

        public ContactContext()
        {
        }

        public ContactContext(IConfiguration configuration, IPrincipalResolver principalResolver)
            : base(configuration, principalResolver)
        {
        }

        #endregion

        #region Properties

        public DbSet<CountryModel> Countries { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateCountryModel();
        }

        #endregion
    }
}
