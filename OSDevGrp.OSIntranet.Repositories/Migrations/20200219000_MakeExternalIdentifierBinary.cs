using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20200219000_MakeExternalIdentifierBinary")]
    internal class MakeExternalIdentifierBinary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.AlterColumn<string>("ExternalIdentifier", "ContactSupplementBindings", "VARBINARY(256)", unicode: true, nullable: false, maxLength: 256);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.AlterColumn<string>("ExternalIdentifier", "ContactSupplementBindings", "NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256);
        }
    }
}
