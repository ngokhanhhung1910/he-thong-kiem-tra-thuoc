namespace MediCheck.Api.DTOs
{
    public class NhomTuoiBenhNhanDto
    {
        public string NhomTuoi { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public double TyLe { get; set; }
    }

    public class BenhNhanTheoNhomTuoiResponseDto
    {
        public int TongSoBenhNhan { get; set; }
        public List<NhomTuoiBenhNhanDto> NhomTuoi { get; set; } = new();
    }

    public class TopThuocCanhBaoDto
    {
        public int Hang { get; set; }
        public int ThuocId { get; set; }
        public string TenThuoc { get; set; } = string.Empty;
        public string HoatChat { get; set; } = string.Empty;
        public int SoLanCanhBao { get; set; }
        public string MucDoDoTuoi { get; set; } = string.Empty;
    }

    public class TopThuocCanhBaoResponseDto
    {
        public List<TopThuocCanhBaoDto> DanhSach { get; set; } = new();
    }
}
