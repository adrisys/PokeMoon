using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPokemon.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abilities",
                columns: table => new
                {
                    IDability = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    abilityname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abilities", x => x.IDability);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    IDcat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.IDcat);
                });

            migrationBuilder.CreateTable(
                name: "Egggroups",
                columns: table => new
                {
                    IDegg = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    eggname = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egggroups", x => x.IDegg);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    IDpoke = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pokename = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HP = table.Column<byte>(type: "tinyint", nullable: false),
                    attack = table.Column<byte>(type: "tinyint", nullable: false),
                    defense = table.Column<byte>(type: "tinyint", nullable: false),
                    spattack = table.Column<byte>(type: "tinyint", nullable: false),
                    spdefense = table.Column<byte>(type: "tinyint", nullable: false),
                    speed = table.Column<byte>(type: "tinyint", nullable: false),
                    dualtype = table.Column<bool>(type: "bit", nullable: false),
                    PicURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.IDpoke);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    IDtype = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.IDtype);
                });

            migrationBuilder.CreateTable(
                name: "poke-ability",
                columns: table => new
                {
                    IDpoke = table.Column<int>(type: "int", nullable: false),
                    IDability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poke-ability", x => new { x.IDpoke, x.IDability });
                    table.ForeignKey(
                        name: "FK_poke-ability_Abilities_IDability",
                        column: x => x.IDability,
                        principalTable: "Abilities",
                        principalColumn: "IDability",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_poke-ability_Pokemons_IDpoke",
                        column: x => x.IDpoke,
                        principalTable: "Pokemons",
                        principalColumn: "IDpoke",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "poke-egg",
                columns: table => new
                {
                    IDpoke = table.Column<int>(type: "int", nullable: false),
                    IDegg = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poke-egg", x => new { x.IDpoke, x.IDegg });
                    table.ForeignKey(
                        name: "FK_poke-egg_Egggroups_IDegg",
                        column: x => x.IDegg,
                        principalTable: "Egggroups",
                        principalColumn: "IDegg",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_poke-egg_Pokemons_IDpoke",
                        column: x => x.IDpoke,
                        principalTable: "Pokemons",
                        principalColumn: "IDpoke",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Moves",
                columns: table => new
                {
                    IDmove = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movename = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDtype = table.Column<int>(type: "int", nullable: false),
                    IDcat = table.Column<int>(type: "int", nullable: false),
                    power = table.Column<byte>(type: "tinyint", nullable: true),
                    accuracy = table.Column<byte>(type: "tinyint", nullable: true),
                    PP = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moves", x => x.IDmove);
                    table.ForeignKey(
                        name: "fk_moves_cat",
                        column: x => x.IDcat,
                        principalTable: "Categories",
                        principalColumn: "IDcat");
                    table.ForeignKey(
                        name: "fk_moves_type",
                        column: x => x.IDtype,
                        principalTable: "Types",
                        principalColumn: "IDtype");
                });

            migrationBuilder.CreateTable(
                name: "poke-type",
                columns: table => new
                {
                    IDpoke = table.Column<int>(type: "int", nullable: false),
                    IDtype = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poke-type", x => new { x.IDpoke, x.IDtype });
                    table.ForeignKey(
                        name: "FK_poke-type_Pokemons_IDpoke",
                        column: x => x.IDpoke,
                        principalTable: "Pokemons",
                        principalColumn: "IDpoke",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_poke-type_Types_IDtype",
                        column: x => x.IDtype,
                        principalTable: "Types",
                        principalColumn: "IDtype",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "poke-move",
                columns: table => new
                {
                    IDpoke = table.Column<int>(type: "int", nullable: false),
                    IDmove = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_poke-move", x => new { x.IDpoke, x.IDmove });
                    table.ForeignKey(
                        name: "FK_poke-move_Moves_IDmove",
                        column: x => x.IDmove,
                        principalTable: "Moves",
                        principalColumn: "IDmove",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_poke-move_Pokemons_IDpoke",
                        column: x => x.IDpoke,
                        principalTable: "Pokemons",
                        principalColumn: "IDpoke",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "fk_moves_cat",
                table: "Moves",
                column: "IDcat");

            migrationBuilder.CreateIndex(
                name: "fk_moves_type",
                table: "Moves",
                column: "IDtype");

            migrationBuilder.CreateIndex(
                name: "movename",
                table: "Moves",
                column: "movename",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_poke-ability_IDability",
                table: "poke-ability",
                column: "IDability");

            migrationBuilder.CreateIndex(
                name: "IX_poke-egg_IDegg",
                table: "poke-egg",
                column: "IDegg");

            migrationBuilder.CreateIndex(
                name: "IX_poke-move_IDmove",
                table: "poke-move",
                column: "IDmove");

            migrationBuilder.CreateIndex(
                name: "IX_poke-type_IDtype",
                table: "poke-type",
                column: "IDtype");

            migrationBuilder.CreateIndex(
                name: "pokename",
                table: "Pokemons",
                column: "pokename",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "poke-ability");

            migrationBuilder.DropTable(
                name: "poke-egg");

            migrationBuilder.DropTable(
                name: "poke-move");

            migrationBuilder.DropTable(
                name: "poke-type");

            migrationBuilder.DropTable(
                name: "Abilities");

            migrationBuilder.DropTable(
                name: "Egggroups");

            migrationBuilder.DropTable(
                name: "Moves");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
