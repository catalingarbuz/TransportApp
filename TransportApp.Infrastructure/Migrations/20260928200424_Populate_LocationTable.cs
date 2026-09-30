using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportApp.Infrastructure.Migrations
{
    public partial class Populate_LocationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ""Location"" (""Id"", ""City"", ""Country"", ""CreatedAt"", ""UpdatedAt"")
                VALUES 
                    (gen_random_uuid(), 'Chisinau', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Soroca', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Balti', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Tiraspol', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Drochia', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Otaci', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'Edineț', 'Moldova', NOW(), NOW()),
                    (gen_random_uuid(), 'București', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Iași', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Cluj-Napoca', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Timișoara', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Constanța', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Brașov', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Sibiu', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Oradea', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Arad', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Galați', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Ploiești', 'România', NOW(), NOW()),
                    (gen_random_uuid(), 'Craiova', 'România', NOW(), NOW())
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""Location""
                WHERE (""City"", ""Country"") IN (
                    ('Chisinau', 'Moldova'),
                    ('Soroca', 'Moldova'),
                    ('Balti', 'Moldova'),
                    ('Tiraspol', 'Moldova'),
                    ('Drochia', 'Moldova'),
                    ('Otaci', 'Moldova'),
                    ('Edineț', 'Moldova'),
                    ('București', 'România'),
                    ('Iași', 'România'),
                    ('Cluj-Napoca', 'România'),
                    ('Timișoara', 'România'),
                    ('Constanța', 'România'),
                    ('Brașov', 'România'),
                    ('Sibiu', 'România'),
                    ('Oradea', 'România'),
                    ('Arad', 'România'),
                    ('Galați', 'România'),
                    ('Ploiești', 'România'),
                    ('Craiova', 'România')
                )
            ");
        }
    }
}
