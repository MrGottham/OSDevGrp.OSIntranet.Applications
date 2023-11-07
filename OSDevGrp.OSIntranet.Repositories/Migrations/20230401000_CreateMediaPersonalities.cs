using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230401000_CreateMediaPersonalities")]
    internal class CreateMediaPersonalities : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("MediaPersonalities",
	            table => new
	            {
		            MediaPersonalityIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalMediaPersonalityIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            GivenName = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
		            MiddleName = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
		            Surname = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: false, maxLength: 32),
		            NationalityIdentifier = table.Column<int>(),
		            BirthDate = table.Column<DateTime>("DATETIME", nullable: true),
		            DateOfDead = table.Column<DateTime>("DATETIME", nullable: true),
		            Url = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
		            Image = table.Column<string>("TEXT", unicode: true, nullable: true),
		            CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_MediaPersonalities", m => m.MediaPersonalityIdentifier);
		            table.UniqueConstraint("IX_MediaPersonalities_ExternalMediaPersonalityIdentifier", m => m.ExternalMediaPersonalityIdentifier);
		            table.UniqueConstraint("IX_MediaPersonalities_FullName", m => new { m.GivenName, m.MiddleName, m.Surname });
		            table.UniqueConstraint("IX_MediaPersonalities_NationalityIdentifier_Identifier", m => new { m.NationalityIdentifier, m.MediaPersonalityIdentifier });
		            table.ForeignKey("FK_Nationalities_MediaPersonalityNationalityIdentifier", m => m.NationalityIdentifier, "Nationalities", "NationalityIdentifier", onUpdate: ReferentialAction.Cascade, onDelete: ReferentialAction.Cascade);
	            });

            migrationBuilder.Sql("ALTER TABLE MediaPersonalities MODIFY MediaPersonalityIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("MediaPersonalities");
        }

        #endregion
    }
}