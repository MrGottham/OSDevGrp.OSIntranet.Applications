using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20190722000_CreateAccountings")]
    internal class CreateAccountings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Accountings",
                table => new
                {
                    AccountingIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    LetterHeadIdentifier = table.Column<int>(),
                    BalanceBelowZero = table.Column<BalanceBelowZeroType>("TINYINT"),
                    BackDating = table.Column<int>(),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accountings", m => m.AccountingIdentifier);
                    table.UniqueConstraint("IX_Accountings_LetterHeadIdentifier_AccountingIdentifier", m => new {m.LetterHeadIdentifier, m.AccountingIdentifier});
                    table.ForeignKey("FK_LetterHead_AccountingLetterHeadIdentifier", m => m.LetterHeadIdentifier, "LetterHeads", "LetterHeadIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Accountings");
        }
    }
}