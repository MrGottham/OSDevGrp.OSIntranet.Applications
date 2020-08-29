using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.ModelHandlers.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.ModelHandlers.Contacts
{
    internal class ContactGroupModelHandler : ModelHandlerBase<IContactGroup, ContactContext, ContactGroupModel, int>
    {
        #region Constructor

        public ContactGroupModelHandler(ContactContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override Func<ContactContext, DbSet<ContactGroupModel>> Entities => dbContext => dbContext.ContactGroups;

        protected override Func<IContactGroup, int> PrimaryKey => contactGroup => contactGroup.Number;

        protected override Expression<Func<ContactGroupModel, bool>> EntitySelector(int primaryKey) => contactGroupModel => contactGroupModel.ContactGroupIdentifier == primaryKey;

        #endregion

        #region Methods

        protected override Task<IEnumerable<IContactGroup>> Sort(IEnumerable<IContactGroup> contactGroupCollection)
        {
            NullGuard.NotNull(contactGroupCollection, nameof(contactGroupCollection));

            return Task.FromResult(contactGroupCollection.OrderBy(contactGroup => contactGroup.Number).AsEnumerable());
        }

        protected override async Task<ContactGroupModel> OnReadAsync(ContactGroupModel contactGroupModel)
        {
            NullGuard.NotNull(contactGroupModel, nameof(contactGroupModel));

            contactGroupModel.Deletable = await CanDeleteAsync(contactGroupModel);

            return contactGroupModel;
        }

        protected override Task OnUpdateAsync(IContactGroup contactGroup, ContactGroupModel contactGroupModel)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup))
                .NotNull(contactGroupModel, nameof(contactGroupModel));

            contactGroupModel.Name = contactGroup.Name;

            return Task.CompletedTask;
        }

        protected override async Task<bool> CanDeleteAsync(ContactGroupModel contactGroupModel)
        {
            NullGuard.NotNull(contactGroupModel, nameof(contactGroupModel));

            return await DbContext.ContactSupplements.FirstOrDefaultAsync(contactSupplementModel => contactSupplementModel.ContactGroupIdentifier == contactGroupModel.ContactGroupIdentifier) == null;
        }

        #endregion
    }
}