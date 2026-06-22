using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    public enum MucDoCanhBao
    {
        AnToan,
        ThanTrong,
        NguyHiem
    }

    [Table("GioiHanTuoiThuoc")]
    public class GioiHanTuoiThuoc
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
        public MucDoCanhBao MucDo { get; set; } = MucDoCanhBao.AnToan;

        [MaxLength(500)]
        public string? LyDo { get; set; }
    }
}
