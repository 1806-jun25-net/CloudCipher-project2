using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantAPI.API.Migrations.Project2DB
{
    public partial class Project2DBMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "RestaurantSite");

            migrationBuilder.CreateTable(
                name: "AppUser",
                schema: "RestaurantSite",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    FirstName = table.Column<string>(maxLength: 128, nullable: false),
                    LastName = table.Column<string>(maxLength: 128, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    UserType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Keyword",
                schema: "RestaurantSite",
                columns: table => new
                {
                    Word = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.Word);
                });

            migrationBuilder.CreateTable(
                name: "Query",
                schema: "RestaurantSite",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    Location = table.Column<string>(maxLength: 128, nullable: true),
                    Location2 = table.Column<string>(maxLength: 128, nullable: true),
                    Radius = table.Column<int>(nullable: true),
                    QueryTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReservationTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Query", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Query__Username__66603565",
                        column: x => x.Username,
                        principalSchema: "RestaurantSite",
                        principalTable: "AppUser",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                schema: "RestaurantSite",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Phone = table.Column<string>(maxLength: 128, nullable: true),
                    Hours = table.Column<string>(maxLength: 128, nullable: true),
                    Location = table.Column<string>(maxLength: 128, nullable: false),
                    Location2 = table.Column<string>(maxLength: 128, nullable: true),
                    Owner = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.ID);
                    table.ForeignKey(
                        name: "FK__Restauran__Owner__6383C8BA",
                        column: x => x.Owner,
                        principalSchema: "RestaurantSite",
                        principalTable: "AppUser",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueryKeywordJunction",
                schema: "RestaurantSite",
                columns: table => new
                {
                    QueryID = table.Column<int>(nullable: false),
                    Word = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryKeywordJunction", x => new { x.QueryID, x.Word });
                    table.ForeignKey(
                        name: "FK__QueryKeyw__Query__06CD04F7",
                        column: x => x.QueryID,
                        principalSchema: "RestaurantSite",
                        principalTable: "Query",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__QueryKeywo__Word__07C12930",
                        column: x => x.Word,
                        principalSchema: "RestaurantSite",
                        principalTable: "Keyword",
                        principalColumn: "Word",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blacklist",
                schema: "RestaurantSite",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blacklist", x => new { x.RestaurantID, x.Username });
                    table.ForeignKey(
                        name: "FK__Blacklist__Resta__02FC7413",
                        column: x => x.RestaurantID,
                        principalSchema: "RestaurantSite",
                        principalTable: "Restaurant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Blacklist__Usern__03F0984C",
                        column: x => x.Username,
                        principalSchema: "RestaurantSite",
                        principalTable: "AppUser",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Favorite",
                schema: "RestaurantSite",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => new { x.RestaurantID, x.Username });
                    table.ForeignKey(
                        name: "FK__Favorite__Restau__7F2BE32F",
                        column: x => x.RestaurantID,
                        principalSchema: "RestaurantSite",
                        principalTable: "Restaurant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Favorite__Userna__00200768",
                        column: x => x.Username,
                        principalSchema: "RestaurantSite",
                        principalTable: "AppUser",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RestaurantKeywordJunction",
                schema: "RestaurantSite",
                columns: table => new
                {
                    RestaurantID = table.Column<int>(nullable: false),
                    Word = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantKeywordJunction", x => new { x.RestaurantID, x.Word });
                    table.ForeignKey(
                        name: "FK__Restauran__Resta__0A9D95DB",
                        column: x => x.RestaurantID,
                        principalSchema: "RestaurantSite",
                        principalTable: "Restaurant",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Restaurant__Word__0B91BA14",
                        column: x => x.Word,
                        principalSchema: "RestaurantSite",
                        principalTable: "Keyword",
                        principalColumn: "Word",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blacklist_Username",
                schema: "RestaurantSite",
                table: "Blacklist",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_Username",
                schema: "RestaurantSite",
                table: "Favorite",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Query_Username",
                schema: "RestaurantSite",
                table: "Query",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_QueryKeywordJunction_Word",
                schema: "RestaurantSite",
                table: "QueryKeywordJunction",
                column: "Word");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Owner",
                schema: "RestaurantSite",
                table: "Restaurant",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantKeywordJunction_Word",
                schema: "RestaurantSite",
                table: "RestaurantKeywordJunction",
                column: "Word");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blacklist",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "Favorite",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "QueryKeywordJunction",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "RestaurantKeywordJunction",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "Query",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "Restaurant",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "Keyword",
                schema: "RestaurantSite");

            migrationBuilder.DropTable(
                name: "AppUser",
                schema: "RestaurantSite");
        }
    }
}
