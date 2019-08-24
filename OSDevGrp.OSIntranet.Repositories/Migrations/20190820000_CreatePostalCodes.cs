using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20190820000_CreatePostalCodes")]
    internal class CreatePostalCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("PostalCodes",
                table => new
                {
                    CountryCode = table.Column<string>("NVARCHAR(4)", unicode: true, nullable: false, maxLength: 4),
                    PostalCode = table.Column<string>("NVARCHAR(16)", unicode: true, nullable: false, maxLength: 16),
                    City = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    State = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalCodes", m => new {m.CountryCode, m.PostalCode});
                    table.ForeignKey("FK_Countries_CountryCode", m => m.CountryCode, "Countries", "Code", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("PostalCodes");
        }
    }
}
