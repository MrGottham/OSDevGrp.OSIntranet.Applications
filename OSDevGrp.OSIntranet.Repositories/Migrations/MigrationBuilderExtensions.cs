using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	internal static class MigrationBuilderExtensions
	{
		#region Methods

		internal static void RecreateUniqueIndex(this MigrationBuilder migrationBuilder, string name, string table, params string[] columns)
		{
			NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder))
				.NotNullOrWhiteSpace(name, nameof(name))
				.NotNullOrWhiteSpace(table, nameof(table))
				.NotNull(columns, nameof(columns));

			migrationBuilder.DropIndex(name, table);
			migrationBuilder.CreateIndex(name, table, columns, unique: true);
		}

		#endregion
	}
}