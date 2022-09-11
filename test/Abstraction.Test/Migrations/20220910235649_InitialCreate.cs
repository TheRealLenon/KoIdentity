using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tekoding.KoIdentity.Abstraction.Test.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "KoIdentity.Abstraction.Tests");

            migrationBuilder.CreateTable(
                name: "BaseEntity",
                schema: "KoIdentity.Abstraction.Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEntity", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseEntity",
                schema: "KoIdentity.Abstraction.Tests");
        }
    }
}
