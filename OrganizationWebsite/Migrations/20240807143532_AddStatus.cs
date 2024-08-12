using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizationWebsite.Migrations
{
    /// <inheritdoc />
    public partial class AddStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "AspNetUsers",
            type: "nvarchar(max)",
            nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Status",
             table: "AspNetUsers");

        }
    }
}
