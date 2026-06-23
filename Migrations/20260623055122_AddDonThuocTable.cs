using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDonThuocTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenhNhan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBenhNhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TienSuBenh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonThuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BenhNhanId = table.Column<int>(type: "int", nullable: false),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    BacSiKeId = table.Column<int>(type: "int", nullable: true),
                    NgayKe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KetQuaKiemTra = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonThuoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonThuoc_BenhNhan_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DonThuoc_NguoiDung_BacSiKeId",
                        column: x => x.BacSiKeId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonThuoc_Thuoc_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenhNhan_MaBenhNhan",
                table: "BenhNhan",
                column: "MaBenhNhan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonThuoc_BacSiKeId",
                table: "DonThuoc",
                column: "BacSiKeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuoc_BenhNhanId",
                table: "DonThuoc",
                column: "BenhNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_DonThuoc_ThuocId",
                table: "DonThuoc",
                column: "ThuocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonThuoc");

            migrationBuilder.DropTable(
                name: "BenhNhan");
        }
    }
}
