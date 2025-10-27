using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contribute.Migrations
{
    /// <inheritdoc />
    public partial class IncludeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receipients",
                table: "Contributors");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Receipients",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                collation: "utf8mb3_general_ci")
                .Annotation("MySql:CharSet", "utf8mb3");

            migrationBuilder.AddColumn<int>(
                name: "ReceipientId",
                table: "Contributors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contributors_ReceipientId",
                table: "Contributors",
                column: "ReceipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributors_Receipients_ReceipientId",
                table: "Contributors",
                column: "ReceipientId",
                principalTable: "Receipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributors_Receipients_ReceipientId",
                table: "Contributors");

            migrationBuilder.DropIndex(
                name: "IX_Contributors_ReceipientId",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Receipients");

            migrationBuilder.DropColumn(
                name: "ReceipientId",
                table: "Contributors");

            migrationBuilder.AddColumn<string>(
                name: "Receipients",
                table: "Contributors",
                type: "longtext",
                nullable: false,
                collation: "utf8mb3_general_ci")
                .Annotation("MySql:CharSet", "utf8mb3");
        }
    }
}
