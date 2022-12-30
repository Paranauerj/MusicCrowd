using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoCrowdsourcing.Migrations
{
    /// <inheritdoc />
    public partial class withinstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Instrument",
                table: "HITs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Instrument",
                table: "HITs");
        }
    }
}
