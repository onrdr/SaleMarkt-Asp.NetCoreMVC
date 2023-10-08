using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    OrderHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2b76647d-c501-44c3-91c7-a2bd6843b6e7"), null, "SuperAdmin", "SUPERADMIN" },
                    { new Guid("2f514e34-8a22-4e36-aefc-752ba3aa0b34"), null, "Admin", "ADMIN" },
                    { new Guid("41102f40-1cee-4a61-9add-140d2608b1a5"), null, "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "City", "ConcurrencyStamp", "Country", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("ad4c1f6b-f620-41d7-d3d8-08dbc6b87e76"), 0, "Şişli", "Istanbul", "b124af6f-c7a3-4abc-a917-456602df151d", "Turkiye", "customer@salemarkt.com", false, false, null, "Company Customer ", "CUSTOMER@SALEMARKT.COM", "COMPANY-CUSTOMER", "AQAAAAIAAYagAAAAEEPT05PyWYpkO5AqVs4mvyalAB0VPjWhSknJ1HonAod5tewuuD5R9FtW2mO23/21XQ==", "1029384756", false, "34000", "PVNIK4FXHMWDXUJSOPGDPO4DPHTHOAM7", false, "Company-Customer" },
                    { new Guid("e6218c9e-f224-46f1-a38e-08dbc6b81e6e"), 0, "Levent", "Istanbul", "a5b4ce0d-3189-4b16-a4df-9adbde35b40d", "Turkiye", "superadmin@salemarkt.com", false, false, null, "Super Admin", "SUPERADMIN@SALEMARKT.COM", "SUPER-ADMIN", "AQAAAAIAAYagAAAAECByl9RKmTARk5w1x7F8JgWCAT8zKI9y2s/BaQVHO2pPxMAncULF+44kC7fMz4nnpA==", "1234567890", false, "34000", "LB3ETSAI7Y2TFBWIODGRBQU774ITQI7G", false, "Super-Admin" },
                    { new Guid("fd91a0bc-13e9-46d0-d3d7-08dbc6b87e76"), 0, "Beşiktaş", "Istanbul", "156d7eb0-4752-4147-817a-90696985e4d5", "Turkiye", "companyadmin@salemarkt.com", false, false, null, "Company Admin", "COMPANYADMIN@SALEMARKT.COM", "COMPANY-ADMIN", "AQAAAAIAAYagAAAAEPvC/yQD0wrfDxCYISaEgR2+RfJQcGEJzK2PeseSSyDlpOj9cVEsz9oaCIEGHGw1ag==", "0987654321", false, "34000", "BZMCGV4LGIAVLGR4NXGRVTBCBKZNAZRK", false, "Company-Admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "DisplayOrder", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("34245a4d-0baa-4c22-8245-02abb9063b11"), "Category1 Description", 1, "", "Category1" },
                    { new Guid("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"), "Category2 Description", 2, "", "Category2" },
                    { new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Category4 Description", 4, "", "Category4" },
                    { new Guid("db9b235f-a5d6-49dc-8e95-022f443f8582"), "Category3 Description", 3, "", "Category3" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "Country", "Email", "ImageUrl", "Name", "PhoneNumber", "PostalCode" },
                values: new object[] { new Guid("dc25bafb-40d3-4f4c-9a26-f2f14c34ad9c"), "Beşiktaş", "Istanbul", "Turkiye", "salemarkt@salemarkt.com", null, "SaleMarkt", "1234567890", "34000" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId", "Discriminator" },
                values: new object[,]
                {
                    { new Guid("41102f40-1cee-4a61-9add-140d2608b1a5"), new Guid("ad4c1f6b-f620-41d7-d3d8-08dbc6b87e76"), "AppUserRole" },
                    { new Guid("2b76647d-c501-44c3-91c7-a2bd6843b6e7"), new Guid("e6218c9e-f224-46f1-a38e-08dbc6b81e6e"), "AppUserRole" },
                    { new Guid("2f514e34-8a22-4e36-aefc-752ba3aa0b34"), new Guid("fd91a0bc-13e9-46d0-d3d7-08dbc6b87e76"), "AppUserRole" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Color", "Description", "ImageUrl", "ListPrice", "Price", "Price100", "Price50", "Size", "Title" },
                values: new object[,]
                {
                    { new Guid("1b8fe840-2751-4282-b41c-0beb710b1f45"), new Guid("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"), "Black", "Product 10 Description", "images\\product\\product.png", 35.0, 32.0, 28.0, 30.0, "XL", "Product 10" },
                    { new Guid("2f36eefc-89d9-49c1-9ebf-be97db6870e0"), new Guid("db9b235f-a5d6-49dc-8e95-022f443f8582"), "White", "Product 5 Description", "images\\product\\product.png", 30.0, 27.0, 20.0, 25.0, "M", "Product 5" },
                    { new Guid("327353b3-04ee-411b-a096-24980fa107ba"), new Guid("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"), "Blue", "Product 2 Description", "images\\product\\product.png", 40.0, 30.0, 20.0, 25.0, "S", "Product 2" },
                    { new Guid("40c72ab7-31ac-4814-be33-b7232e8e44e9"), new Guid("db9b235f-a5d6-49dc-8e95-022f443f8582"), "Yellow", "Product 12 Description", "images\\product\\product.png", 50.0, 45.0, 40.0, 42.0, "M", "Product 12" },
                    { new Guid("503f1008-d44f-49cf-b528-9de7b4078c54"), new Guid("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"), "Green", "Product 9 Description", "images\\product\\product.png", 70.0, 65.0, 55.0, 60.0, "L", "Product 9" },
                    { new Guid("560f45eb-1d1c-4024-b6ac-6b54a1bb68ea"), new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Red", "Product 13 Description", "images\\product\\product.png", 40.0, 38.0, 32.0, 35.0, "L", "Product 13" },
                    { new Guid("69ebc18b-c821-4ade-9790-04f8fc510146"), new Guid("db9b235f-a5d6-49dc-8e95-022f443f8582"), "Green", "Product 3 Description", "images\\product\\product.png", 55.0, 50.0, 35.0, 40.0, "L", "Product 3" },
                    { new Guid("72623fd5-7860-4a98-838f-f773773d5de9"), new Guid("db9b235f-a5d6-49dc-8e95-022f443f8582"), "White", "Product 11 Description", "images\\product\\product.png", 25.0, 23.0, 19.0, 21.0, "S", "Product 11" },
                    { new Guid("7e02241b-fc68-4bd8-b601-8f09bf049f94"), new Guid("34245a4d-0baa-4c22-8245-02abb9063b11"), "Red", "Product 7 Description", "images\\product\\product.png", 60.0, 55.0, 45.0, 50.0, "S", "Product 7" },
                    { new Guid("83240dc5-6012-425b-9c35-5176a8b55f9b"), new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Blue", "Product 8 Description", "images\\product\\product.png", 45.0, 40.0, 35.0, 38.0, "M", "Product 8" },
                    { new Guid("938d8e64-db0e-4390-b7d2-582810774755"), new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Black", "Product 4 Description", "images\\product\\product.png", 70.0, 65.0, 55.0, 60.0, "XL", "Product 4" },
                    { new Guid("9cad99c8-412d-4c88-abb5-0ccd311853b0"), new Guid("34245a4d-0baa-4c22-8245-02abb9063b11"), "Blue", "Product 14 Description", "images\\product\\product.png", 55.0, 52.0, 48.0, 50.0, "XL", "Product 14" },
                    { new Guid("aeaa8536-d241-41af-95a6-c900c3527437"), new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Green", "Product 15 Description", "images\\product\\product.png", 70.0, 68.0, 60.0, 65.0, "S", "Product 15" },
                    { new Guid("b4591371-41d8-48fd-990f-e5c4f3b89ad8"), new Guid("8179ed4d-7e5b-49c4-33f3-08dbc21909ac"), "Yellow", "Product 6 Description", "images\\product\\product.png", 25.0, 23.0, 20.0, 22.0, "L", "Product 6" },
                    { new Guid("cf8523a5-79cb-4c6d-affc-ee99942906d1"), new Guid("34245a4d-0baa-4c22-8245-02abb9063b11"), "Red", "Product 1 Description", "images\\product\\product.png", 99.0, 90.0, 80.0, 85.0, "M", "Product 1" },
                    { new Guid("fbe600cf-1fee-4402-87c8-aacd627fcbd3"), new Guid("76d6be2f-8d6c-4e93-94cc-4eb0341950bc"), "Black", "Product 16 Description", "images\\product\\product.png", 45.0, 42.0, 38.0, 40.0, "M", "Product 16" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_AppUserId",
                table: "OrderHeaders",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_AppUserId",
                table: "ShoppingCarts",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
