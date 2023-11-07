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
    [Migration("20230513000_AddExtraMediaLibraryClaims")]
    internal class AddExtraMediaLibraryClaims : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            DateTime createdUtcDateTime = DateTime.UtcNow;
            foreach (Claim extraMediaLibraryClaim in GetExtraMediaLibraryClaims())
            {
	            migrationBuilder.InsertData("Claims", new[] { "ClaimType", "ClaimValue", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier" }, new[] { "varchar(256)", "varchar(256)", "datetime", "varchar(256)", "'datetime'", "varchar(256)" }, new object[] { extraMediaLibraryClaim.Type, extraMediaLibraryClaim.Value, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            foreach (Claim extraMediaLibraryClaim in GetExtraMediaLibraryClaims())
            {
	            migrationBuilder.DeleteData("Claims", "ClaimType", extraMediaLibraryClaim.Type);
            }
        }

        private static IEnumerable<Claim> GetExtraMediaLibraryClaims()
        {
	        return new[]
	        {
		        ClaimHelper.CreateMediaLibraryModifierClaim(),
		        ClaimHelper.CreateMediaLibraryLenderClaim()
	        };
        }

		#endregion
	}
}