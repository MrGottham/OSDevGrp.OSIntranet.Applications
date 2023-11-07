using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230415001_AlterUniqueConstraintOnMediaCoreData")]
    internal class AlterUniqueConstraintOnMediaCoreData : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

	        migrationBuilder.RecreateUniqueIndex("IX_MediaCoreData_Title", "MediaCoreData", "Title", "Subtitle", "MediaCoreDataIdentifier");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.RecreateUniqueIndex("IX_MediaCoreData_Title", "MediaCoreData", "Title", "Subtitle");
        }

		#endregion
	}
}