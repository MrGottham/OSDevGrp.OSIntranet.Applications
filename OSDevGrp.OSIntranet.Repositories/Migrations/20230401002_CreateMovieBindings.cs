using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401002_CreateMovieBindings")]
    internal class CreateMovieBindings : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("MovieBindings",
	            table => new
	            {
		            MovieBindingIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            MovieIdentifier = table.Column<int>(),
		            MediaPersonalityIdentifier = table.Column<int>(),
		            Role = table.Column<short>("SMALLINT"),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_MovieBindings", m => m.MovieBindingIdentifier);
		            table.UniqueConstraint("IX_MovieBindings_MovieIdentifier_MediaPersonalityIdentifier_Role", m => new { m.MovieIdentifier, m.MediaPersonalityIdentifier, m.Role });
		            table.UniqueConstraint("IX_MovieBindings_MovieIdentifier_BindingIdentifier", m => new { m.MovieIdentifier, m.MovieBindingIdentifier });
		            table.UniqueConstraint("IX_MovieBindings_MediaPersonalityIdentifier_BindingIdentifier", m => new { m.MediaPersonalityIdentifier, m.MovieBindingIdentifier });
		            table.ForeignKey("FK_Movies_MovieBindingMovieIdentifier", m => m.MovieIdentifier, "Movies", "MovieIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
					table.ForeignKey("FK_MediaPersonalities_MovieMediaPersonalityIdentifier", m => m.MediaPersonalityIdentifier, "MediaPersonalities", "MediaPersonalityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE MovieBindings MODIFY MovieBindingIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("MovieBindings");
        }

        #endregion
    }
}