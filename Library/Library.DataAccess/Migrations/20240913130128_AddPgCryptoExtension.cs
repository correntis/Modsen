using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPgCryptoExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"pgcrypto\";");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksAuthors_Books_BooksId",
                table: "BooksAuthors");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "BooksAuthors",
                newName: "BookEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_BooksAuthors_BooksId",
                table: "BooksAuthors",
                newName: "IX_BooksAuthors_BookEntityId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Books",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Authors",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAuthors_Books_BookEntityId",
                table: "BooksAuthors",
                column: "BookEntityId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP EXTENSION IF EXISTS \"pgcrypto\";");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksAuthors_Books_BookEntityId",
                table: "BooksAuthors");

            migrationBuilder.RenameColumn(
                name: "BookEntityId",
                table: "BooksAuthors",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_BooksAuthors_BookEntityId",
                table: "BooksAuthors",
                newName: "IX_BooksAuthors_BooksId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Books",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Authors",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksAuthors_Books_BooksId",
                table: "BooksAuthors",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
