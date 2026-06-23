namespace MediCheck.Api.DTOs
{
    public class DonKeThuocCreateDto
    {
        public int BenhNhanId { get; set; }
        public int? BacSiKeId { get; set; }
        public List<int> ThuocIds { get; set; } = new();
    }

    public class ChiTietDonThuocResponseDto
    {
        public int ThuocId { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string KetQuaKiemTra { get; set; } = string.Empty;
        public string? LyDoCanhBao { get; set; }
    }

    public class DonKeThuocResponseDto
    {
        public int Id { get; set; }
        public string MaDon { get; set; } = string.Empty;
        public DateTime NgayKe { get; set; }
        public int BenhNhanId { get; set; }
        public string TenBenhNhan { get; set; } = string.Empty;
        public int TuoiBenhNhan { get; set; }
        public List<ChiTietDonThuocResponseDto> ChiTiets { get; set; } = new();
    }
}
