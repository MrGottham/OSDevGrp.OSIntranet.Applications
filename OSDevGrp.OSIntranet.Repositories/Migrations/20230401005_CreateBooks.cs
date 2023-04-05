using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401005_CreateBooks")]
    internal class CreateBooks : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Books",
	            table => new
	            {
		            BookIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalMediaIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            BookGenreIdentifier = table.Column<int>(),
		            WrittenLanguageIdentifier = table.Column<int?>(nullable: true),
		            InternationalStandardBookNumber = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_Books", m => m.BookIdentifier);
		            table.UniqueConstraint("IX_Books_ExternalMediaIdentifier", m => m.ExternalMediaIdentifier);
		            table.UniqueConstraint("IX_Books_BookGenreIdentifier_BookIdentifier", m => new { m.BookGenreIdentifier, m.BookIdentifier });
		            table.UniqueConstraint("IX_Books_WrittenLanguageIdentifier_BookIdentifier", m => new { m.WrittenLanguageIdentifier, m.BookIdentifier });
		            table.ForeignKey("FK_BookGenres_BookGenreIdentifier", m => m.BookGenreIdentifier, "BookGenres", "BookGenreIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
					table.ForeignKey("FK_Languages_BookWrittenLanguageIdentifier", m => m.WrittenLanguageIdentifier, "Languages", "LanguageIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE Books MODIFY BookIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Books");
        }

        #endregion
    }
}