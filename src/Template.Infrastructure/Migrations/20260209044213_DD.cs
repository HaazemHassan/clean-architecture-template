using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Template.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "RefreshToken",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "RefreshToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "RefreshToken",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "DomainUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "DomainUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "DomainUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DomainUser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "DomainUser",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DomainUser");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DomainUser");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RefreshToken",
                newName: "Created");
        }
    }
}
