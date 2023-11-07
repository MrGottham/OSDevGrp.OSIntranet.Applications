using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230507001_CreateLendings")]
    internal class CreateLendings : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Lendings",
	            table => new
	            {
		            LendingIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalLendingIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            BorrowerIdentifier = table.Column<int>(nullable: false),
		            MovieIdentifier = table.Column<int?>(nullable: true),
		            MusicIdentifier = table.Column<int?>(nullable: true),
		            BookIdentifier = table.Column<int?>(nullable: true),
		            LendingDate = table.Column<DateTime>("DATETIME", nullable: false),
		            RecallDate = table.Column<DateTime>("DATETIME", nullable: false),
		            ReturnedDate = table.Column<DateTime?>("DATETIME", nullable: true),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_Lendings", m => m.LendingIdentifier);
		            table.UniqueConstraint("IX_Lendings_ExternalLendingIdentifier", m => m.ExternalLendingIdentifier);
		            table.UniqueConstraint("IX_Lendings_BorrowerIdentifier_LendingIdentifier", m => new { m.BorrowerIdentifier, m.LendingIdentifier });
		            table.UniqueConstraint("IX_Lendings_MovieIdentifier_LendingIdentifier", m => new { m.MovieIdentifier, m.LendingIdentifier });
		            table.UniqueConstraint("IX_Lendings_MusicIdentifier_LendingIdentifier", m => new { m.MusicIdentifier, m.LendingIdentifier });
		            table.UniqueConstraint("IX_Lendings_BookIdentifier_LendingIdentifier", m => new { m.BookIdentifier, m.LendingIdentifier });
		            table.UniqueConstraint("IX_Lendings_LendingDate_LendingIdentifier", m => new { m.LendingDate, m.LendingIdentifier });
		            table.UniqueConstraint("IX_Lendings_RecallDate_LendingIdentifier", m => new { m.RecallDate, m.LendingIdentifier });
		            table.ForeignKey("FK_Borrowers_BorrowerIdentifier", m => m.BorrowerIdentifier, "Borrowers", "BorrowerIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Movies_LendingMovieIdentifier", m => m.MovieIdentifier, "Movies", "MovieIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Music_LendingMusicIdentifier", m => m.MusicIdentifier, "Music", "MusicIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
		            table.ForeignKey("FK_Books_LendingBookIdentifier", m => m.BookIdentifier, "Books", "BookIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE Lendings MODIFY LendingIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Lendings");
        }

        #endregion
    }
}