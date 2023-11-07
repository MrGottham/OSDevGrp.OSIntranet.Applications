using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
	[Migration("20231107000_AlterTitleConstraintOnMediaCoreData")]
	internal class AlterTitleConstraintOnMediaCoreData : Migration
	{
		#region Private constants

		private const string TableName = "MediaCoreData";
		private const string ConstraintName = "IX_MediaCoreData_Title";

		#endregion

		#region Methods

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

			migrationBuilder.DropIndex(ConstraintName, TableName);
			migrationBuilder.AddUniqueConstraint(ConstraintName, TableName, new string[] {"Title", "Subtitle", "MediaTypeIdentifier"});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

			migrationBuilder.DropIndex(ConstraintName, TableName);
			migrationBuilder.AddUniqueConstraint(ConstraintName, TableName, new string[] {"Title", "Subtitle"});
		}

		#endregion
	}
}