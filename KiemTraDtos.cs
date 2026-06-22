namespace MediCheck.Api.DTOs
{
    public class ThuocPhuHopDto
    {
        public string TenThuoc { get; set; } = string.Empty;
        public string HoatChat { get; set; } = string.Empty;
        public string HamLuong { get; set; } = string.Empty;
        public string DoTuoiPhuHop { get; set; } = string.Empty;
    }

    public class KiemTraTuoiResponseDto
    {
        public bool AnToan { get; set; }
        public string MucDo { get; set; } = string.Empty;
        public string? LyDo { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string HoatChat { get; set; } = string.Empty;
        public string HamLuong { get; set; } = string.Empty;
        public string DoTuoiPhuHop { get; set; } = string.Empty;
        public List<ThuocPhuHopDto> ThuocThayThe { get; set; } = new();
    }
}
