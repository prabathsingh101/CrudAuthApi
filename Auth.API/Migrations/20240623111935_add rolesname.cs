using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.API.Migrations
{
    /// <inheritdoc />
    public partial class addrolesname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "GetAllUsers",
                newName: "rolename");

            migrationBuilder.AddColumn<string>(
                name: "mobileno",
                table: "GetAllUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mobileno",
                table: "GetAllUsers");

            migrationBuilder.RenameColumn(
                name: "rolename",
                table: "GetAllUsers",
                newName: "Roles");
        }
    }
}
