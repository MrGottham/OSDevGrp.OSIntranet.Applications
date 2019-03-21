using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Migrations
{
    [DbContext(typeof(AccountingContext))]
    [Migration("20190319000_InitialCreate")]
    internal class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.CreateTable("AccountGroups",
                table => new
                {
                    AccountGroupIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256),
                    AccountGroupType = table.Column<AccountGroupType>("TINYINT")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGroups", m => m.AccountGroupIdentifier);
                });

            migrationBuilder.CreateTable("BudgetAccountGroups",
                table => new
                {
                    BudgetAccountGroupIdentifier = table.Column<int>(),
                    Name = table.Column<string>("NVARCHAR(256)", unicode: true, nullable: false, maxLength: 256)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetAccountGroups", m => m.BudgetAccountGroupIdentifier);
                });

            InsertAccountGroupModels(migrationBuilder);
            InsertBudgetAccountGroupModels(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            migrationBuilder.DropTable("AccountGroups");
            migrationBuilder.DropTable("BudgetAccountGroups");
        }

        private static void InsertAccountGroupModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            IEnumerable<AccountGroupModel> accountGroupModelCollection = new List<AccountGroupModel>
            {
                // ReSharper disable StringLiteralTypo
                new AccountGroupModel {AccountGroupIdentifier = 1, Name = "Bankkonti", AccountGroupType = AccountGroupType.Assets},
                new AccountGroupModel {AccountGroupIdentifier = 2, Name = "Kontanter", AccountGroupType = AccountGroupType.Assets},
                new AccountGroupModel {AccountGroupIdentifier = 11, Name = "Anlægsaktiver", AccountGroupType = AccountGroupType.Assets},
                new AccountGroupModel {AccountGroupIdentifier = 12, Name = "Omsætningsaktiver", AccountGroupType = AccountGroupType.Assets},
                new AccountGroupModel {AccountGroupIdentifier = 13, Name = "Egenkapital", AccountGroupType = AccountGroupType.Liabilities},
                new AccountGroupModel {AccountGroupIdentifier = 15, Name = "Gæld", AccountGroupType = AccountGroupType.Liabilities},
                new AccountGroupModel {AccountGroupIdentifier = 21, Name = "Aktiver", AccountGroupType = AccountGroupType.Assets},
                new AccountGroupModel {AccountGroupIdentifier = 22, Name = "Passiver", AccountGroupType = AccountGroupType.Liabilities}
                // ReSharper restore StringLiteralTypo
            };
            foreach (AccountGroupModel accountGroupModel in accountGroupModelCollection)
            {
                migrationBuilder.InsertData("AccountGroups", new[] { "AccountGroupIdentifier", "Name", "AccountGroupType" }, new object[] {accountGroupModel.AccountGroupIdentifier, accountGroupModel.Name, (int) accountGroupModel.AccountGroupType});
            }
        }

        private static void InsertBudgetAccountGroupModels(MigrationBuilder migrationBuilder)
        {
            NullGuard.NotNull(migrationBuilder, nameof(migrationBuilder));

            IEnumerable<BudgetAccountGroupModel> budgetAccountGroupModelCollection = new List<BudgetAccountGroupModel>
            {
                // ReSharper disable StringLiteralTypo
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 1, Name = "Indtægter"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 2, Name = "Faste udgifter"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 3, Name = "Dagligvareforretninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 4, Name = "Specialvareforretninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 5, Name = "Restaurationer"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 6, Name = "Transportomkostninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 7, Name = "Forlystelser"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 8, Name = "Øvrige udgifter"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 11, Name = "Nettoomsætning"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 12, Name = "Varekøb"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 13, Name = "Eksterne omkostninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 15, Name = "Afskrivninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 16, Name = "Renteindtægter"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 17, Name = "Renteomkostninger"},
                new BudgetAccountGroupModel {BudgetAccountGroupIdentifier = 21, Name = "Udgifter"},
                // ReSharper restore StringLiteralTypo
            };
            foreach (BudgetAccountGroupModel budgetAccountGroupModel in budgetAccountGroupModelCollection)
            {
                migrationBuilder.InsertData("BudgetAccountGroups", new[] {"BudgetAccountGroupIdentifier", "Name"}, new object[] {budgetAccountGroupModel.BudgetAccountGroupIdentifier, budgetAccountGroupModel.Name});
            }
        }
    }
}
