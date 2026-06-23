using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("CanhBao")]
    public class CanhBao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThuocId { get; set; }
        public Thuoc Thuoc { get; set; } = null!;

        public int? BenhNhanId { get; set; }
        public BenhNhan? BenhNhan { get; set; }

        [Required]
        public int TuoiKiemTra { get; set; }

        [Required]
        public MucDoCanhBao MucDo { get; set; }

        [MaxLength(500)]
        public string? LyDo { get; set; }

        [Required]
        public DateTime ThoiGian { get; set; } = DateTime.Now;
    }
}
