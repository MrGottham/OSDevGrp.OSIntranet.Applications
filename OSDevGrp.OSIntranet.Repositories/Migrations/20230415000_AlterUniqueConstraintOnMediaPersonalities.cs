using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230415000_AlterUniqueConstraintOnMediaPersonalities")]
    internal class AlterUniqueConstraintOnMediaPersonalities : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

	        migrationBuilder.RecreateUniqueIndex("IX_MediaPersonalities_FullName", "MediaPersonalities", "GivenName", "MiddleName", "Surname", "MediaPersonalityIdentifier");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.RecreateUniqueIndex("IX_MediaPersonalities_FullName", "MediaPersonalities", "GivenName", "MiddleName", "Surname");
        }

		#endregion
	}
}