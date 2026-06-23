using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonKeThuocTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DonKeThuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BenhNhanId = table.Column<int>(type: "int", nullable: false),
                    BacSiKeId = table.Column<int>(type: "int", nullable: true),
                    NgayKe = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonKeThuoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonKeThuoc_BenhNhan_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonKeThuoc_NguoiDung_BacSiKeId",
                        column: x => x.BacSiKeId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonThuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonKeThuocId = table.Column<int>(type: "int", nullable: false),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    KetQuaKiemTra = table.Column<int>(type: "int", nullable: false),
                    LyDoCanhBao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonThuoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDonThuoc_DonKeThuoc_DonKeThuocId",
                        column: x => x.DonKeThuocId,
                        principalTable: "DonKeThuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonThuoc_Thuoc_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonThuoc_DonKeThuocId",
                table: "ChiTietDonThuoc",
                column: "DonKeThuocId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonThuoc_ThuocId",
                table: "ChiTietDonThuoc",
                column: "ThuocId");

            migrationBuilder.CreateIndex(
                name: "IX_DonKeThuoc_BacSiKeId",
                table: "DonKeThuoc",
                column: "BacSiKeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonKeThuoc_BenhNhanId",
                table: "DonKeThuoc",
                column: "BenhNhanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonThuoc");

            migrationBuilder.DropTable(
                name: "DonKeThuoc");
        }
    }
}
