using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401004_CreateMusicBindings")]
    internal class CreateMusicBindings : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("MusicBindings",
	            table => new
	            {
		            MusicBindingIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            MusicIdentifier = table.Column<int>(),
		            MediaPersonalityIdentifier = table.Column<int>(),
		            Role = table.Column<short>("SMALLINT"),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_MusicBindings", m => m.MusicBindingIdentifier);
		            table.UniqueConstraint("IX_MusicBindings_MusicIdentifier_MediaPersonalityIdentifier_Role", m => new { m.MusicIdentifier, m.MediaPersonalityIdentifier, m.Role });
		            table.UniqueConstraint("IX_MusicBindings_MusicIdentifier_BindingIdentifier", m => new { m.MusicIdentifier, m.MusicBindingIdentifier });
		            table.UniqueConstraint("IX_MusicBindings_MediaPersonalityIdentifier_BindingIdentifier", m => new { m.MediaPersonalityIdentifier, m.MusicBindingIdentifier });
		            table.ForeignKey("FK_Music_MusicBindingMusicIdentifier", m => m.MusicIdentifier, "Music", "MusicIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
					table.ForeignKey("FK_MediaPersonalities_MusicMediaPersonalityIdentifier", m => m.MediaPersonalityIdentifier, "MediaPersonalities", "MediaPersonalityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE MusicBindings MODIFY MusicBindingIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("MusicBindings");
        }

        #endregion
    }
}