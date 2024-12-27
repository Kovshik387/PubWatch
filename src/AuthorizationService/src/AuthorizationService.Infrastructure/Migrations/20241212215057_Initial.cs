using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthorizationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authorization",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EmailNormalized = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("auth_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refreshes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    token = table.Column<string>(type: "text", nullable: false),
                    device = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    idaccount = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("refresh_pkey", x => x.id);
                    table.ForeignKey(
                        name: "refresh_idaccount_fkey",
                        column: x => x.idaccount,
                        principalTable: "authorization",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "auth_email_key",
                table: "authorization",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "auth_normalized_email_key",
                table: "authorization",
                column: "EmailNormalized",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refreshes_idaccount",
                table: "refreshes",
                column: "idaccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refreshes");

            migrationBuilder.DropTable(
                name: "authorization");
        }
    }
}
