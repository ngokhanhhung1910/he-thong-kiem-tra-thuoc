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
}
