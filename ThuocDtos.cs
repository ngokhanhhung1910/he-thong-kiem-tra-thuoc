namespace MediCheck.Api.DTOs
{
    public class ThuocCreateDto
    {
        public string TenThuoc { get; set; } = string.Empty;
        public string HoatChatChinh { get; set; } = string.Empty;
        public string HamLuong { get; set; } = string.Empty;
        public string DangBaoChe { get; set; } = string.Empty;
        public int TuoiApDungTu { get; set; }
        public int TuoiApDungDen { get; set; }
        public string LieuLuongKhuyenNghi { get; set; } = string.Empty;
        public string? GhiChuChongChiDinh { get; set; }
    }

    public class ThuocUpdateDto : ThuocCreateDto
    {
        public bool DangSuDung { get; set; } = true;
    }

    public class ThuocResponseDto
    {
        public int Id { get; set; }
        public string MaThuoc { get; set; } = string.Empty;
        public string TenThuoc { get; set; } = string.Empty;
        public string HoatChatChinh { get; set; } = string.Empty;
        public string HamLuong { get; set; } = string.Empty;
        public string DangBaoChe { get; set; } = string.Empty;
        public int TuoiApDungTu { get; set; }
        public int TuoiApDungDen { get; set; }
        public string LieuLuongKhuyenNghi { get; set; } = string.Empty;
        public string? GhiChuChongChiDinh { get; set; }
        public bool DangSuDung { get; set; }
    }

    public class ThuocStatsDto
    {
        public int TongSoThuoc { get; set; }
        public int DangSuDung { get; set; }
        public int SoNhomTuoi { get; set; }
        public int NgungSuDung { get; set; }
    }
}
