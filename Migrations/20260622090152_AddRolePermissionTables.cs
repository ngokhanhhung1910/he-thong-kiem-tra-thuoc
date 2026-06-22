using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quyen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaQuyen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenQuyen = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quyen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaThuoc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TenThuoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HoatChatChinh = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    HamLuong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DangBaoChe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TuoiApDungTu = table.Column<int>(type: "int", nullable: false),
                    TuoiApDungDen = table.Column<int>(type: "int", nullable: false),
                    LieuLuongKhuyenNghi = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GhiChuChongChiDinh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DangSuDung = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thuoc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VaiTroDanhSach",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenVaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTroDanhSach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VaiTroQuyen",
                columns: table => new
                {
                    VaiTroId = table.Column<int>(type: "int", nullable: false),
                    QuyenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaiTroQuyen", x => new { x.VaiTroId, x.QuyenId });
                    table.ForeignKey(
                        name: "FK_VaiTroQuyen_Quyen_QuyenId",
                        column: x => x.QuyenId,
                        principalTable: "Quyen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VaiTroQuyen_VaiTroDanhSach_VaiTroId",
                        column: x => x.VaiTroId,
                        principalTable: "VaiTroDanhSach",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Quyen",
                columns: new[] { "Id", "MaQuyen", "Module", "TenQuyen" },
                values: new object[,]
                {
                    { 1, "THUOC_XEM", "DanhMucThuoc", "Xem danh mục thuốc" },
                    { 2, "THUOC_QUANLY", "DanhMucThuoc", "Thêm/Sửa/Xoá thuốc" },
                    { 3, "KIEMTRA_THUOC", "KiemTra", "Kiểm tra thuốc theo độ tuổi" },
                    { 4, "BENHNHAN_XEM", "BenhNhan", "Xem hồ sơ bệnh nhân" },
                    { 5, "BENHNHAN_QUANLY", "BenhNhan", "Thêm/Sửa hồ sơ bệnh nhân" },
                    { 6, "TAIKHOAN_QUANLY", "TaiKhoan", "Quản lý tài khoản người dùng" },
                    { 7, "BAOCAO_XEM", "BaoCao", "Xem báo cáo & thống kê" }
                });

            migrationBuilder.InsertData(
                table: "VaiTroDanhSach",
                columns: new[] { "Id", "MoTa", "TenVaiTro" },
                values: new object[,]
                {
                    { 1, "Quản trị viên hệ thống", "Admin" },
                    { 2, "Bác sĩ", "BacSi" },
                    { 3, "Dược sĩ", "DuocSi" }
                });

            migrationBuilder.InsertData(
                table: "VaiTroQuyen",
                columns: new[] { "QuyenId", "VaiTroId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 },
                    { 1, 2 },
                    { 3, 2 },
                    { 4, 2 },
                    { 5, 2 },
                    { 7, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 },
                    { 4, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Thuoc_MaThuoc",
                table: "Thuoc",
                column: "MaThuoc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaiTroQuyen_QuyenId",
                table: "VaiTroQuyen",
                column: "QuyenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Thuoc");

            migrationBuilder.DropTable(
                name: "VaiTroQuyen");

            migrationBuilder.DropTable(
                name: "Quyen");

            migrationBuilder.DropTable(
                name: "VaiTroDanhSach");
        }
    }
}
