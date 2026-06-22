using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("Thuoc")]
    public class Thuoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string MaThuoc { get; set; } = string.Empty; // VD: TH-0001

        [Required]
        [MaxLength(200)]
        public string TenThuoc { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string HoatChatChinh { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string HamLuong { get; set; } = string.Empty; // VD: "500mg"

        [Required]
        [MaxLength(50)]
        public string DangBaoChe { get; set; } = string.Empty; // VD: "Viên nén", "Siro"

        [Required]
        public int TuoiApDungTu { get; set; }

        [Required]
        public int TuoiApDungDen { get; set; }

        [Required]
        [MaxLength(300)]
        public string LieuLuongKhuyenNghi { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? GhiChuChongChiDinh { get; set; }

        public bool DangSuDung { get; set; } = true;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
