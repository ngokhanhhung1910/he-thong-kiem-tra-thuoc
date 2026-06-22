using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.DTOs;
using MediCheck.Api.Authorization;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/thuoc")]
    public class ThuocController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThuocController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/thuoc?search=&category=&dangBaoChe=&trangThai=&page=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int? category, // NhomThuocId
            [FromQuery] string? dangBaoChe,
            [FromQuery] string? trangThai,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var query = _context.Thuocs.Include(t => t.NhomThuoc).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(t => t.TenThuoc.Contains(search) || t.HoatChatChinh.Contains(search));

            if (category.HasValue)
                query = query.Where(t => t.NhomThuocId == category.Value);

            if (!string.IsNullOrWhiteSpace(dangBaoChe) && dangBaoChe != "Tất cả")
                query = query.Where(t => t.DangBaoChe == dangBaoChe);

            if (trangThai == "DangDung")
                query = query.Where(t => t.DangSuDung);
            else if (trangThai == "NgungDung")
                query = query.Where(t => !t.DangSuDung);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ThuocResponseDto
                {
                    Id = t.Id,
                    MaThuoc = t.MaThuoc,
                    TenThuoc = t.TenThuoc,
                    HoatChatChinh = t.HoatChatChinh,
                    HamLuong = t.HamLuong,
                    DangBaoChe = t.DangBaoChe,
                    TuoiApDungTu = t.TuoiApDungTu,
                    TuoiApDungDen = t.TuoiApDungDen,
                    LieuLuongKhuyenNghi = t.LieuLuongKhuyenNghi,
                    GhiChuChongChiDinh = t.GhiChuChongChiDinh,
                    DangSuDung = t.DangSuDung,
                    NhomThuocId = t.NhomThuocId,
                    TenNhomThuoc = t.NhomThuoc != null ? t.NhomThuoc.TenNhomThuoc : null
                })
                .ToListAsync();

            return Ok(new { total, page, pageSize, items });
        }

        // GET: api/thuoc/nhom-thuoc  (dùng cho dropdown lọc nhóm thuốc)
        [HttpGet("nhom-thuoc")]
        public async Task<IActionResult> GetNhomThuoc()
        {
            var nhomThuocs = await _context.NhomThuocs.OrderBy(n => n.TenNhomThuoc).ToListAsync();
            return Ok(nhomThuocs);
        }

        // GET: api/thuoc/stats  (4 ô thống kê ở Image 4)
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new ThuocStatsDto
            {
                TongSoThuoc = await _context.Thuocs.CountAsync(),
                DangSuDung = await _context.Thuocs.CountAsync(t => t.DangSuDung),
                NgungSuDung = await _context.Thuocs.CountAsync(t => !t.DangSuDung),
                SoNhomTuoi = await _context.Thuocs
                    .Select(t => new { t.TuoiApDungTu, t.TuoiApDungDen })
                    .Distinct()
                    .CountAsync()
            };
            return Ok(stats);
        }

        // GET: api/thuoc/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");
            return Ok(thuoc);
        }

        // POST: api/thuoc
        [HttpPost]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> Create([FromBody] ThuocCreateDto dto)
        {
            if (dto.TuoiApDungTu > dto.TuoiApDungDen)
                return BadRequest("Tuổi 'Từ' không thể lớn hơn tuổi 'Đến'.");

            var lastId = await _context.Thuocs.CountAsync();
            var maThuoc = $"TH-{(lastId + 1):D4}";

            var thuoc = new Thuoc
            {
                MaThuoc = maThuoc,
                TenThuoc = dto.TenThuoc,
                HoatChatChinh = dto.HoatChatChinh,
                HamLuong = dto.HamLuong,
                DangBaoChe = dto.DangBaoChe,
                TuoiApDungTu = dto.TuoiApDungTu,
                TuoiApDungDen = dto.TuoiApDungDen,
                LieuLuongKhuyenNghi = dto.LieuLuongKhuyenNghi,
                GhiChuChongChiDinh = dto.GhiChuChongChiDinh,
                NhomThuocId = dto.NhomThuocId,
                DangSuDung = true
            };

            _context.Thuocs.Add(thuoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = thuoc.Id }, thuoc);
        }

        // PUT: api/thuoc/5
        [HttpPut("{id}")]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> Update(int id, [FromBody] ThuocUpdateDto dto)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            if (dto.TuoiApDungTu > dto.TuoiApDungDen)
                return BadRequest("Tuổi 'Từ' không thể lớn hơn tuổi 'Đến'.");

            thuoc.TenThuoc = dto.TenThuoc;
            thuoc.HoatChatChinh = dto.HoatChatChinh;
            thuoc.HamLuong = dto.HamLuong;
            thuoc.DangBaoChe = dto.DangBaoChe;
            thuoc.TuoiApDungTu = dto.TuoiApDungTu;
            thuoc.TuoiApDungDen = dto.TuoiApDungDen;
            thuoc.LieuLuongKhuyenNghi = dto.LieuLuongKhuyenNghi;
            thuoc.GhiChuChongChiDinh = dto.GhiChuChongChiDinh;
            thuoc.NhomThuocId = dto.NhomThuocId;
            thuoc.DangSuDung = dto.DangSuDung;

            await _context.SaveChangesAsync();
            return Ok(thuoc);
        }

        // PATCH: api/thuoc/5/toggle-status
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            thuoc.DangSuDung = !thuoc.DangSuDung;
            await _context.SaveChangesAsync();
            return Ok(thuoc);
        }

        // DELETE: api/thuoc/5
        [HttpDelete("{id}")]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> Delete(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            _context.Thuocs.Remove(thuoc);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xoá thuốc." });
        }

        [HttpGet("{id}/age-rules")]
        public async Task<IActionResult> GetAgeRules(int id)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            var rules = await _context.GioiHanTuoiThuocs
                .Where(g => g.ThuocId == id)
                .OrderBy(g => g.TuoiTu)
                .Select(g => new AgeRuleDto
                {
                    Id = g.Id,
                    TuoiTu = g.TuoiTu,
                    TuoiDen = g.TuoiDen,
                    MucDo = g.MucDo.ToString(),
                    LyDo = g.LyDo
                })
                .ToListAsync();

            return Ok(rules);
        }

        [HttpPost("{id}/age-rules")]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> CreateAgeRule(int id, [FromBody] AgeRuleUpsertDto dto)
        {
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound("Không tìm thấy thuốc.");

            if (!Enum.TryParse<MucDoCanhBao>(dto.MucDo, true, out var mucDo))
                return BadRequest("Mức độ không hợp lệ.");

            var rule = new GioiHanTuoiThuoc
            {
                ThuocId = id,
                TuoiTu = dto.TuoiTu,
                TuoiDen = dto.TuoiDen,
                MucDo = mucDo,
                LyDo = dto.LyDo
            };

            _context.GioiHanTuoiThuocs.Add(rule);
            await _context.SaveChangesAsync();
            return Ok(rule);
        }

        [HttpPut("{id}/age-rules/{ruleId}")]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> UpdateAgeRule(int id, int ruleId, [FromBody] AgeRuleUpsertDto dto)
        {
            var rule = await _context.GioiHanTuoiThuocs
                .FirstOrDefaultAsync(g => g.Id == ruleId && g.ThuocId == id);
            if (rule == null) return NotFound("Không tìm thấy quy tắc độ tuổi.");

            if (!Enum.TryParse<MucDoCanhBao>(dto.MucDo, true, out var mucDo))
                return BadRequest("Mức độ không hợp lệ.");

            rule.TuoiTu = dto.TuoiTu;
            rule.TuoiDen = dto.TuoiDen;
            rule.MucDo = mucDo;
            rule.LyDo = dto.LyDo;

            await _context.SaveChangesAsync();
            return Ok(rule);
        }

        [HttpDelete("{id}/age-rules/{ruleId}")]
        [RequirePermission("THUOC_QUANLY")]
        public async Task<IActionResult> DeleteAgeRule(int id, int ruleId)
        {
            var rule = await _context.GioiHanTuoiThuocs
                .FirstOrDefaultAsync(g => g.Id == ruleId && g.ThuocId == id);
            if (rule == null) return NotFound("Không tìm thấy quy tắc độ tuổi.");

            _context.GioiHanTuoiThuocs.Remove(rule);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xoá quy tắc." });
        }
    }
}
