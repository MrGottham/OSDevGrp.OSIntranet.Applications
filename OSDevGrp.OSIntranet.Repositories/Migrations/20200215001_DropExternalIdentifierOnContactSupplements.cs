using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20200215001_DropExternalIdentifierOnContactSupplements")]
    internal class DropExternalIdentifierOnContactSupplements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropIndex("IX_ContactSupplements_ExternalIdentifier", "ContactSupplements");
            migrationBuilder.DropColumn("ExternalIdentifier", "ContactSupplements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException("Cannot recreate the column named 'ExternalIdentifier' on the table named 'ContactSupplements'.");
        }
    }
}