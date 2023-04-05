using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401003_CreateMusic")]
    internal class CreateMusic : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Music",
	            table => new
	            {
		            MusicIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalMediaIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            MusicGenreIdentifier = table.Column<int>(),
		            Tracks = table.Column<short?>("SMALLINT", nullable: true),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_Music", m => m.MusicIdentifier);
		            table.UniqueConstraint("IX_Music_ExternalMediaIdentifier", m => m.ExternalMediaIdentifier);
		            table.UniqueConstraint("IX_Music_MusicGenreIdentifier_MusicIdentifier", m => new { m.MusicGenreIdentifier, m.MusicIdentifier });
		            table.ForeignKey("FK_MusicGenres_MusicGenreIdentifier", m => m.MusicGenreIdentifier, "MusicGenres", "MusicGenreIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE Music MODIFY MusicIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Music");
        }

        #endregion
    }
}