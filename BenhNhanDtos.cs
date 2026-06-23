namespace MediCheck.Api.DTOs
{
    public class BenhNhanCreateDto
    {
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = "Nam";
        public string? CCCD { get; set; }
        public string? TienSuBenh { get; set; }
    }

    public class BenhNhanUpdateDto : BenhNhanCreateDto
    {
    }

    public class BenhNhanResponseDto
    {
        public int Id { get; set; }
        public string MaBenhNhan { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public int Tuoi { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string? CCCD { get; set; }
        public string? TienSuBenh { get; set; }
    }

    public class DonThuocResponseDto
    {
        public int Id { get; set; }
        public DateTime NgayKe { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string? TenBacSi { get; set; }
        public string KetQuaKiemTra { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }
}