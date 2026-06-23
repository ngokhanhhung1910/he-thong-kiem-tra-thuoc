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
    }
}
