using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20190323002_SecurityRelations")]
    public class SecurityRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("UserIdentityClaims",
                table => new
                {
                    UserIdentityClaimIdentifier = table.Column<int>().Annotation("MySQL:AutoIncrement", true),
                    UserIdentityIdentifier = table.Column<int>(),
                    ClaimIdentifier = table.Column<int>(),
                    ClaimValue = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentityClaims", m => m.UserIdentityClaimIdentifier);
                    table.UniqueConstraint("IX_UserIdentityClaims_IdentityIdentifier_ClaimIdentifier", m => new {m.UserIdentityIdentifier, m.ClaimIdentifier});
                    table.ForeignKey("FK_UserIdentities_UserIdentityIdentifier", m => m.UserIdentityIdentifier, "UserIdentities", "UserIdentityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Claims_UserClaimIdentifier", m => m.ClaimIdentifier, "Claims", "ClaimIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable("ClientSecretIdentityClaims",
                table => new
                {
                    ClientSecretIdentityClaimIdentifier = table.Column<int>().Annotation("MySQL:AutoIncrement", true),
                    ClientSecretIdentityIdentifier = table.Column<int>(),
                    ClaimIdentifier = table.Column<int>(),
                    ClaimValue = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecretIdentityClaims", m => m.ClientSecretIdentityClaimIdentifier);
                    table.UniqueConstraint("IX_ClientSecretIdentityClaims_IdentityIdentifier_ClaimIdentifier", m => new {m.ClientSecretIdentityIdentifier, m.ClaimIdentifier});
                    table.ForeignKey("FK_ClientSecretIdentities_ClientSecretIdentityIdentifier", m => m.ClientSecretIdentityIdentifier, "ClientSecretIdentities", "ClientSecretIdentityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Claims_ClientSecretClaimIdentifier", m => m.ClaimIdentifier, "Claims", "ClaimIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            InsertUserIdentityClaimModels(migrationBuilder);
            InsertClientSecretIdentityClaimModels(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("UserIdentityClaims");
            migrationBuilder.DropTable("ClientSecretIdentityClaims");
        }

        private void InsertUserIdentityClaimModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.Sql(GetUserIdentityClaimModelInsertSql("mrgottham@gmail.com", ClaimHelper.SecurityAdminClaimType));
            migrationBuilder.Sql(GetUserIdentityClaimModelInsertSql("mrgottham@gmail.com", ClaimHelper.AccountingClaimType, "1"));
        }

        private void InsertClientSecretIdentityClaimModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.Sql(GetClientSecretIdentityClaimModelInsertSql("OSDevGrp.OSIntranet.Test", ClaimHelper.AccountingClaimType));
        }

        private string GetUserIdentityClaimModelInsertSql(string externalUserIdentifier, string claimType, string claimValue = null)
        {
            NullGuard.NotNullOrWhiteSpace(externalUserIdentifier, nameof(externalUserIdentifier))
                .NotNullOrWhiteSpace(claimType, nameof(claimType));

            string sqlValue = string.IsNullOrWhiteSpace(claimValue) ? "NULL" : $"'{claimValue}'";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO UserIdentityClaims (UserIdentityIdentifier,ClaimIdentifier,ClaimValue)");
            sqlBuilder.AppendLine($"SELECT userIdentities.UserIdentityIdentifier,claims.ClaimIdentifier,{sqlValue}");
            sqlBuilder.AppendLine("FROM UserIdentities AS userIdentities, Claims AS claims");
            sqlBuilder.AppendLine($"WHERE userIdentities.ExternalUserIdentifier='{externalUserIdentifier}' AND claims.ClaimType='{claimType}'");

            return sqlBuilder.ToString();
        }

        private string GetClientSecretIdentityClaimModelInsertSql(string friendlyName, string claimType, string claimValue = null)
        {
            NullGuard.NotNullOrWhiteSpace(friendlyName, nameof(friendlyName))
                .NotNullOrWhiteSpace(claimType, nameof(claimType));

            string sqlValue = string.IsNullOrWhiteSpace(claimValue) ? "NULL" : $"'{claimValue}'";

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("INSERT INTO ClientSecretIdentityClaims (ClientSecretIdentityIdentifier,ClaimIdentifier,ClaimValue)");
            sqlBuilder.AppendLine($"SELECT clientSecretIdentities.ClientSecretIdentityIdentifier,claims.ClaimIdentifier,{sqlValue}");
            sqlBuilder.AppendLine("FROM ClientSecretIdentities AS clientSecretIdentities, Claims AS claims");
            sqlBuilder.AppendLine($"WHERE clientSecretIdentities.FriendlyName='{friendlyName}' AND claims.ClaimType='{claimType}'");

            return sqlBuilder.ToString();
        }
    }
}