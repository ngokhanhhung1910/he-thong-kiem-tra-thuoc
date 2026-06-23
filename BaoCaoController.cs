using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.DTOs;
using MediCheck.Api.Authorization;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/baocao")]
    [Authorize]
    public class BaoCaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BaoCaoController(AppDbContext context)
        {
            _context = context;
        }

        private static int TinhTuoi(DateTime ngaySinh)
        {
            var today = DateTime.Today;
            int tuoi = today.Year - ngaySinh.Year;
            if (ngaySinh.Date > today.AddYears(-tuoi)) tuoi--;
            return tuoi;
        }

        private static string PhanNhomTuoi(int tuoi)
        {
            if (tuoi <= 5) return "0 - 5 tuổi";
            if (tuoi <= 12) return "6 - 12 tuổi";
            return "Trên 12 tuổi";
        }

        // GET: api/baocao/benhnhan-theo-nhom-tuoi
        [HttpGet("benhnhan-theo-nhom-tuoi")]
        [RequirePermission("BAOCAO_XEM")]
        public async Task<IActionResult> GetBenhNhanTheoNhomTuoi()
        {
            var benhNhans = await _context.BenhNhans
                .Select(b => b.NgaySinh)
                .ToListAsync();

            var nhomCounts = new Dictionary<string, int>
            {
                ["0 - 5 tuổi"] = 0,
                ["6 - 12 tuổi"] = 0,
                ["Trên 12 tuổi"] = 0
            };

            foreach (var ngaySinh in benhNhans)
            {
                var nhom = PhanNhomTuoi(TinhTuoi(ngaySinh));
                nhomCounts[nhom]++;
            }

            var tongSo = benhNhans.Count;
            var nhomTuoi = nhomCounts.Select(kv => new NhomTuoiBenhNhanDto
            {
                NhomTuoi = kv.Key,
                SoLuong = kv.Value,
                TyLe = tongSo > 0 ? Math.Round(kv.Value * 100.0 / tongSo, 1) : 0
            }).ToList();

            return Ok(new BenhNhanTheoNhomTuoiResponseDto
            {
                TongSoBenhNhan = tongSo,
                NhomTuoi = nhomTuoi
            });
        }

        // GET: api/baocao/top-thuoc-canh-bao?top=3
        [HttpGet("top-thuoc-canh-bao")]
        [RequirePermission("BAOCAO_XEM")]
        public async Task<IActionResult> GetTopThuocCanhBao([FromQuery] int top = 3)
        {
            if (top < 1) top = 1;
            if (top > 10) top = 10;

            var grouped = await _context.CanhBaos
                .GroupBy(c => c.ThuocId)
                .Select(g => new { ThuocId = g.Key, SoLanCanhBao = g.Count() })
                .OrderByDescending(x => x.SoLanCanhBao)
                .Take(top)
                .ToListAsync();

            var thuocIds = grouped.Select(g => g.ThuocId).ToList();
            var thuocs = await _context.Thuocs
                .Where(t => thuocIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id);

            var danhSach = grouped.Select((g, index) =>
            {
                thuocs.TryGetValue(g.ThuocId, out var thuoc);
                return new TopThuocCanhBaoDto
                {
                    Hang = index + 1,
                    ThuocId = g.ThuocId,
                    TenThuoc = thuoc?.TenThuoc ?? "Không rõ",
                    HoatChat = thuoc?.HoatChatChinh ?? "—",
                    SoLanCanhBao = g.SoLanCanhBao,
                    MucDoDoTuoi = thuoc?.GhiChuChongChiDinh
                        ?? $"Từ {thuoc?.TuoiApDungTu} - {thuoc?.TuoiApDungDen} tuổi"
                };
            }).ToList();

            return Ok(new TopThuocCanhBaoResponseDto { DanhSach = danhSach });
        }
    }
}
