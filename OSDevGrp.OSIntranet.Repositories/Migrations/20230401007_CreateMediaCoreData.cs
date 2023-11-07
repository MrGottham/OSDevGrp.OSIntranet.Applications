using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401007_CreateMediaCoreData")]
    internal class CreateMediaCoreData : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("MediaCoreData",
	            table => new
	            {
		            MediaCoreDataIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            Title = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            Subtitle = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
		            Description = table.Column<string>("NVARCHAR(512)", unicode: true, nullable: true, maxLength: 512),
		            Details = table.Column<string>("TEXT", unicode: true, nullable: true),
		            MediaTypeIdentifier = table.Column<int>(),
		            Published = table.Column<short?>("SMALLINT", nullable: true),
		            Url = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
		            Image = table.Column<string>("TEXT", unicode: true, nullable: true),
					MovieIdentifier = table.Column<int?>(nullable: true),
					MusicIdentifier = table.Column<int?>(nullable: true),
					BookIdentifier = table.Column<int?>(nullable: true),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_MediaCoreData", m => m.MediaCoreDataIdentifier);
		            table.UniqueConstraint("IX_MediaCoreData_Title", m => new { m.Title, m.Subtitle });
		            table.UniqueConstraint("IX_MediaCoreData_MediaTypeIdentifier_MediaCoreDataIdentifier", m => new { m.MediaTypeIdentifier, m.MediaCoreDataIdentifier });
		            table.UniqueConstraint("IX_MediaCoreData_MovieIdentifier_MediaCoreDataIdentifier", m => new { m.MovieIdentifier, m.MediaCoreDataIdentifier });
		            table.UniqueConstraint("IX_MediaCoreData_MusicIdentifier_MediaCoreDataIdentifier", m => new { m.MusicIdentifier, m.MediaCoreDataIdentifier });
		            table.UniqueConstraint("IX_MediaCoreData_BookIdentifier_MediaCoreDataIdentifier", m => new { m.BookIdentifier, m.MediaCoreDataIdentifier });
		            table.ForeignKey("FK_MediaTypes_MediaTypeIdentifier", m => m.MediaTypeIdentifier, "MediaTypes", "MediaTypeIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Movies_MediaCoreDataMovieIdentifier", m => m.MovieIdentifier, "Movies", "MovieIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Music_MediaCoreDataMusicIdentifier", m => m.MusicIdentifier, "Music", "MusicIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Books_MediaCoreDataBookIdentifier", m => m.BookIdentifier, "Books", "BookIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE MediaCoreData MODIFY MediaCoreDataIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("MediaCoreData");
        }

        #endregion
    }
}