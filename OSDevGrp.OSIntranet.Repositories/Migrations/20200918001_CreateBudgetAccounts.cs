using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200918001_CreateBudgetAccounts")]
    internal class CreateBudgetAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("BudgetAccounts",
                table => new
                {
                    BudgetAccountIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountingIdentifier = table.Column<int>(),
                    AccountNumber = table.Column<string>("NVARCHAR(16)", unicode: true, nullable: false, maxLength: 16),
                    BasicAccountIdentifier = table.Column<int>(),
                    BudgetAccountGroupIdentifier = table.Column<int>(),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_BudgetAccounts", m => m.BudgetAccountIdentifier);
                    table.UniqueConstraint("IX_BudgetAccounts_AccountingIdentifier_AccountNumber", m => new {m.AccountingIdentifier, m.AccountNumber});
                    table.UniqueConstraint("IX_BudgetAccounts_BasicAccountIdentifier", m => m.BasicAccountIdentifier);
                    table.UniqueConstraint("IX_BudgetAccounts_BudgetAccountGroupIdentifier", m => new {m.BudgetAccountGroupIdentifier, m.AccountingIdentifier, m.AccountNumber});
                    table.ForeignKey("FK_Accountings_BudgetAccountAccountingIdentifier", m => m.AccountingIdentifier, "Accountings", "AccountingIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BasicAccounts_BudgetAccountBasicAccountIdentifier", m => m.BasicAccountIdentifier, "BasicAccounts", "BasicAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BudgetAccountGroups_BudgetAccountGroupIdentifier", m => m.BudgetAccountGroupIdentifier, "BudgetAccountGroups", "BudgetAccountGroupIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE BudgetAccounts MODIFY BudgetAccountIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("BudgetAccounts");
        }
    }
}