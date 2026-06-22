using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("LieuLuongTheoNhomTuoi")]
    public class LieuLuongTheoNhomTuoi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThuocId { get; set; }
        public Thuoc Thuoc { get; set; } = null!;

        [Required]
        public int TuoiTu { get; set; }

        [Required]
        public int TuoiDen { get; set; }

        [Required]
        [MaxLength(300)]
        public string LieuLuongKhuyenNghi { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? GhiChu { get; set; }
    }
}
