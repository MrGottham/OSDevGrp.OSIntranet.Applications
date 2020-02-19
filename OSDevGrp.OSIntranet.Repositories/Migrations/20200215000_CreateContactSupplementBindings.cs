using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20200215000_CreateContactSupplementBindings")]
    internal class CreateContactSupplementBindings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("ContactSupplementBindings",
                table => new
                {
                    ContactSupplementBindingIdentifier = table.Column<int>().Annotation("MySQL:AutoIncrement", true),
                    ContactSupplementIdentifier = table.Column<int>(),
                    ExternalIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactSupplementBindings", m => m.ContactSupplementBindingIdentifier);
                    table.UniqueConstraint("IX_ContactSupplementBindings_ContactSupplement_External", m => new {m.ContactSupplementIdentifier, m.ExternalIdentifier});
                    table.UniqueConstraint("IX_ContactSupplementBindings_ExternalIdentifier", m => m.ExternalIdentifier);
                    table.ForeignKey("FK_ContactSupplements_ContactSupplementIdentifier", m => m.ContactSupplementIdentifier, "ContactSupplements", "ContactSupplementIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("ContactSupplementBindings");
        }
    }
}
