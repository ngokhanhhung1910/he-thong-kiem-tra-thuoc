using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    public enum GioiTinh
    {
        Nam,
        Nu
    }

    [Table("BenhNhan")]
    public class BenhNhan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string MaBenhNhan { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        public DateTime NgaySinh { get; set; }

        [Required]
        public GioiTinh GioiTinh { get; set; }

        [MaxLength(20)]
        public string? CCCD { get; set; }

        [MaxLength(500)]
        public string? TienSuBenh { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
