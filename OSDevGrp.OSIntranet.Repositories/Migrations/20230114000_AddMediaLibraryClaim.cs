using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20230114000_AddMediaLibraryClaim")]
    internal class AddMediaLibraryClaim : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            Claim madiaLibraryClaim = ClaimHelper.CreateMediaLibraryClaim();
            DateTime createdUtcDateTime = DateTime.UtcNow;

            migrationBuilder.InsertData("Claims", new[] { "ClaimType", "ClaimValue", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier" }, new[] { "varchar(256)", "varchar(256)", "datetime", "varchar(256)", "'datetime'", "varchar(256)" }, new object[] { madiaLibraryClaim.Type, madiaLibraryClaim.Value, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            Claim madiaLibraryClaim = ClaimHelper.CreateMediaLibraryClaim();

            migrationBuilder.DeleteData("Claims", "ClaimType", madiaLibraryClaim.Type);
        }

        #endregion
    }
}