using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(CommonContext))]
    [Migration("20190504000_CreateLetterHeads")]
    internal class CreateLetterHeads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("LetterHeads",
                table => new
                {
                    LetterHeadIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    Line1 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: false, maxLength: 64),
                    Line2 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    Line3 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    Line4 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    Line5 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    Line6 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    Line7 = table.Column<string>("NVARCHAR(64)", unicode: true, nullable: true, maxLength: 64),
                    CompanyIdentificationNumber = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterHeads", m => m.LetterHeadIdentifier);
                });
        }
 
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("LetterHeads");
        }
    }
}