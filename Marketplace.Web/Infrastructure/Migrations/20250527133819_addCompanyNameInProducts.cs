using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Marketplace.Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyNameInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorsCompanyName",
                table: "products",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorsCompanyName",
                table: "products");
        }
    }
}
