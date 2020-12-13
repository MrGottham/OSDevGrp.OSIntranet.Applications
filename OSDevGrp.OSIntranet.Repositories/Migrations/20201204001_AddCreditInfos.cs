using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20201204001_AddCreditInfos")]
    internal class AddCreditInfos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("CreditInfos",
                table => new
                {
                    CreditInfoIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountIdentifier = table.Column<int>(),
                    YearMonthIdentifier = table.Column<int>(),
                    Credit = table.Column<decimal>("DECIMAL(10,2)"),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditInfos", m => m.CreditInfoIdentifier);
                    table.UniqueConstraint("IX_CreditInfos_AccountIdentifier_YearMonthIdentifier", m => new {m.AccountIdentifier, m.YearMonthIdentifier});
                    table.ForeignKey("FK_Accounts_CreditInfoAccountIdentifier", m => m.AccountIdentifier, "Accounts", "AccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_YearMonths_CreditInfoYearMonthIdentifier", m => m.YearMonthIdentifier, "YearMonths", "YearMonthIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE CreditInfos MODIFY CreditInfoIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("CreditInfos");
        }
    }
}