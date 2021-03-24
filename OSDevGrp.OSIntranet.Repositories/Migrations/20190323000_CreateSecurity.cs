using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20190323000_CreateSecurity")]
    internal class CreateSecurity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("UserIdentities",
                table => new
                {
                    UserIdentityIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ExternalUserIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIdentities", m => m.UserIdentityIdentifier);
                    table.UniqueConstraint("IX_UserIdentities_ExternalUserIdentifier", m => m.ExternalUserIdentifier);
                });

            migrationBuilder.Sql("ALTER TABLE UserIdentities MODIFY UserIdentityIdentifier INT NOT NULL AUTO_INCREMENT");

            migrationBuilder.CreateTable("ClientSecretIdentities",
                table => new
                {
                    ClientSecretIdentityIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ClientId = table.Column<string>("CHAR(32)", unicode: true, nullable: false, maxLength: 32),
                    ClientSecret = table.Column<string>("CHAR(32)", unicode: true, nullable: false, maxLength: 32)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecretIdentities", m => m.ClientSecretIdentityIdentifier);
                    table.UniqueConstraint("IX_ClientSecretIdentities_FriendlyName", m => m.FriendlyName);
                    table.UniqueConstraint("IX_ClientSecretIdentities_ClientId", m => m.ClientId);
                });

            migrationBuilder.Sql("ALTER TABLE ClientSecretIdentities MODIFY ClientSecretIdentityIdentifier INT NOT NULL AUTO_INCREMENT");

            InsertUserIdentityModels(migrationBuilder);
            InsertClientSecretModels(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("UserIdentities");
            migrationBuilder.DropTable("ClientSecretIdentities");
        }

        private void InsertUserIdentityModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            IEnumerable<IUserIdentity> userIdentityCollection = new List<IUserIdentity>
            {
                new UserIdentityBuilder("mrgottham@gmail.com").Build(),
            };
            foreach (IUserIdentity userIdentity in userIdentityCollection)
            {
                migrationBuilder.InsertData("UserIdentities", new[] {"ExternalUserIdentifier"}, new object[] {userIdentity.ExternalUserIdentifier});
            }
        }

        private void InsertClientSecretModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            IClientSecretIdentity clientSecretIdentity = new ClientSecretIdentityBuilder("OSDevGrp.OSIntranet.Test").Build();

            migrationBuilder.InsertData("ClientSecretIdentities", new[] {"FriendlyName", "ClientId", "ClientSecret"}, new object[] {clientSecretIdentity.FriendlyName, clientSecretIdentity.ClientId, clientSecretIdentity.ClientSecret});
        }
    }
}