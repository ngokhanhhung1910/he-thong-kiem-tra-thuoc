namespace MediCheck.Api.DTOs
{
    public class BenhNhanCreateDto
    {
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = "Nam";
        public string? CCCD { get; set; }
        public string? TiensuBenh { get; set; }
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
}
