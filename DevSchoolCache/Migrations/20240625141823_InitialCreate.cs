using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DevSchoolCache.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "School",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    SchoolId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_School_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "School",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_SchoolId",
                table: "Staff",
                column: "SchoolId");
            
            migrationBuilder.InsertData(
                table: "School",
                columns: new[] { "Id", "FullName", "City", "Address", "Email" },
                values: new object[,]
                {
                    { 1L, "Greenwood High School", "Greenwood", "123 Elm Street", "contact@greenwoodhigh.edu" },
                    { 2L, "Riverside Elementary School", "Riverside", "456 Oak Avenue", "info@riversideelementary.edu" },
                    { 3L, "Mountainview Middle School", "Mountainview", "789 Pine Road", "support@mountainview.edu" }
                });
            
            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "FirstName", "LastName", "MiddleName", "Birthday", "Position", "SchoolId", "Email" },
                values: new object[,]
                {
                    { 1L, "John", "Doe", "A.", new DateOnly(1980, 5, 15), Position.Teacher.ToString(), 3L, "john.doe@greenwoodhigh.edu" },
                    { 2L, "Jane", "Smith", "B.", new DateOnly(1975, 8, 20), Position.HeadTeacher.ToString(), 1L, "jane.smith@riversideelementary.edu" },
                    { 3L, "Mark", "Johnson", "C.", new DateOnly(1990, 12, 5), Position.Janitor.ToString(), 2L, "mark.johnson@mountainview.edu" },
                    { 4L, "Emily", "Davis", "D.", new DateOnly(1985, 3, 25), Position.Librarian.ToString(), 2L, "emily.davis@greenwoodhigh.edu" },
                    { 5L, "Michael", "Brown", "E.", new DateOnly(1982, 7, 10), Position.Guard.ToString(), 1L, "michael.brown@riversideelementary.edu" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "School");
        }
    }
}
