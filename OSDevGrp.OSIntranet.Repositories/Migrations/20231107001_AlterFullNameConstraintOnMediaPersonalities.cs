using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
	[Migration("20231107001_AlterFullNameConstraintOnMediaPersonalities")]
	internal class AlterFullNameConstraintOnMediaPersonalities : Migration
	{
		#region Private constants

		private const string TableName = "MediaPersonalities";
		private const string ConstraintName = "IX_MediaPersonalities_FullName";

		#endregion

		#region Methods

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

			migrationBuilder.DropIndex(ConstraintName, TableName);
			migrationBuilder.AddUniqueConstraint(ConstraintName, TableName, new string[] {"GivenName", "MiddleName", "Surname", "BirthDate"});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

			migrationBuilder.DropIndex(ConstraintName, TableName);
			migrationBuilder.AddUniqueConstraint(ConstraintName, TableName, new string[] {"GivenName", "MiddleName", "Surname"});
		}

		#endregion
	}
}