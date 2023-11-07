using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401001_CreateMovies")]
    internal class CreateMovies : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Movies",
	            table => new
	            {
		            MovieIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalMediaIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            MovieGenreIdentifier = table.Column<int>(),
		            SpokenLanguageIdentifier = table.Column<int?>(nullable: true),
		            Length = table.Column<short?>("SMALLINT", nullable: true),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_Movies", m => m.MovieIdentifier);
		            table.UniqueConstraint("IX_Movies_ExternalMediaIdentifier", m => m.ExternalMediaIdentifier);
		            table.UniqueConstraint("IX_Movies_MovieGenreIdentifier_MovieIdentifier", m => new { m.MovieGenreIdentifier, m.MovieIdentifier });
		            table.UniqueConstraint("IX_Movies_SpokenLanguageIdentifier_MovieIdentifier", m => new { m.SpokenLanguageIdentifier, m.MovieIdentifier });
		            table.ForeignKey("FK_MovieGenres_MovieGenreIdentifier", m => m.MovieGenreIdentifier, "MovieGenres", "MovieGenreIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
					table.ForeignKey("FK_Languages_MovieSpokenLanguageIdentifier", m => m.SpokenLanguageIdentifier, "Languages", "LanguageIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE Movies MODIFY MovieIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Movies");
        }

        #endregion
    }
}