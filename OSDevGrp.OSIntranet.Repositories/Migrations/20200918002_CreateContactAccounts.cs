using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200918002_CreateContactAccounts")]
    internal class CreateContactAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("ContactAccounts",
                table => new
                {
                    ContactAccountIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountingIdentifier = table.Column<int>(),
                    AccountNumber = table.Column<string>("NVARCHAR(16)", unicode: true, nullable: false, maxLength: 16),
                    BasicAccountIdentifier = table.Column<int>(),
                    PaymentTermIdentifier = table.Column<int>(),
                    MailAddress = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
                    PrimaryPhone = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
                    SecondaryPhone = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table => 
                {
                    table.PrimaryKey("PK_ContactAccounts", m => m.ContactAccountIdentifier);
                    table.UniqueConstraint("IX_ContactAccounts_AccountingIdentifier_AccountNumber", m => new {m.AccountingIdentifier, m.AccountNumber});
                    table.UniqueConstraint("IX_ContactAccounts_BasicAccountIdentifier", m => m.BasicAccountIdentifier);
                    table.UniqueConstraint("IX_ContactAccounts_PaymentTermIdentifier", m => new {m.PaymentTermIdentifier, m.AccountingIdentifier, m.AccountNumber});
                    table.ForeignKey("FK_Accountings_ContactAccountAccountingIdentifier", m => m.AccountingIdentifier, "Accountings", "AccountingIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_BasicAccounts_ContactAccountBasicAccountIdentifier", m => m.BasicAccountIdentifier, "BasicAccounts", "BasicAccountIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_PaymentTerms_ContactAccountPaymentTermIdentifier", m => m.PaymentTermIdentifier, "PaymentTerms", "PaymentTermIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("ALTER TABLE ContactAccounts MODIFY ContactAccountIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("ContactAccounts");
        }
    }
}