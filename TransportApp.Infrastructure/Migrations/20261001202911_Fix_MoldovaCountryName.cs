using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class Fix_MoldovaCountryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Location""
                SET ""Country"" = 'Republica Moldova'
                WHERE ""Country"" = 'Moldova';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
              UPDATE ""Location""
                SET ""Country"" = 'Moldova'
                WHERE ""Country"" = 'Republica Moldova';"); 

        }
    }
}
