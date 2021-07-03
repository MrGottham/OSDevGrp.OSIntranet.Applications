using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20210625000_AddPostingLines")]
    internal class AddPostingLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("PostingLines",
                table => new
                {
                    PostingLineIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PostingLineIdentification = table.Column<string>("CHAR(36)", nullable: false),
                    AccountingIdentifier = table.Column<int>(),
                    PostingDate = table.Column<DateTime>("DATETIME", nullable: false),
                    Reference = table.Column<string>("NVARCHAR(16)", unicode: true, nullable: true, maxLength: 16),
                    AccountIdentifier = table.Column<int>(),
                    Details = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    BudgetAccountIdentifier = table.Column<int>(nullable: true),
                    Debit = table.Column<decimal>("DECIMAL(10,2)", nullable: true),
                    Credit = table.Column<decimal>("DECIMAL(10,2)", nullable: true),
                    ContactAccountIdentifier = table.Column<int>(nullable: true),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingLines", m => m.PostingLineIdentifier);
                    table.UniqueConstraint("IX_PostingLines_PostingLineIdentification", m => m.PostingLineIdentification);
                    table.UniqueConstraint("IX_PostingLines_AccountingIdentifier_Date_LineIdentifier", m => new {m.AccountingIdentifier, m.PostingDate, m.PostingLineIdentifier});
                    table.UniqueConstraint("IX_PostingLines_Date_LineIdentifier", m => new {m.PostingDate, m.PostingLineIdentifier});
                    table.UniqueConstraint("IX_PostingLines_AccountIdentifier_Date_LineIdentifier", m => new {m.AccountIdentifier, m.PostingDate, m.PostingLineIdentifier});
                    table.UniqueConstraint("IX_PostingLines_BudgetAccountIdentifier_Date_LineIdentifier", m => new {m.BudgetAccountIdentifier, m.PostingDate, m.PostingLineIdentifier});
                    table.UniqueConstraint("IX_PostingLines_ContactAccountIdentifier_Date_LineIdentifier", m => new {m.ContactAccountIdentifier, m.PostingDate, m.PostingLineIdentifier});
                    table.ForeignKey("FK_Accountings_PostingLineAccountingIdentifier", m => m.AccountingIdentifier, "Accountings", "AccountingIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Accounts_PostingLineAccountIdentifier", m => m.AccountIdentifier, "Accounts", "AccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BudgetAccounts_PostingLineBudgetAccountIdentifier", m => m.BudgetAccountIdentifier, "BudgetAccounts", "BudgetAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_ContactAccounts_PostingLineContactAccountIdentifier", m => m.ContactAccountIdentifier, "ContactAccounts", "ContactAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE PostingLines MODIFY PostingLineIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("PostingLines");
        }
    }
}