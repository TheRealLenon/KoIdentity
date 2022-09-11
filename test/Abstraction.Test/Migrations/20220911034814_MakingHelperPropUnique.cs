using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tekoding.KoIdentity.Abstraction.Test.Migrations
{
    public partial class MakingHelperPropUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestProp",
                schema: "KoIdentity.Abstraction.Tests",
                table: "BaseEntity",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntity_TestProp",
                schema: "KoIdentity.Abstraction.Tests",
                table: "BaseEntity",
                column: "TestProp",
                unique: true,
                filter: "[TestProp] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BaseEntity_TestProp",
                schema: "KoIdentity.Abstraction.Tests",
                table: "BaseEntity");

            migrationBuilder.DropColumn(
                name: "TestProp",
                schema: "KoIdentity.Abstraction.Tests",
                table: "BaseEntity");
        }
    }
}
