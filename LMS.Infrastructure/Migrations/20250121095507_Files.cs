using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Files : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityDocuments");

            migrationBuilder.DropTable(
                name: "CourseDocuments");

            migrationBuilder.DropTable(
                name: "ModuleDocuments");

            migrationBuilder.CreateTable(
                name: "UserFiles",
                columns: table => new
                {
                    UserFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    ActivityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.UserFileId);
                    table.ForeignKey(
                        name: "FK_UserFiles_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId");
                    table.ForeignKey(
                        name: "FK_UserFiles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFiles_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId");
                    table.ForeignKey(
                        name: "FK_UserFiles_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId");
                });

            migrationBuilder.InsertData(
                table: "ActivityTypes",
                columns: new[] { "ActivityTypeId", "Type" },
                values: new object[] { 1, "Föreläsning" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 1, "Lorem ipsum odor amet.", new DateTime(2025, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fullstack.NET 2025", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "ModuleId", "CourseId", "Description", "EndDate", "Name", "StartDate" },
                values: new object[] { 1, 1, "C# module", new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "C#-basics", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "ActivityId", "ActivityTypeId", "Description", "EndDate", "ModuleId", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, "Asp.Net", new DateTime(2025, 1, 9, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, "Föreläsning - C#", new DateTime(2025, 1, 9, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Spring Boot", new DateTime(2025, 1, 10, 15, 0, 0, 0, DateTimeKind.Unspecified), 1, "Föreläsning - Java", new DateTime(2025, 1, 10, 11, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_ActivityId",
                table: "UserFiles",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_ApplicationUserId",
                table: "UserFiles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_CourseId",
                table: "UserFiles",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_ModuleId",
                table: "UserFiles",
                column: "ModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "ActivityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ActivityTypes",
                keyColumn: "ActivityTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "ModuleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "CourseId",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "ActivityDocuments",
                columns: table => new
                {
                    ActivityDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityDocuments", x => x.ActivityDocumentId);
                    table.ForeignKey(
                        name: "FK_ActivityDocuments_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "ActivityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityDocuments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseDocuments",
                columns: table => new
                {
                    CourseDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocuments", x => x.CourseDocumentId);
                    table.ForeignKey(
                        name: "FK_CourseDocuments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseDocuments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleDocuments",
                columns: table => new
                {
                    ModuleDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleDocuments", x => x.ModuleDocumentId);
                    table.ForeignKey(
                        name: "FK_ModuleDocuments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleDocuments_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDocuments_ActivityId",
                table: "ActivityDocuments",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityDocuments_ApplicationUserId",
                table: "ActivityDocuments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_ApplicationUserId",
                table: "CourseDocuments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseDocuments_CourseId",
                table: "CourseDocuments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleDocuments_ApplicationUserId",
                table: "ModuleDocuments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleDocuments_ModuleId",
                table: "ModuleDocuments",
                column: "ModuleId");
        }
    }
}
