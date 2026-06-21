using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCheck.Api.Models
{
    public enum VaiTro
    {
        Admin,
        BacSi,
        DuocSi
    }

    public enum TrangThaiTaiKhoan
    {
        HoatDong,
        TamKhoa
    }

    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public VaiTro VaiTro { get; set; } = VaiTro.DuocSi;

        [Required]
        public TrangThaiTaiKhoan TrangThai { get; set; } = TrangThaiTaiKhoan.HoatDong;

        public string? MaXacThucResetPassword { get; set; }

        public DateTime? ResetPasswordExpiry { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
