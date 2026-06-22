using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.DTOs;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/thuoc")]
    public class KiemTraController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KiemTraController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckTuoi([FromQuery] int medicine_id, [FromQuery] int age)
        {
            var thuoc = await _context.Thuocs.FindAsync(medicine_id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            var gioiHan = await _context.GioiHanTuoiThuocs
                .Where(g => g.ThuocId == medicine_id && age >= g.TuoiTu && age <= g.TuoiDen)
                .OrderByDescending(g => g.MucDo)
                .FirstOrDefaultAsync();

            bool anToan;
            string mucDo;
            string? lyDo;

            if (gioiHan != null)
            {
                anToan = gioiHan.MucDo == Models.MucDoCanhBao.AnToan;
                mucDo = gioiHan.MucDo.ToString();
                lyDo = gioiHan.LyDo;
            }
            else if (age >= thuoc.TuoiApDungTu && age <= thuoc.TuoiApDungDen)
            {
                anToan = true;
                mucDo = "AnToan";
                lyDo = null;
            }
            else
            {
                anToan = false;
                mucDo = "NguyHiem";
                lyDo = thuoc.GhiChuChongChiDinh ?? $"Thuốc {thuoc.TenThuoc} chống chỉ định cho độ tuổi này. Khuyến nghị từ {thuoc.TuoiApDungTu} đến {thuoc.TuoiApDungDen} tuổi.";
            }

            var thuocThayThe = await _context.Thuocs
                .Where(t => t.Id != medicine_id
                    && t.DangSuDung
                    && age >= t.TuoiApDungTu
                    && age <= t.TuoiApDungDen
                    && t.HoatChatChinh == thuoc.HoatChatChinh)
                .Take(5)
                .Select(t => new ThuocPhuHopDto
                {
                    TenThuoc = t.TenThuoc,
                    HoatChat = t.HoatChatChinh,
                    HamLuong = t.HamLuong,
                    DoTuoiPhuHop = $"Từ {t.TuoiApDungTu} - {t.TuoiApDungDen} tuổi"
                })
                .ToListAsync();

            var result = new KiemTraTuoiResponseDto
            {
                AnToan = anToan,
                MucDo = mucDo,
                LyDo = lyDo,
                TenThuoc = thuoc.TenThuoc,
                HoatChat = thuoc.HoatChatChinh,
                HamLuong = thuoc.HamLuong,
                DoTuoiPhuHop = $"Từ {thuoc.TuoiApDungTu} - {thuoc.TuoiApDungDen} tuổi",
                ThuocThayThe = thuocThayThe
            };

            return Ok(result);
        }
    }
}
