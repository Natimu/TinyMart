using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyMartAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Owner_FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    Owner_LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    ReviewRate = table.Column<float>(type: "REAL", nullable: false),
                    CartId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Singer_FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    Singer_LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Genre = table.Column<int>(type: "INTEGER", nullable: true),
                    Author_FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    Author_LastName = table.Column<string>(type: "TEXT", nullable: true),
                    Pages = table.Column<int>(type: "INTEGER", nullable: true),
                    Director_FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    Director_LastName = table.Column<string>(type: "TEXT", nullable: true),
                    FilmRate = table.Column<int>(type: "INTEGER", nullable: true),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: true),
                    RunTime = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartId",
                table: "Products",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}
