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
    [Migration("20230117001_CreateLanguages")]
    internal class CreateLanguages : Migration
    {
        #region Methods

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("Languages",
                table => new
                {
                    LanguageIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    CreatedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    CreatedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    ModifiedUtcDateTime = table.Column<DateTime>("DATETIME", nullable: false),
                    ModifiedByIdentifier = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", m => m.LanguageIdentifier);
                });

            DateTime createdUtcDateTime = DateTime.UtcNow;
            foreach (ILanguage language in CreateLanguagesCollection())
            {
                migrationBuilder.InsertData("Languages", new[] { "LanguageIdentifier", "Name", "CreatedUtcDateTime", "CreatedByIdentifier", "ModifiedUtcDateTime", "ModifiedByIdentifier" }, new[] { "INT(11)", "NVARCHAR(256)", "DATETIME", "NVARCHAR(256)", "DATETIME", "NVARCHAR(256)" }, new object[] { language.Number, language.Name, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier, createdUtcDateTime, MigrationHelper.MigrationUserIdentifier });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("Languages");
        }

        private static IEnumerable<ILanguage> CreateLanguagesCollection()
        {
            return new ILanguage[]
            {
                new Language(1, "Dansk"),
                new Language(2, "Norsk"),
                new Language(3, "Svensk"),
                new Language(4, "Engelsk"),
                new Language(5, "Tysk"),
                new Language(6, "Spansk"),
                new Language(7, "Fransk"),
                new Language(8, "Italiensk")
            };
        }

        #endregion
    }
}