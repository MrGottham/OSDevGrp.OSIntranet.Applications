using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(ContactContext))]
    [Migration("20190626000_CreateCountries")]
    internal class CreateCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Countries",
                table => new
                {
                    Code = table.Column<string>("NVARCHAR(4)", unicode: true, nullable: false, maxLength: 4),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    UniversalName = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    PhonePrefix = table.Column<string>("NVARCHAR(4)", unicode: true, nullable: false, maxLength: 4),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", m => m.Code);
                });

            DateTime createdUtcDateTime = DateTime.UtcNow;
            const string createdByIdentifier = "OSDevGrp.OSIntranet.Repositories.Migrations";
            foreach (ICountry country in GetCountries())
            {
                migrationBuilder.InsertData("Countries", new[] {"Code", "Name", "UniversalName", "PhonePrefix", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier"}, new object[] {country.Code, country.Name, country.UniversalName, country.PhonePrefix, createdUtcDateTime, createdByIdentifier, createdUtcDateTime, createdByIdentifier});
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Countries");
        }

        private static IEnumerable<ICountry> GetCountries()
        {
            return new List<ICountry>
            {
                // ReSharper disable StringLiteralTypo
                new Country("DK", "Danmark", "Denmark", "+45"),
                new Country("FO", "Færøerne", "Faroe Islands", "+298"),
                new Country("GL", "Grønland", "Greenland", "+299"),
                new Country("IS", "Island", "Iceland", "+354"),
                new Country("NO", "Norge", "Norway", "+47"),
                new Country("SE", "Sverige", "Sweden", "+46"),
                new Country("DE", "Tyskland", "Germany", "+49"),
                new Country("US", "USA", "United States", "+1")
                // ReSharper restore StringLiteralTypo
            };
        }
    }
}
