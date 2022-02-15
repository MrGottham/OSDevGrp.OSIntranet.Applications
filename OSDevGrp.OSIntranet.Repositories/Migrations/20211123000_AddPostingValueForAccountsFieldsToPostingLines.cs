using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20211123000_AddPostingValueForAccountsFieldsToPostingLines")]
    internal class AddPostingValueForAccountsFieldsToPostingLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.AddColumn<decimal>("PostingValueForAccount", "PostingLines", "DECIMAL(10,2)", nullable: false);
            migrationBuilder.AddColumn<decimal>("PostingValueForBudgetAccount", "PostingLines", "DECIMAL(10,2)", nullable: true);
            migrationBuilder.AddColumn<decimal>("PostingValueForContactAccount", "PostingLines", "DECIMAL(10,2)", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropColumn("PostingValueForAccount", "PostingLines");
            migrationBuilder.DropColumn("PostingValueForBudgetAccount", "PostingLines");
            migrationBuilder.DropColumn("PostingValueForContactAccount", "PostingLines");
        }
    }
}