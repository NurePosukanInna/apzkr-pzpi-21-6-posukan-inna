using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionType",
                columns: table => new
                {
                    subscription_type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionType", x => x.subscription_type_id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    supplier_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.supplier_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    supplier_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK__Product__categor__4316F928",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "category_id");
                    table.ForeignKey(
                        name: "FK__Product__supplie__440B1D61",
                        column: x => x.supplier_id,
                        principalTable: "Supplier",
                        principalColumn: "supplier_id");
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    store_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.store_id);
                    table.ForeignKey(
                        name: "FK__Store__user_id__398D8EEE",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    subscription_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionType_ID = table.Column<int>(type: "int", nullable: true),
                    SubscriptionStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.subscription_id);
                    table.ForeignKey(
                        name: "FK__Subscript__Subsc__59FA5E80",
                        column: x => x.SubscriptionType_ID,
                        principalTable: "SubscriptionType",
                        principalColumn: "subscription_type_id");
                    table.ForeignKey(
                        name: "FK__Subscript__user___5AEE82B9",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Defective_Products",
                columns: table => new
                {
                    defective_product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateDetected = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defective_Products", x => x.defective_product_id);
                    table.ForeignKey(
                        name: "FK__Defective__produ__52593CB8",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id");
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    store_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK__Employee__store___3C69FB99",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    sensor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Store_ID = table.Column<int>(type: "int", nullable: true),
                    Temperature = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.sensor_id);
                    table.ForeignKey(
                        name: "FK__Sensor__Store_ID__5DCAEF64",
                        column: x => x.Store_ID,
                        principalTable: "Store",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "Store_Products",
                columns: table => new
                {
                    store_product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    store_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    MinQuantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store_Products", x => x.store_product_id);
                    table.ForeignKey(
                        name: "FK__Store_Pro__produ__4F7CD00D",
                        column: x => x.product_id,
                        principalTable: "Product",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "FK__Store_Pro__store__4E88ABD4",
                        column: x => x.store_id,
                        principalTable: "Store",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    sale_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Employee_ID = table.Column<int>(type: "int", nullable: true),
                    Store_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.sale_id);
                    table.ForeignKey(
                        name: "FK__Sale__Employee_I__46E78A0C",
                        column: x => x.Employee_ID,
                        principalTable: "Employee",
                        principalColumn: "employee_id");
                    table.ForeignKey(
                        name: "FK__Sale__Store_ID__47DBAE45",
                        column: x => x.Store_ID,
                        principalTable: "Store",
                        principalColumn: "store_id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierRequest",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    store_product_id = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RequestStatus = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__18D3B90F48696F5B", x => x.request_id);
                    table.ForeignKey(
                        name: "FK__SupplierR__store__5535A963",
                        column: x => x.store_product_id,
                        principalTable: "Store_Products",
                        principalColumn: "store_product_id");
                });

            migrationBuilder.CreateTable(
                name: "Sale_Items",
                columns: table => new
                {
                    sale_item_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sale_ID = table.Column<int>(type: "int", nullable: true),
                    Product_ID = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale_Items", x => x.sale_item_id);
                    table.ForeignKey(
                        name: "FK__Sale_Item__Produ__4BAC3F29",
                        column: x => x.Product_ID,
                        principalTable: "Product",
                        principalColumn: "product_id");
                    table.ForeignKey(
                        name: "FK__Sale_Item__Sale___4AB81AF0",
                        column: x => x.Sale_ID,
                        principalTable: "Sale",
                        principalColumn: "sale_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Defective_Products_product_id",
                table: "Defective_Products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_store_id",
                table: "Employee",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_category_id",
                table: "Product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Product_supplier_id",
                table: "Product",
                column: "supplier_id");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Employee_ID",
                table: "Sale",
                column: "Employee_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Store_ID",
                table: "Sale",
                column: "Store_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Items_Product_ID",
                table: "Sale_Items",
                column: "Product_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Items_Sale_ID",
                table: "Sale_Items",
                column: "Sale_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Sensor_Store_ID",
                table: "Sensor",
                column: "Store_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Store_user_id",
                table: "Store",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Products_product_id",
                table: "Store_Products",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_Store_Products_store_id",
                table: "Store_Products",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriptionType_ID",
                table: "Subscription",
                column: "SubscriptionType_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_user_id",
                table: "Subscription",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequest_store_product_id",
                table: "SupplierRequest",
                column: "store_product_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Defective_Products");

            migrationBuilder.DropTable(
                name: "Sale_Items");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "SupplierRequest");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "SubscriptionType");

            migrationBuilder.DropTable(
                name: "Store_Products");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Supplier");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
