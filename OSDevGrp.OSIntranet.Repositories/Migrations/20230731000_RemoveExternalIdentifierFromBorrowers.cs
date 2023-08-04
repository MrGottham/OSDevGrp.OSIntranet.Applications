using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230731000_RemoveExternalIdentifierFromBorrowers")]
    internal class RemoveExternalIdentifierFromBorrowers : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropIndex("IX_Borrowers_ExternalIdentifier_BorrowerIdentifier", "Borrowers");
            migrationBuilder.DropColumn("ExternalIdentifier", "Borrowers");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.AddColumn<string>("ExternalIdentifier", "Borrowers", "VARBINARY(256)", unicode: true, nullable: true, maxLength: 256);
            migrationBuilder.AddUniqueConstraint("IX_Borrowers_ExternalIdentifier_BorrowerIdentifier", "Borrowers", new[] {"ExternalIdentifier", "BorrowerIdentifier"});
        }

		#endregion
	}
}