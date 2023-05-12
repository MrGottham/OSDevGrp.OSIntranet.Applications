using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
	[DbContext(typeof(RepositoryContext))]
    [Migration("20230507000_CreateBorrowers")]
    internal class CreateBorrowers : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Borrowers",
	            table => new
	            {
		            BorrowerIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
		            ExternalBorrowerIdentifier = table.Column<string>("CHAR(36)", nullable: false),
		            ExternalIdentifier = table.Column<string>("VARBINARY(256)", unicode: true, nullable: true, maxLength: 256),
		            FullName = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            MailAddress = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256),
		            PrimaryPhone = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
					SecondaryPhone = table.Column<string>("NVARCHAR(32)", unicode: true, nullable: true, maxLength: 32),
					LendingLimit = table.Column<int>(nullable: false),
					CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
		            ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
		            ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
	            },
	            constraints: table =>
	            {
		            table.PrimaryKey("PK_Borrowers", m => m.BorrowerIdentifier);
		            table.UniqueConstraint("IX_Borrowers_ExternalBorrowerIdentifier", m => m.ExternalBorrowerIdentifier);
		            table.UniqueConstraint("IX_Borrowers_ExternalIdentifier_BorrowerIdentifier", m => new { m.ExternalIdentifier, m.BorrowerIdentifier });
		            table.UniqueConstraint("IX_Borrowers_FullName_BorrowerIdentifier", m => new { m.FullName, m.BorrowerIdentifier });
	            });

            migrationBuilder.Sql("ALTER TABLE Borrowers MODIFY BorrowerIdentifier INT NOT NULL AUTO_INCREMENT");
        }

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Borrowers");
        }

        #endregion
    }
}