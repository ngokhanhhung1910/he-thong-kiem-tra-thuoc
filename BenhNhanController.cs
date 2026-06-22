using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.DTOs;
using MediCheck.Api.Authorization;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/benhnhan")]
    public class BenhNhanController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BenhNhanController(AppDbContext context)
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

        private static BenhNhanResponseDto ToDto(BenhNhan b) => new BenhNhanResponseDto
        {
            Id = b.Id,
            MaBenhNhan = b.MaBenhNhan,
            HoTen = b.HoTen,
            NgaySinh = b.NgaySinh,
            Tuoi = TinhTuoi(b.NgaySinh),
            GioiTinh = b.GioiTinh.ToString(),
            CCCD = b.CCCD,
            TienSuBenh = b.TienSuBenh
        };

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.BenhNhans.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b => b.HoTen.Contains(search) || b.MaBenhNhan.Contains(search));

            var items = await query.OrderByDescending(b => b.Id).ToListAsync();
            return Ok(items.Select(ToDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var b = await _context.BenhNhans.FindAsync(id);
            if (b == null) return NotFound("Không tìm thấy bệnh nhân.");
            return Ok(ToDto(b));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BenhNhanCreateDto dto)
        {
            if (!Enum.TryParse<GioiTinh>(dto.GioiTinh, true, out var gioiTinh))
                gioiTinh = GioiTinh.Nam;

            var count = await _context.BenhNhans.CountAsync();
            var maBenhNhan = $"BN{(count + 1):D4}";

            var benhNhan = new BenhNhan
            {
                MaBenhNhan = maBenhNhan,
                HoTen = dto.HoTen,
                NgaySinh = dto.NgaySinh,
                GioiTinh = gioiTinh,
                CCCD = dto.CCCD,
                TienSuBenh = dto.TienSuBenh
            };

            _context.BenhNhans.Add(benhNhan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = benhNhan.Id }, ToDto(benhNhan));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BenhNhanUpdateDto dto)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null) return NotFound("Không tìm thấy bệnh nhân.");

            if (!Enum.TryParse<GioiTinh>(dto.GioiTinh, true, out var gioiTinh))
                gioiTinh = GioiTinh.Nam;

            benhNhan.HoTen = dto.HoTen;
            benhNhan.NgaySinh = dto.NgaySinh;
            benhNhan.GioiTinh = gioiTinh;
            benhNhan.CCCD = dto.CCCD;
            benhNhan.TienSuBenh = dto.TienSuBenh;

            await _context.SaveChangesAsync();
            return Ok(ToDto(benhNhan));
        }

        [HttpDelete("{id}")]
        [RequirePermission("BENHNHAN_QUANLY")]
        public async Task<IActionResult> Delete(int id)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null) return NotFound("Không tìm thấy bệnh nhân.");

            _context.BenhNhans.Remove(benhNhan);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xoá bệnh nhân." });
        }
    }
}
