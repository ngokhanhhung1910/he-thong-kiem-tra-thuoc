using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediCheck.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLieuLuongTheoNhomTuoiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LieuLuongTheoNhomTuoi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThuocId = table.Column<int>(type: "int", nullable: false),
                    TuoiTu = table.Column<int>(type: "int", nullable: false),
                    TuoiDen = table.Column<int>(type: "int", nullable: false),
                    LieuLuongKhuyenNghi = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LieuLuongTheoNhomTuoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LieuLuongTheoNhomTuoi_Thuoc_ThuocId",
                        column: x => x.ThuocId,
                        principalTable: "Thuoc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LieuLuongTheoNhomTuoi_ThuocId",
                table: "LieuLuongTheoNhomTuoi",
                column: "ThuocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LieuLuongTheoNhomTuoi");
        }
    }
}
