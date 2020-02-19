﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Contexts
{
    internal class ContactContext : RepositoryContextBase
    {
        #region Constructors

        public ContactContext()
        {
        }

        public ContactContext(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Properties

        public DbSet<ContactSupplementModel> ContactSupplements { get; set; }

        public DbSet<ContactSupplementBindingModel> ContactSupplementBindings { get; set; }

        public DbSet<ContactGroupModel> ContactGroups { get; set; }

        public DbSet<PaymentTermModel> PaymentTerms { get; set; }

        public DbSet<CountryModel> Countries { get; set; }

        public DbSet<PostalCodeModel> PostalCodes { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateContactSupplementModel();
            modelBuilder.CreateContactSupplementBindingModel();
            modelBuilder.CreateContactGroupModel();
            modelBuilder.CreatePaymentTermModel();
            modelBuilder.CreateCountryModel();
            modelBuilder.CreatePostalCodeModel();
        }

        #endregion
    }
}