using Microsoft.EntityFrameworkCore.Migrations;

namespace webapp.Migrations
{
    public partial class seedEmployeetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "id", "Department", "Email", "Name" },
                values: new object[] { 1, 2, "nicolas@gmil.com", "Nicolas" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
