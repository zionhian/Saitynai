using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaitynaiBackend.Migrations
{
    /// <inheritdoc />
    public partial class updatestuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Publishers_OwnerId",
                table: "Publishers");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_OwnerId",
                table: "Publishers",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Publishers_OwnerId",
                table: "Publishers");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_OwnerId",
                table: "Publishers",
                column: "OwnerId",
                unique: true);
        }
    }
}
