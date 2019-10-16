using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Currencies.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.CreateTable(
                name: "CurrencyInfos",
                columns: table => new
                {
                    CharCode = table.Column<String>( nullable: false ),
                    Nominal = table.Column<Int32>( nullable: false ),
                    NumCode = table.Column<Int32>( nullable: false ),
                    Name = table.Column<String>( nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_CurrencyInfos", x => x.CharCode );
                } );

            migrationBuilder.CreateTable(
                name: "CurrencyValues",
                columns: table => new
                {
                    Id = table.Column<Int32>( nullable: false )
                        .Annotation( "SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn ),
                    Value = table.Column<Single>( nullable: false ),
                    Date = table.Column<DateTime>( nullable: false ),
                    CurrencyInfoId = table.Column<String>( nullable: true )
                },
                constraints: table =>
                {
                    table.PrimaryKey( "PK_CurrencyValues", x => x.Id );
                    table.ForeignKey(
                        name: "FK_CurrencyValues_CurrencyInfos_CurrencyInfoId",
                        column: x => x.CurrencyInfoId,
                        principalTable: "CurrencyInfos",
                        principalColumn: "CharCode",
                        onDelete: ReferentialAction.Restrict );
                } );

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyValues_CurrencyInfoId",
                table: "CurrencyValues",
                column: "CurrencyInfoId" );
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "CurrencyValues" );

            migrationBuilder.DropTable(
                name: "CurrencyInfos" );
        }
    }
}
