using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pamaxie.Database.Sql.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgressiveUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgressiveUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AppTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastAuth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateLimited = table.Column<bool>(type: "bit", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "BankingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BitcoinUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BitcoinUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoJackingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoJackingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DDosUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DDosUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GamblingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamblingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HackingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HackingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixedAdultUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixedAdultUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhishingUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhishingUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PornographicUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PornographicUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedirectorUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedirectorUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedAccount = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_GoogleUserId", x => x.GoogleUserId);
                });

            migrationBuilder.CreateTable(
                name: "WarezUrls",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarezUrls", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisingUrls");

            migrationBuilder.DropTable(
                name: "AgressiveUrls");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "BankingUrls");

            migrationBuilder.DropTable(
                name: "BitcoinUrls");

            migrationBuilder.DropTable(
                name: "CryptoJackingUrls");

            migrationBuilder.DropTable(
                name: "DDosUrls");

            migrationBuilder.DropTable(
                name: "DrugUrls");

            migrationBuilder.DropTable(
                name: "GamblingUrls");

            migrationBuilder.DropTable(
                name: "HackingUrls");

            migrationBuilder.DropTable(
                name: "MarketingUrls");

            migrationBuilder.DropTable(
                name: "MixedAdultUrls");

            migrationBuilder.DropTable(
                name: "PhishingUrls");

            migrationBuilder.DropTable(
                name: "PornographicUrls");

            migrationBuilder.DropTable(
                name: "RedirectorUrls");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WarezUrls");
        }
    }
}
