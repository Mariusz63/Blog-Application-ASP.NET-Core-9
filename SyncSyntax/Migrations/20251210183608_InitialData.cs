using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Posts related to technology.", "Technology" },
                    { 2, "Posts related to health and wellness.", "Health" },
                    { 3, "Posts about travel experiences and tips.", "Travel" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Author", "CategoryId", "Content", "FeatureImagePath", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "John Doe", 1, "Content about AI...", "rise_of_ai_image.jpg", new DateTime(2019, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Rise of AI" },
                    { 2, "Jane Smith", 2, "Content about health...", "Healthy_image_jpg", new DateTime(2020, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Healthy Living Tips" },
                    { 3, "Emily Johnson", 3, "Content about travel...", "travel_Destinations_image.jpg", new DateTime(2025, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Top 10 Travel Destinations" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
