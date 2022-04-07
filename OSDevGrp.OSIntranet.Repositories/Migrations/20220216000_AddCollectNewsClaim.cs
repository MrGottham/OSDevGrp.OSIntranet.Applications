using System;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20220216000_AddCollectNewsClaim")]
    internal class AddCollectNewsClaim : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            Claim collectNewsClaim = BuildCollectNewsClaim();
            DateTime createdUtcDateTime = DateTime.UtcNow;

            migrationBuilder.InsertData("Claims", new[] {"ClaimType", "ClaimValue", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier"}, new[] {"varchar(256)", "varchar(256)", "datetime", "varchar(256)", "'datetime'", "varchar(256)"}, new object[] {collectNewsClaim.Type, collectNewsClaim.Value, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier});

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO UserIdentityClaims (UserIdentityIdentifier, ClaimIdentifier, ClaimValue, CreatedUtcDateTime, CreatedByIdentifier, ModifiedUtcDateTime, ModifiedByIdentifier)");
            sqlBuilder.AppendLine("SELECT userIdentity.UserIdentityIdentifier, claim.ClaimIdentifier, claim.ClaimValue, claim.CreatedUtcDateTime, claim.CreatedByIdentifier, claim.ModifiedUtcDateTime, claim.ModifiedByIdentifier");
            sqlBuilder.AppendLine("FROM UserIdentities AS userIdentity, Claims claim");
            sqlBuilder.AppendLine("WHERE");
            sqlBuilder.AppendLine($"claim.ClaimType = '{collectNewsClaim.Type}'");

            migrationBuilder.Sql(sqlBuilder.ToString());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            Claim collectNewsClaim = BuildCollectNewsClaim();

            RemoveCollectNewsClaimRelations(migrationBuilder, collectNewsClaim, "UserIdentityClaims");
            RemoveCollectNewsClaimRelations(migrationBuilder, collectNewsClaim, "ClientSecretIdentityClaims");

            migrationBuilder.DeleteData("Claims", "ClaimType", collectNewsClaim.Type);
        }

        private static Claim BuildCollectNewsClaim()
        {
            return ClaimHelper.CreateCollectNewsClaim(10);
        }

        private static void RemoveCollectNewsClaimRelations(MigrationBuilder migrationBuilder, Claim collectNewsClaim, string tableName)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder))
                .NotNull(collectNewsClaim, nameof(collectNewsClaim))
                .NotNullOrWhiteSpace(tableName, nameof(tableName));

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("DELETE entity");
            sqlBuilder.AppendLine($"FROM {tableName} AS entity");
            sqlBuilder.AppendLine("INNER JOIN Claims AS claim ON claim.ClaimIdentifier = entity.ClaimIdentifier");
            sqlBuilder.AppendLine("WHERE");
            sqlBuilder.AppendLine($"claim.ClaimType = '{collectNewsClaim.Type}'");

            migrationBuilder.Sql(sqlBuilder.ToString());
        }

        #endregion
    }
}