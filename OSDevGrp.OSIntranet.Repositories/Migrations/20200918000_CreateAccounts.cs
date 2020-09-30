using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200918000_CreateAccounts")]
    internal class CreateAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Accounts",
                table => new
                {
                    AccountIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountingIdentifier = table.Column<int>(),
                    AccountNumber = table.Column<string>("NVARCHAR(16)", unicode: true, nullable: false, maxLength: 16),
                    BasicAccountIdentifier = table.Column<int>(),
                    AccountGroupIdentifier = table.Column<int>(),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_Accounts", m => m.AccountIdentifier);
                    table.UniqueConstraint("IX_Accounts_AccountingIdentifier_AccountNumber", m => new {m.AccountingIdentifier, m.AccountNumber});
                    table.UniqueConstraint("IX_Accounts_BasicAccountIdentifier", m => m.BasicAccountIdentifier);
                    table.UniqueConstraint("IX_Accounts_AccountGroupIdentifier", m => new {m.AccountGroupIdentifier, m.AccountingIdentifier, m.AccountNumber});
                    table.ForeignKey("FK_Accountings_AccountAccountingIdentifier", m => m.AccountingIdentifier, "Accountings", "AccountingIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BasicAccounts_AccountBasicAccountIdentifier", m => m.BasicAccountIdentifier, "BasicAccounts", "BasicAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_AccountGroups_AccountGroupIdentifier", m => m.AccountGroupIdentifier, "AccountGroups", "AccountGroupIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE Accounts MODIFY AccountIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Accounts");
        }
    }
}