using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20230117000_CreateNationalities")]
    internal class CreateNationalities : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Nationalities",
                table => new
                {
                    NationalityIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", m => m.NationalityIdentifier);
                });

            DateTime createdUtcDateTime = DateTime.UtcNow;
            foreach (INationality nationality in CreateNationalitiesCollection())
            {
                migrationBuilder.InsertData("Nationalities", new[] { "NationalityIdentifier", "Name", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier" }, new[] { "INT(11)", "NVARCHAR(256)", "DATETIME", "NVARCHAR(256)", "DATETIME", "NVARCHAR(256)" }, new object[] { nationality.Number, nationality.Name, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Nationalities");
        }

        private static IEnumerable<INationality> CreateNationalitiesCollection()
        {
            return new INationality[]
            {
                new Nationality(1, "Dansk"),
                new Nationality(2, "Norsk"),
                new Nationality(3, "Svensk"),
                new Nationality(4, "Engelsk"),
                new Nationality(5, "Amerikansk"),
                new Nationality(6, "Tysk"),
                new Nationality(7, "Spansk"),
                new Nationality(8, "Fransk"),
                new Nationality(9, "Italiensk")
            };
        }

        #endregion
    }
}