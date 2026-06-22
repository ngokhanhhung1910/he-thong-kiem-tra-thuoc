using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddGioiHanTuoiThuocTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NhomThuocId",
                table: "Thuoc",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GioiHanTuoiThuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    TuoiTu = table.Column<int>(type: "int", nullable: false),
                    TuoiDen = table.Column<int>(type: "int", nullable: false),
                    MucDo = table.Column<int>(type: "int", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioiHanTuoiThuoc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GioiHanTuoiThuoc_Thuoc_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NhomThuoc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhomThuoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhomThuoc", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NhomThuoc",
                columns: new[] { "Id", "MoTa", "TenNhomThuoc" },
                values: new object[,]
                {
                    { 1, null, "Giảm đau - Hạ sốt" },
                    { 2, null, "Kháng sinh" },
                    { 3, null, "Kháng viêm" },
                    { 4, null, "Dị ứng - Kháng Histamin" },
                    { 5, null, "Tiêu hoá" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Thuoc_NhomThuocId",
                table: "Thuoc",
                column: "NhomThuocId");

            migrationBuilder.CreateIndex(
                name: "IX_GioiHanTuoiThuoc_ThuocId",
                table: "GioiHanTuoiThuoc",
                column: "ThuocId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thuoc_NhomThuoc_NhomThuocId",
                table: "Thuoc",
                column: "NhomThuocId",
                principalTable: "NhomThuoc",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thuoc_NhomThuoc_NhomThuocId",
                table: "Thuoc");

            migrationBuilder.DropTable(
                name: "GioiHanTuoiThuoc");

            migrationBuilder.DropTable(
                name: "NhomThuoc");

            migrationBuilder.DropIndex(
                name: "IX_Thuoc_NhomThuocId",
                table: "Thuoc");

            migrationBuilder.DropColumn(
                name: "NhomThuocId",
                table: "Thuoc");
        }
    }
}
