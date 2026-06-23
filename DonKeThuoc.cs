using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("DonKeThuoc")]
    public class DonKeThuoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string MaDon { get; set; } = string.Empty;

        [Required]
        public int BenhNhanId { get; set; }
        public BenhNhan BenhNhan { get; set; } = null!;

        public int? BacSiKeId { get; set; }
        public NguoiDung? BacSiKe { get; set; }

        [Required]
        public DateTime NgayKe { get; set; } = DateTime.Now;

        public ICollection<ChiTietDonThuoc> ChiTiets { get; set; } = new List<ChiTietDonThuoc>();
    }

    [Table("ChiTietDonThuoc")]
    public class ChiTietDonThuoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DonKeThuocId { get; set; }
        public DonKeThuoc DonKeThuoc { get; set; } = null!;

        [Required]
        public int ThuocId { get; set; }
        public Thuoc Thuoc { get; set; } = null!;

        [Required]
        public KetQuaKiemTra KetQuaKiemTra { get; set; } = KetQuaKiemTra.AnToan;

        [MaxLength(500)]
        public string? LyDoCanhBao { get; set; }
    }
}
