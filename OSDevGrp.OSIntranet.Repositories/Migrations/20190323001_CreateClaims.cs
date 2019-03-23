using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(SecurityContext))]
    [Migration("20190323001_CreateClaims")]
    internal class CreateClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Claims",
                table => new
                {
                    ClaimIdentifier = table.Column<int>().Annotation("MySQL:AutoIncrement", true),
                    ClaimType = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ClaimValue = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: true, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", m => m.ClaimIdentifier);
                    table.UniqueConstraint("IX_Claims_ClaimType", m => m.ClaimType);
                });

            InsertClaimModels(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Claims");
        }

        private void InsertClaimModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            IEnumerable<Claim> claimCollection = new List<Claim>
            {
                ClaimHelper.CreateSecurityAdminClaim(),
                ClaimHelper.CreateAccountingClaim()
            };
            foreach (Claim claim in claimCollection)
            {
                migrationBuilder.InsertData("Claims", new[] {"ClaimType", "ClaimValue"}, new object[] {claim.Type, claim.Value});
            }
        }
    }
}
