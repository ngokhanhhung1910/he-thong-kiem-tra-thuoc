using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    [Table("NhomThuoc")]
    public class NhomThuoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TenNhomThuoc { get; set; } = string.Empty; 

        [MaxLength(200)]
        public string? MoTa { get; set; }
    }
}
