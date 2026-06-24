namespace MediCheck.Api.DTOs
{
    public class NguoiDungResponseDto
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
    }

    public class NguoiDungCreateDto
    {
        public string HoTen { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VaiTro { get; set; } = "DuocSi";
        public string TrangThai { get; set; } = "HoatDong";
    }

    public class NguoiDungUpdateDto
    {
        public string HoTen { get; set; } = string.Empty;
        public string VaiTro { get; set; } = "DuocSi";
        public string TrangThai { get; set; } = "HoatDong";
    }
}
