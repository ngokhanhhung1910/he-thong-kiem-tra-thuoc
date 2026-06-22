using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("VaiTroDanhSach")]
    public class VaiTroEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TenVaiTro { get; set; } = string.Empty; 

        [MaxLength(200)]
        public string? MoTa { get; set; }

        public ICollection<VaiTroQuyen> VaiTroQuyens { get; set; } = new List<VaiTroQuyen>();
    }

    [Table("Quyen")]
    public class QuyenEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string MaQuyen { get; set; } = string.Empty; 

        [Required]
        [MaxLength(150)]
        public string TenQuyen { get; set; } = string.Empty; 

        [MaxLength(50)]
        public string Module { get; set; } = string.Empty; 

        public ICollection<VaiTroQuyen> VaiTroQuyens { get; set; } = new List<VaiTroQuyen>();
    }

    [Table("VaiTroQuyen")]
    public class VaiTroQuyen
    {
        public int VaiTroId { get; set; }
        public VaiTroEntity VaiTro { get; set; } = null!;

        public int QuyenId { get; set; }
        public QuyenEntity Quyen { get; set; } = null!;
    }
}
