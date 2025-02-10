using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExchangeService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quotation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("quotation_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    idname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    numcode = table.Column<int>(type: "integer", nullable: false),
                    charcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nominal = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false),
                    vunitrate = table.Column<decimal>(type: "numeric", nullable: false),
                    valcursid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("currency_pkey", x => x.id);
                    table.ForeignKey(
                        name: "currency_quotationid_fkey",
                        column: x => x.valcursid,
                        principalTable: "quotation",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_currency_valcursid",
                table: "currency",
                column: "valcursid");

            migrationBuilder.CreateIndex(
                name: "date",
                table: "quotation",
                column: "date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "quotation_date_key",
                table: "quotation",
                column: "date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropTable(
                name: "quotation");
        }
    }
}
