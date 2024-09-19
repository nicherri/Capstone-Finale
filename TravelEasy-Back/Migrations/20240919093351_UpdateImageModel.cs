using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelEasy.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image1Url",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Image2Url",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Image3Url",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Images",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<bool>(
                name: "IsCover",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCover",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Images",
                newName: "CoverImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "Image1Url",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image2Url",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3Url",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
