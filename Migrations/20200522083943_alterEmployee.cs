using Microsoft.EntityFrameworkCore.Migrations;

namespace webapp.Migrations
{
    public partial class alterEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "id",
                keyValue: 1,
                column: "Name",
                value: "Nic");

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "id", "Department", "Email", "Name" },
                values: new object[] { 2, 1, "joseline@gmil.com", "joseline" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "id",
                keyValue: 1,
                column: "Name",
                value: "Nicolas");
        }
    }
}
