using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRefreshTokenRevokedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RevokedAt",
                table: "UserRefreshTokens",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "UserRefreshTokens");
        }
    }
}
