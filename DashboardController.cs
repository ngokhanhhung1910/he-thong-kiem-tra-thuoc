using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var today = DateTime.Today;

            var tongSoThuoc = await _context.Thuocs.CountAsync();
            var tongBenhNhan = await _context.BenhNhans.CountAsync();
            var luotKhamHomNay = await _context.DonKeThuocs.CountAsync(d => d.NgayKe.Date == today);
            var soCanhBao = await _context.CanhBaos.CountAsync(c => c.ThoiGian.Date == today);

            return Ok(new
            {
                tongSoThuoc,
                tongBenhNhan,
                luotKhamHomNay,
                soCanhBao
            });
        }

        [HttpGet("canh-bao-gan-day")]
        public async Task<IActionResult> GetCanhBaoGanDay()
        {
            var list = await _context.CanhBaos
                .Include(c => c.Thuoc)
                .Include(c => c.BenhNhan)
                .OrderByDescending(c => c.ThoiGian)
                .Take(10)
                .Select(c => new
                {
                    thoiGian = c.ThoiGian,
                    tenBenhNhan = c.BenhNhan != null ? c.BenhNhan.HoTen : "Không rõ",
                    tenThuoc = c.Thuoc.TenThuoc,
                    mucDo = c.MucDo.ToString()
                })
                .ToListAsync();

            return Ok(list);
        }
    }
}
