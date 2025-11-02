using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBreedRegistryAndMixedBreedSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the obsolete Species column (replaced by Breed)
            migrationBuilder.DropColumn(
                name: "Species",
                table: "Animals");

            migrationBuilder.AddColumn<string>(
                name: "BreedComposition",
                table: "Animals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPurebred",
                table: "Animals",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Animals",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                table: "Animals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistryOrganization",
                table: "Animals",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedComposition",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "IsPurebred",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "RegistryOrganization",
                table: "Animals");
        }
    }
}
