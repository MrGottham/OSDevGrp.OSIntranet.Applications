using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20200103000_CreateContactSupplements")]
    internal class CreateContactSupplements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("ContactSupplements",
                table => new
                {
                    ContactSupplementIdentifier = table.Column<int>().Annotation("MySQL:AutoIncrement", true),
                    ExternalIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    Birthday = table.Column<DateTime>("DATE", nullable: true),
                    ContactGroupIdentifier = table.Column<int>(),
                    Acquaintance = table.Column<string>("NVARCHAR(4096)", unicode: true, nullable: true, maxLength: 4096),
                    PersonalHomePage = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
                    LendingLimit = table.Column<int>(),
                    PaymentTermIdentifier = table.Column<int>(),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSupplements", m => m.ContactSupplementIdentifier);
                    table.UniqueConstraint("IX_ContactSupplements_ExternalIdentifier", m => m.ExternalIdentifier);
                    table.ForeignKey("FK_ContactGroups_ContactGroupIdentifier", m => m.ContactGroupIdentifier, "ContactGroups", "ContactGroupIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_PaymentTerms_ContactSupplementPaymentTermIdentifier", m => m.PaymentTermIdentifier, "PaymentTerms", "PaymentTermIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("ContactSupplements");
        }
    }
}
