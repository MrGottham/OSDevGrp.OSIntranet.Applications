using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20190414000_AddAuditFields")]
    internal class AddAuditFieldsToAccountingContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.AddColumn<DateTime>("CreatedUtcDateTime", "AccountGroups", "DATETIME", nullable: true);
            migrationBuilder.AddColumn<string>("CreatedByIdentifier", "AccountGroups", "NVARCHAR(256)", true, 256, nullable: true);
            migrationBuilder.AddColumn<DateTime>("ModifiedUtcDateTime", "AccountGroups", "DATETIME", nullable: true);
            migrationBuilder.AddColumn<string>("ModifiedByIdentifier", "AccountGroups", "NVARCHAR(256)", true, 256, nullable: true);

            migrationBuilder.AddColumn<DateTime>("CreatedUtcDateTime", "BudgetAccountGroups", "DATETIME", nullable: true);
            migrationBuilder.AddColumn<string>("CreatedByIdentifier", "BudgetAccountGroups", "NVARCHAR(256)", true, 256, nullable: true);
            migrationBuilder.AddColumn<DateTime>("ModifiedUtcDateTime", "BudgetAccountGroups", "DATETIME", nullable: true);
            migrationBuilder.AddColumn<string>("ModifiedByIdentifier", "BudgetAccountGroups", "NVARCHAR(256)", true, 256, nullable: true);

            string utcNow = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            migrationBuilder.Sql($"UPDATE AccountGroups SET CreatedUtcDateTime='{utcNow}',CreatedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations',ModifiedUtcDateTime='{utcNow}',ModifiedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations'");
            migrationBuilder.Sql($"UPDATE BudgetAccountGroups SET CreatedUtcDateTime='{utcNow}',CreatedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations',ModifiedUtcDateTime='{utcNow}',ModifiedByIdentifier='OSDevGrp.OSIntranet.Repositories.Migrations'");

            migrationBuilder.AlterColumn<DateTime>("CreatedUtcDateTime", "AccountGroups", nullable: false);
            migrationBuilder.AlterColumn<string>("CreatedByIdentifier", "AccountGroups", "NVARCHAR(256)", true, 256, nullable: false);
            migrationBuilder.AlterColumn<DateTime>("ModifiedUtcDateTime", "AccountGroups", nullable: false);
            migrationBuilder.AlterColumn<string>("ModifiedByIdentifier", "AccountGroups", "NVARCHAR(256)", true, 256, nullable: false);

            migrationBuilder.AlterColumn<DateTime>("CreatedUtcDateTime", "BudgetAccountGroups", nullable: false);
            migrationBuilder.AlterColumn<string>("CreatedByIdentifier", "BudgetAccountGroups", "NVARCHAR(256)", true, 256, nullable: false);
            migrationBuilder.AlterColumn<DateTime>("ModifiedUtcDateTime", "BudgetAccountGroups", nullable: false);
            migrationBuilder.AlterColumn<string>("ModifiedByIdentifier", "BudgetAccountGroups", "NVARCHAR(256)", true, 256, nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropColumn("CreatedUtcDateTime", "AccountGroups");
            migrationBuilder.DropColumn("CreatedByIdentifier", "AccountGroups");
            migrationBuilder.DropColumn("ModifiedUtcDateTime", "AccountGroups");
            migrationBuilder.DropColumn("ModifiedByIdentifier", "AccountGroups");

            migrationBuilder.DropColumn("CreatedUtcDateTime", "BudgetAccountGroups");
            migrationBuilder.DropColumn("CreatedByIdentifier", "BudgetAccountGroups");
            migrationBuilder.DropColumn("ModifiedUtcDateTime", "BudgetAccountGroups");
            migrationBuilder.DropColumn("ModifiedByIdentifier", "BudgetAccountGroups");
        }
    }
}