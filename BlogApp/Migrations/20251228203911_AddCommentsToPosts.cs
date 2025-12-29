using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Author", "Content", "PostId", "PostedDate" },
                values: new object[,]
                {
                    { 1, "Alice Brown", "Very interesting article about AI. Looking forward to more!", 1, new DateTime(2019, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Michael Green", "AI is definitely shaping our future. Great insights.", 1, new DateTime(2019, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Laura White", "These healthy living tips are really useful. Thanks!", 2, new DateTime(2020, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Chris Black", "I have already tried some of these tips and they work!", 2, new DateTime(2020, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Sophia Martinez", "Amazing travel destinations. Adding these to my bucket list!", 3, new DateTime(2025, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
