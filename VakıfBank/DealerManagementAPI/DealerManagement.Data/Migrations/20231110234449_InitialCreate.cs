using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DealerManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "TRY"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 693, DateTimeKind.Utc).AddTicks(5817))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 694, DateTimeKind.Utc).AddTicks(5433))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "500"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    KDV = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 22m),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 696, DateTimeKind.Utc).AddTicks(6553))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardHolder = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ExpireDate = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    ExpenseLimit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 693, DateTimeKind.Utc).AddTicks(6978))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Card_Account",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckingAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 693, DateTimeKind.Utc).AddTicks(3816))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckingAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckingAccount_Account",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    OpeningAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DebtTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    Interest = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 693, DateTimeKind.Utc).AddTicks(1158))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositAccount_Account",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 693, DateTimeKind.Utc).AddTicks(9057))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 694, DateTimeKind.Utc).AddTicks(1317))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyCard_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "dealer"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 696, DateTimeKind.Utc).AddTicks(8206))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SupplierIBAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPriceSum = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    KDVPriceSum = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    BillingCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Cart"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 695, DateTimeKind.Utc).AddTicks(6676))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 694, DateTimeKind.Utc).AddTicks(3530))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 694, DateTimeKind.Utc).AddTicks(6321))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyStock_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyStock_Product",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 695, DateTimeKind.Utc).AddTicks(2782))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_Sender",
                        column: x => x.SenderId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 695, DateTimeKind.Utc).AddTicks(9896))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Product",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Confirmation Awaited"),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 696, DateTimeKind.Utc).AddTicks(2811))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_CompanyCard",
                        column: x => x.CardId,
                        principalTable: "CompanyCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Order",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dealers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProfitMargin = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    ContractDeadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 694, DateTimeKind.Utc).AddTicks(9326))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealer_Company",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dealer_Supplier",
                        column: x => x.SupplierId,
                        principalTable: "Supliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MessageDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 695, DateTimeKind.Utc).AddTicks(4896)),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2023, 11, 10, 23, 44, 49, 695, DateTimeKind.Utc).AddTicks(4523))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageContent_Message",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountNumber", "CurrencyCode", "IBAN" },
                values: new object[,]
                {
                    { 1, "9012345678901234", "TRY", "TR123456789012345678901234" },
                    { 2, "1012345678901234", "TRY", "TR123456781012345678901234" },
                    { 3, "2012345678901234", "TRY", "TR123456782012345678901234" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CompanyName", "Mail" },
                values: new object[,]
                {
                    { 1, "EGE AKÜ", "salihekin@gmail.com" },
                    { 2, "EGE AKÜ BAYİ 1", "mervetopuz@gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "KDV", "ProductCode", "ProductName", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus veniam corporis magnam aliquid officia explicabo expedita. Veritatis qui ipsum odio nihil facilis velit suscipit? A iusto incidunt nostrum nam alias.", 22m, "10001", "Ürün 1", 10m },
                    { 2, "Lorem ipsum dolor sit amet consectetur adipisicing elit. Accusamus veniam corporis magnam aliquid officia explicabo expedita. Veritatis qui ipsum odio nihil facilis velit suscipit? A iusto incidunt nostrum nam alias.", 22m, "10002", "Ürün 2", 20m }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "AddressLine1", "AddressLine2", "City", "CompanyId", "Country", "PostalCode", "Province" },
                values: new object[,]
                {
                    { 1, "İvedik Sanayi", null, "Yenimahalle", 1, "Turkey", "06370", "Ankara" },
                    { 2, "Ostim", null, "Yenimahalle", 2, "Turkey", "06370", "Ankara" }
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "AccountId", "CVV", "CardHolder", "CardNumber", "ExpenseLimit", "ExpireDate" },
                values: new object[,]
                {
                    { 1, 2, "123", "Merve Topuz", "1234567890123456", 10000m, "12/28" },
                    { 2, 3, "321", "Yiğit Aslan", "1234567890123457", 10000m, "12/28" }
                });

            migrationBuilder.InsertData(
                table: "CheckingAccounts",
                columns: new[] { "Id", "AccountId", "Balance" },
                values: new object[,]
                {
                    { 1, 1, 0m },
                    { 2, 2, 1000m }
                });

            migrationBuilder.InsertData(
                table: "CompanyCards",
                columns: new[] { "Id", "CardName", "CardNumber", "CardType", "CompanyId" },
                values: new object[,]
                {
                    { 1, "Card 1", "1234567890123456", "credit", 2 },
                    { 2, "Card 2", "1234567890123457", "deposit", 2 }
                });

            migrationBuilder.InsertData(
                table: "CompanyStocks",
                columns: new[] { "Id", "CompanyId", "ProductId", "Stock" },
                values: new object[,]
                {
                    { 1, 1, 1, 50 },
                    { 2, 1, 2, 30 },
                    { 3, 2, 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "DepositAccounts",
                columns: new[] { "Id", "AccountId", "Interest", "OpeningAmount" },
                values: new object[] { 1, 3, 12m, 1000m });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "EmployeeNumber", "Mail", "Name", "Password", "Role", "Surname" },
                values: new object[,]
                {
                    { 1, 1, "10001", "salihekin@gmail.com", "Salih", "5d41402abc4b2a76b9719d911017c592", "admin", "Ekin" },
                    { 2, 2, "10002", "mervetopuz@gmail.com", "Merve", "5d41402abc4b2a76b9719d911017c592", "dealer", "Topuz" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "BillingCode", "CompanyId", "SupplierIBAN" },
                values: new object[] { 1, null, 2, "TR123456789012345678901234" });

            migrationBuilder.InsertData(
                table: "Supliers",
                columns: new[] { "Id", "CompanyId", "IBAN" },
                values: new object[] { 1, 1, "TR123456789012345678901234" });

            migrationBuilder.InsertData(
                table: "Dealers",
                columns: new[] { "Id", "CompanyId", "ContractDeadline", "ProfitMargin", "SupplierId" },
                values: new object[] { 1, 2, new DateTime(2025, 11, 11, 2, 44, 49, 697, DateTimeKind.Local).AddTicks(350), 10.2m, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CompanyId",
                table: "Addresses",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckingAccounts_AccountId",
                table: "CheckingAccounts",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCards_CompanyId",
                table: "CompanyCards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStocks_CompanyId",
                table: "CompanyStocks",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStocks_ProductId",
                table: "CompanyStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_CompanyId",
                table: "Dealers",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_SupplierId",
                table: "Dealers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositAccounts_AccountId",
                table: "DepositAccounts",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageContents_MessageId",
                table: "MessageContents",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompanyId",
                table: "Orders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AccountId",
                table: "Payments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardId",
                table: "Payments",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Supliers_CompanyId",
                table: "Supliers",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "CheckingAccounts");

            migrationBuilder.DropTable(
                name: "CompanyStocks");

            migrationBuilder.DropTable(
                name: "Dealers");

            migrationBuilder.DropTable(
                name: "DepositAccounts");

            migrationBuilder.DropTable(
                name: "MessageContents");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Supliers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "CompanyCards");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
