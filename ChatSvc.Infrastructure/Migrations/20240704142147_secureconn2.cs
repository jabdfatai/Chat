using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureCommSvc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secureconn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "useremail",
                table: "userdevices",
                newName: "channel");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "userdevices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    securekey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    DT_CRTD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DT_MODF = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UNIQUE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "userdevices");

            migrationBuilder.RenameColumn(
                name: "channel",
                table: "userdevices",
                newName: "useremail");
        }
    }
}
