using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20201204002_AddBudgetInfos")]
    internal class AddBudgetInfos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("BudgetInfos",
                table => new
                {
                    BudgetInfoIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BudgetAccountIdentifier = table.Column<int>(),
                    YearMonthIdentifier = table.Column<int>(),
                    Income = table.Column<decimal>("DECIMAL(10,2)"),
                    Expenses = table.Column<decimal>("DECIMAL(10,2)"),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetInfos", m => m.BudgetInfoIdentifier);
                    table.UniqueConstraint("IX_BudgetInfos_BudgetAccountIdentifier_YearMonthIdentifier", m => new {m.BudgetAccountIdentifier, m.YearMonthIdentifier});
                    table.ForeignKey("FK_BudgetAccounts_BudgetInfoBudgetAccountIdentifier", m => m.BudgetAccountIdentifier, "BudgetAccounts", "BudgetAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_YearMonths_BudgetInfoYearMonthIdentifier", m => m.YearMonthIdentifier, "YearMonths", "YearMonthIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE BudgetInfos MODIFY BudgetInfoIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("BudgetInfos");
        }
    }
}