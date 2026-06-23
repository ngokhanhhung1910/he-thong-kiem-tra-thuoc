using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    public enum KetQuaKiemTra
    {
        AnToan,
        CanhBao,
        NguyHiem
    }

    [Table("DonThuoc")]
    public class DonThuoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BenhNhanId { get; set; }
        public BenhNhan BenhNhan { get; set; } = null!;

        [Required]
        public int ThuocId { get; set; }
        public Thuoc Thuoc { get; set; } = null!;

        public int? BacSiKeId { get; set; }
        public NguoiDung? BacSiKe { get; set; }

        [Required]
        public DateTime NgayKe { get; set; } = DateTime.Now;

        [Required]
        public KetQuaKiemTra KetQuaKiemTra { get; set; } = KetQuaKiemTra.AnToan;

        [MaxLength(300)]
        public string? GhiChu { get; set; }
    }
}
