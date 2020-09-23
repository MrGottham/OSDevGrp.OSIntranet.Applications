using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200919000_RenameForeignKeyForAccountingLetterHeadIdentifier")]
    internal class RenameForeignKeyForAccountingLetterHeadIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.Sql("ALTER TABLE Accountings DROP FOREIGN KEY FK_LetterHead_AccountingLetterHeadIdentifier");

            migrationBuilder.AddForeignKey("FK_LetterHeads_AccountingLetterHeadIdentifier", "Accountings", "LetterHeadIdentifier", "LetterHeads", principalColumn: "LetterHeadIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.Sql("ALTER TABLE Accountings DROP FOREIGN KEY FK_LetterHeads_AccountingLetterHeadIdentifier");

            migrationBuilder.AddForeignKey("FK_LetterHead_AccountingLetterHeadIdentifier", "Accountings", "LetterHeadIdentifier", "LetterHeads", principalColumn: "LetterHeadIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
        }
    }
}