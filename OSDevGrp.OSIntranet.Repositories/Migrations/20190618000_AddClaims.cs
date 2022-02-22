using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20190618000_AddClaims")]
    internal class AddClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            DateTime createdUtcDateTime = DateTime.UtcNow;
            foreach (Claim claim in GetClaims())
            {
                migrationBuilder.InsertData("Claims", new[] {"ClaimType", "ClaimValue", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier"}, new object[] {claim.Type, claim.Value, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier});
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

        private IEnumerable<Claim> GetClaims()
        {
            return new List<Claim>
            {
                ClaimHelper.CreateCommonDataClaim(),
                ClaimHelper.CreateContactsClaim(),
                ClaimHelper.CreateCountryCodeClaim("DK")
            };
        }
    }
}