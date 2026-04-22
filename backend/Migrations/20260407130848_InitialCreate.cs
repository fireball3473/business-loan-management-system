using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciAdi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sifre = table.Column<string>(type: "text", nullable: false),
                    AdSoyad = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    MusteriId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VKN = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Unvan = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Sektor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.MusteriId);
                });

            migrationBuilder.CreateTable(
                name: "Krediler",
                columns: table => new
                {
                    KrediId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MusteriId = table.Column<int>(type: "integer", nullable: false),
                    KrediTuru = table.Column<string>(type: "text", nullable: false),
                    AnaPara = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ToplamBorc = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    KalanBorc = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FaizOrani = table.Column<decimal>(type: "numeric", nullable: false),
                    VadeSayisi = table.Column<int>(type: "integer", nullable: false),
                    DovizCinsi = table.Column<string>(type: "text", nullable: false),
                    OdemeTipi = table.Column<string>(type: "text", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Krediler", x => x.KrediId);
                    table.ForeignKey(
                        name: "FK_Krediler_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "MusteriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MevduatHesaplari",
                columns: table => new
                {
                    HesapId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MusteriId = table.Column<int>(type: "integer", nullable: false),
                    IbanNo = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    Bakiye = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DovizCinsi = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MevduatHesaplari", x => x.HesapId);
                    table.ForeignKey(
                        name: "FK_MevduatHesaplari_Musteriler_MusteriId",
                        column: x => x.MusteriId,
                        principalTable: "Musteriler",
                        principalColumn: "MusteriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tahsilatlar",
                columns: table => new
                {
                    TahsilatId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KrediId = table.Column<int>(type: "integer", nullable: false),
                    HesapId = table.Column<int>(type: "integer", nullable: false),
                    TahsilatTutari = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tahsilatlar", x => x.TahsilatId);
                    table.ForeignKey(
                        name: "FK_Tahsilatlar_Krediler_KrediId",
                        column: x => x.KrediId,
                        principalTable: "Krediler",
                        principalColumn: "KrediId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tahsilatlar_MevduatHesaplari_HesapId",
                        column: x => x.HesapId,
                        principalTable: "MevduatHesaplari",
                        principalColumn: "HesapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Krediler_MusteriId",
                table: "Krediler",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_MevduatHesaplari_MusteriId",
                table: "MevduatHesaplari",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_VKN",
                table: "Musteriler",
                column: "VKN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tahsilatlar_HesapId",
                table: "Tahsilatlar",
                column: "HesapId");

            migrationBuilder.CreateIndex(
                name: "IX_Tahsilatlar_KrediId",
                table: "Tahsilatlar",
                column: "KrediId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Tahsilatlar");

            migrationBuilder.DropTable(
                name: "Krediler");

            migrationBuilder.DropTable(
                name: "MevduatHesaplari");

            migrationBuilder.DropTable(
                name: "Musteriler");
        }
    }
}
