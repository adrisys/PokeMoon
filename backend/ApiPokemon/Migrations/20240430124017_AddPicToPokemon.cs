using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPokemon.Migrations
{
    /// <inheritdoc />
    public partial class AddPicToPokemon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicURL",
                table: "pokemon",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicUrl",
                table: "pokemon");
        }
    }
}
