using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_UserRoleEntity_RolesId",
                table: "UsersRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleEntity",
                table: "UserRoleEntity");

            migrationBuilder.RenameTable(
                name: "UserRoleEntity",
                newName: "UserRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRoles_UserRoles_RolesId",
                table: "UsersRoles",
                column: "RolesId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_UserRoles_RolesId",
                table: "UsersRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoleEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleEntity",
                table: "UserRoleEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRoles_UserRoleEntity_RolesId",
                table: "UsersRoles",
                column: "RolesId",
                principalTable: "UserRoleEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
