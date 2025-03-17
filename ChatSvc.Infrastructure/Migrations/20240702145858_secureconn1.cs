using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureCommSvc.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class secureconn1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.CreateTable(
                name: "userdevices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    device_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    useremail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    DT_CRTD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DT_MODF = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UNIQUE_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userdevices", x => x.id);
                });

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userdevices");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "chats");
        }
    }
}
