using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20230103000_AddExtraAccountingClaims")]
    internal class AddExtraAccountingClaims : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            DateTime createdUtcDateTime = DateTime.UtcNow;
            foreach (Claim claim in GetClaims())
            {
                migrationBuilder.InsertData("Claims", new[] { "ClaimType", "ClaimValue", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier" }, new[] { "varchar(256)", "varchar(256)", "datetime", "varchar(256)", "'datetime'", "varchar(256)" }, new object[] { claim.Type, claim.Value, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            foreach (Claim claim in GetClaims())
            {
                migrationBuilder.DeleteData("Claims", "ClaimType", claim.Type);
            }
        }

        private static IEnumerable<Claim> GetClaims()
        {
            return new List<Claim>
            {
                ClaimHelper.CreateAccountingAdministratorClaim(),
                ClaimHelper.CreateAccountingCreatorClaim(),
                ClaimHelper.CreateAccountingModifierClaim(true, 1, 2, 3),
                ClaimHelper.CreateAccountingViewerClaim(true, 1, 2, 3)
            };
        }

        #endregion
    }
}