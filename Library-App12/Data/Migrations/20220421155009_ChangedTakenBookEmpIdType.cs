using Microsoft.EntityFrameworkCore.Migrations;

namespace Library_App.Data.Migrations
{
    public partial class ChangedTakenBookEmpIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakeBooks_Employees_EmployeeId1",
                table: "TakeBooks");

            migrationBuilder.DropIndex(
                name: "IX_TakeBooks_EmployeeId1",
                table: "TakeBooks");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "TakeBooks");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TakeBooks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "OrderListVM",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TakeBooks_EmployeeId",
                table: "TakeBooks",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TakeBooks_Employees_EmployeeId",
                table: "TakeBooks",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TakeBooks_Employees_EmployeeId",
                table: "TakeBooks");

            migrationBuilder.DropIndex(
                name: "IX_TakeBooks_EmployeeId",
                table: "TakeBooks");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "TakeBooks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "TakeBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "OrderListVM",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TakeBooks_EmployeeId1",
                table: "TakeBooks",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TakeBooks_Employees_EmployeeId1",
                table: "TakeBooks",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
