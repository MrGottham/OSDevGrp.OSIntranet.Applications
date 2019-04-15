using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(SecurityContext))]
    [Migration("20190415000_AddAuditFields")]
    internal class AddAuditFieldsToSecurityContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            string utcNow = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            foreach (string tableName in new[] {"UserIdentities", "UserIdentityClaims", "ClientSecretIdentities", "ClientSecretIdentityClaims", "Claims"})
            {
                migrationBuilder.AddColumn<DateTime>("CreatedUtcDateTime", tableName, "DATETIME", nullable: true);
                migrationBuilder.AddColumn<string>("CreatedByIdentifier", tableName, "NVARCHAR(256)", true, 256, nullable: true);
                migrationBuilder.AddColumn<DateTime>("ModifiedUtcDateTime", tableName, "DATETIME", nullable: true);
                migrationBuilder.AddColumn<string>("ModifiedByIdentifier", tableName, "NVARCHAR(256)", true, 256, nullable: true);
                
                migrationBuilder.Sql($"UPDATE {tableName} SET CreatedUtcDateTime='{utcNow}',CreatedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations',ModifiedUtcDateTime='{utcNow}',ModifiedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations'");
                
                migrationBuilder.AlterColumn<DateTime>("CreatedUtcDateTime", tableName, nullable: false);
                migrationBuilder.AlterColumn<string>("CreatedByIdentifier", tableName, "NVARCHAR(256)", true, 256, nullable: false);
                migrationBuilder.AlterColumn<DateTime>("ModifiedUtcDateTime", tableName, nullable: false);
                migrationBuilder.AlterColumn<string>("ModifiedByIdentifier", tableName, "NVARCHAR(256)", true, 256, nullable: false);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            foreach (string tableName in new[] {"UserIdentities", "UserIdentityClaims", "ClientSecretIdentities", "ClientSecretIdentityClaims", "Claims"})
            {
                migrationBuilder.DropColumn("CreatedUtcDateTime", tableName);
                migrationBuilder.DropColumn("CreatedByIdentifier", tableName);
                migrationBuilder.DropColumn("ModifiedUtcDateTime", tableName);
                migrationBuilder.DropColumn("ModifiedByIdentifier", tableName);
            }
        }
    }
}