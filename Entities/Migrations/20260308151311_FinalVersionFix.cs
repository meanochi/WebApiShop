using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class FinalVersionFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__3214EC078D1DB473", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    Profile_img_url = table.Column<string>(type: "nchar(2000)", fixedLength: true, maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Provider__3214EC07753D54FB", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    last_name = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: false),
                    email_address = table.Column<string>(type: "nchar(30)", fixedLength: true, maxLength: 30, nullable: true),
                    phone_number = table.Column<string>(type: "nchar(13)", fixedLength: true, maxLength: 13, nullable: true),
                    password = table.Column<string>(type: "nchar(15)", fixedLength: true, maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Shows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Begin_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    End_time = table.Column<TimeOnly>(type: "time", nullable: false),
                    Audience = table.Column<string>(type: "nchar(15)", fixedLength: true, maxLength: 15, nullable: false),
                    Sector = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: true),
                    Img_url = table.Column<string>(type: "nchar(2000)", fixedLength: true, maxLength: 2000, nullable: true),
                    Provider_id = table.Column<int>(type: "int", nullable: false),
                    Category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tmp_ms_x__3214EC072BDFDAA9", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shows_Categories",
                        column: x => x.Category_id,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shows_Providers",
                        column: x => x.Provider_id,
                        principalTable: "Providers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__3214EC077013FBAE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_User",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "money", nullable: true),
                    Show_id = table.Column<int>(type: "int", nullable: false),
                    Section_type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sections__3214EC0761B18F1A", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Shows",
                        column: x => x.Show_id,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderedSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Section_id = table.Column<int>(type: "int", nullable: false),
                    Row = table.Column<int>(type: "int", nullable: false),
                    Col = table.Column<int>(type: "int", nullable: false),
                    Order_id = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Show_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderedS__3214EC070B4BBFF8", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdSeats_Orders",
                        column: x => x.Order_id,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdSeats_Section",
                        column: x => x.Section_id,
                        principalTable: "Sections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrdSeats_Shows",
                        column: x => x.Show_id,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedSeats_Order_id",
                table: "OrderedSeats",
                column: "Order_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedSeats_Section_id",
                table: "OrderedSeats",
                column: "Section_id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedSeats_Show_id",
                table: "OrderedSeats",
                column: "Show_id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_User_id",
                table: "Orders",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetCodes_Email_Code",
                table: "PasswordResetCodes",
                columns: new[] { "Email", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_Sections_Show_id",
                table: "Sections",
                column: "Show_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_Category_id",
                table: "Shows",
                column: "Category_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_Provider_id",
                table: "Shows",
                column: "Provider_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedSeats");

            migrationBuilder.DropTable(
                name: "PasswordResetCodes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Shows");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
