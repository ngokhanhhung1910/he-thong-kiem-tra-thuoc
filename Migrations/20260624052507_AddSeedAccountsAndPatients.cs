using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedAccountsAndPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CanhBao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    BenhNhanId = table.Column<int>(type: "int", nullable: true),
                    TuoiKiemTra = table.Column<int>(type: "int", nullable: false),
                    MucDo = table.Column<int>(type: "int", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanhBao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanhBao_BenhNhan_BenhNhanId",
                        column: x => x.BenhNhanId,
                        principalTable: "BenhNhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CanhBao_Thuoc_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BenhNhan",
                columns: new[] { "Id", "CCCD", "GioiTinh", "HoTen", "MaBenhNhan", "NgaySinh", "NgayTao", "TienSuBenh" },
                values: new object[,]
                {
                    { 1, "079200000001", 0, "Nguyễn Văn A", "BN0001", new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4672), "Dị ứng với thành phần Aspirin" },
                    { 2, "079200000002", 0, "Trần Gia Bảo", "BN0002", new DateTime(2011, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4680), null },
                    { 3, "079200000003", 1, "Lê Thị Mai", "BN0003", new DateTime(1990, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4680), "Tiền sử viêm gan" }
                });

            migrationBuilder.InsertData(
                table: "NguoiDung",
                columns: new[] { "Id", "Email", "HoTen", "MaXacThucResetPassword", "NgayTao", "PasswordHash", "ResetPasswordExpiry", "TrangThai", "VaiTro" },
                values: new object[,]
                {
                    { 1, "admin@medicheck.com", "Quản trị viên", null, new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4586), "$2b$11$SbZ5gOYejRWZvBm8eKlrOeJy7S/atGER.EoUob7x8uC7flq1AWsGa", null, 0, 0 },
                    { 2, "bacsi@medicheck.com", "Bác sĩ Nguyễn An", null, new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4653), "$2b$11$.X61ltPWJLaNMyprtN9YZ.FNDkMOvU6OfKkmw3I27wEEcxVxC22Tu", null, 0, 1 },
                    { 3, "duocsi@medicheck.com", "Dược sĩ Trần Bình", null, new DateTime(2026, 6, 24, 12, 25, 6, 454, DateTimeKind.Local).AddTicks(4655), "$2b$11$TP1r5FPMtTIsFNNwEOoTau3bm.LfB3ErZ.50zNKw.p3gxPo30iaFq", null, 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CanhBao_BenhNhanId",
                table: "CanhBao",
                column: "BenhNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_CanhBao_ThuocId",
                table: "CanhBao",
                column: "ThuocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CanhBao");

            migrationBuilder.DeleteData(
                table: "BenhNhan",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BenhNhan",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BenhNhan",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "NguoiDung",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
