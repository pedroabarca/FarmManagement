using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FarmManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCattleManagementSchemaV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add new TagId column (not unique yet, to allow data population)
            migrationBuilder.AddColumn<string>(
                name: "TagId",
                table: "Animals",
                type: "text",
                nullable: true);  // temporarily nullable

            // Step 2: Generate unique TagIds for existing animals
            migrationBuilder.Sql(@"
                UPDATE ""Animals""
                SET ""TagId"" = 'A-' || ""Id""::text
                WHERE ""TagId"" IS NULL;
            ");

            // Step 3: Make TagId required
            migrationBuilder.AlterColumn<string>(
                name: "TagId",
                table: "Animals",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            // Step 4: Rename DateOfBirth to UpdatedAt and copy the value
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Animals",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animals",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            // Step 6: Add BirthDate and copy from DateOfBirth (which is now UpdatedAt)
            // Note: DateOfBirth was renamed to UpdatedAt, so we copy from UpdatedAt
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.Sql(@"
                UPDATE ""Animals""
                SET ""BirthDate"" = ""UpdatedAt"";
            ");

            migrationBuilder.AddColumn<decimal>(
                name: "BirthWeightKg",
                table: "Animals",
                type: "numeric",
                nullable: true);

            // Step 5: Add Breed column and copy Species data to it
            migrationBuilder.AddColumn<string>(
                name: "Breed",
                table: "Animals",
                type: "text",
                nullable: true);  // temporarily nullable

            migrationBuilder.Sql(@"
                UPDATE ""Animals""
                SET ""Breed"" = ""Species""
                WHERE ""Breed"" IS NULL;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "Breed",
                table: "Animals",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            // Step 7: Add CreatedAt with current timestamp
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            // Step 8: Update UpdatedAt to current timestamp
            migrationBuilder.Sql(@"
                UPDATE ""Animals""
                SET ""UpdatedAt"" = NOW();
            ");

            migrationBuilder.AddColumn<string>(
                name: "CurrentLocation",
                table: "Animals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DamId",
                table: "Animals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ElectronicId",
                table: "Animals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastBreedingDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCalvingDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastHeatDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPalpationDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextExpectedCalvingDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Animals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Animals",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SireId",
                table: "Animals",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WeaningDate",
                table: "Animals",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WeaningWeightKg",
                table: "Animals",
                type: "numeric",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BirthRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DamId = table.Column<int>(type: "integer", nullable: false),
                    CalfId = table.Column<int>(type: "integer", nullable: true),
                    CalvingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CalvingEase = table.Column<int>(type: "integer", nullable: false),
                    CalfSex = table.Column<int>(type: "integer", nullable: false),
                    CalfBirthWeightKg = table.Column<decimal>(type: "numeric", nullable: true),
                    CalfStatus = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirthRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BirthRecords_Animals_CalfId",
                        column: x => x.CalfId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BirthRecords_Animals_DamId",
                        column: x => x.DamId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BreedingRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SireId = table.Column<int>(type: "integer", nullable: true),
                    BreedingMethod = table.Column<int>(type: "integer", nullable: true),
                    PregnancyStatus = table.Column<int>(type: "integer", nullable: true),
                    ExpectedCalvingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TechnicianName = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BreedingRecords_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreedingRecords_Animals_SireId",
                        column: x => x.SireId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HealthRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Diagnosis = table.Column<string>(type: "text", nullable: true),
                    Treatment = table.Column<string>(type: "text", nullable: true),
                    Medication = table.Column<string>(type: "text", nullable: true),
                    AdministeredBy = table.Column<string>(type: "text", nullable: true),
                    NextDueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthRecords_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnimalId = table.Column<int>(type: "integer", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MeasurementType = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightRecords_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_DamId",
                table: "Animals",
                column: "DamId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_ElectronicId",
                table: "Animals",
                column: "ElectronicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Animals_SireId",
                table: "Animals",
                column: "SireId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_TagId",
                table: "Animals",
                column: "TagId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BirthRecords_CalfId",
                table: "BirthRecords",
                column: "CalfId");

            migrationBuilder.CreateIndex(
                name: "IX_BirthRecords_DamId",
                table: "BirthRecords",
                column: "DamId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_AnimalId_EventDate",
                table: "BreedingRecords",
                columns: new[] { "AnimalId", "EventDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BreedingRecords_SireId",
                table: "BreedingRecords",
                column: "SireId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthRecords_AnimalId_EventDate",
                table: "HealthRecords",
                columns: new[] { "AnimalId", "EventDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WeightRecords_AnimalId_MeasurementDate",
                table: "WeightRecords",
                columns: new[] { "AnimalId", "MeasurementDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Animals_DamId",
                table: "Animals",
                column: "DamId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Animals_SireId",
                table: "Animals",
                column: "SireId",
                principalTable: "Animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Animals_DamId",
                table: "Animals");

            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Animals_SireId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "BirthRecords");

            migrationBuilder.DropTable(
                name: "BreedingRecords");

            migrationBuilder.DropTable(
                name: "HealthRecords");

            migrationBuilder.DropTable(
                name: "WeightRecords");

            migrationBuilder.DropIndex(
                name: "IX_Animals_DamId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_ElectronicId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_SireId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_TagId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "BirthWeightKg",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Breed",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "CurrentLocation",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "DamId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "ElectronicId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "LastBreedingDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "LastCalvingDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "LastHeatDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "LastPalpationDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "NextExpectedCalvingDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "SireId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "WeaningDate",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "WeaningWeightKg",
                table: "Animals");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Animals",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Animals",
                newName: "Species");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animals",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
