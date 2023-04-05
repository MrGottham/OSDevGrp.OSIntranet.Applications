using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401006_CreateBookBindings")]
    internal class CreateBookBindings : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("BookBindings",
	            table => new
	            {
		            BookBindingIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            BookIdentifier = table.Column<int>(),
		            MediaPersonalityIdentifier = table.Column<int>(),
		            Role = table.Column<short>("SMALLINT"),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_BookBindings", m => m.BookBindingIdentifier);
		            table.UniqueConstraint("IX_BookBindings_BookIdentifier_MediaPersonalityIdentifier_Role", m => new { m.BookIdentifier, m.MediaPersonalityIdentifier, m.Role });
		            table.UniqueConstraint("IX_BookBindings_BookIdentifier_BindingIdentifier", m => new { m.BookIdentifier, m.BookBindingIdentifier });
		            table.UniqueConstraint("IX_BookBindings_MediaPersonalityIdentifier_BindingIdentifier", m => new { m.MediaPersonalityIdentifier, m.BookBindingIdentifier });
		            table.ForeignKey("FK_Books_BookBindingBookIdentifier", m => m.BookIdentifier, "Books", "BookIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
					table.ForeignKey("FK_MediaPersonalities_BookMediaPersonalityIdentifier", m => m.MediaPersonalityIdentifier, "MediaPersonalities", "MediaPersonalityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE BookBindings MODIFY BookBindingIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("BookBindings");
        }

        #endregion
    }
}