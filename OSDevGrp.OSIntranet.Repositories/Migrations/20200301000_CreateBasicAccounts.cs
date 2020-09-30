using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200301000_CreateBasicAccounts")]
    internal class CreateBasicAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("BasicAccounts",
                table => new
                {
                    BasicAccountIdentifier = table.Column<int>().Annotation("MySql:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    Description = table.Column<string>("NVARCHAR(512)", unicode: true, nullable: true, maxLength: 512),
                    Note = table.Column<string>("NVARCHAR(4096)", unicode: true, nullable: true, maxLength: 4096),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table => table.PrimaryKey("PK_BasicAccounts", m => m.BasicAccountIdentifier));

            migrationBuilder.Sql("ALTER TABLE BasicAccounts MODIFY BasicAccountIdentifier INT NOT NULL AUTO_INCREMENT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("BasicAccounts");
        }
    }
}