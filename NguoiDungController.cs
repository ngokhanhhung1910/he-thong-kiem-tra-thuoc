using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.DTOs;
using MediCheck.Api.Authorization;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/nguoidung")]
    public class NguoiDungController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NguoiDungController(AppDbContext context)
        {
            _context = context;
        }

        private static NguoiDungResponseDto ToDto(NguoiDung u) => new NguoiDungResponseDto
        {
            Id = u.Id,
            HoTen = u.HoTen,
            Email = u.Email,
            VaiTro = u.VaiTro.ToString(),
            TrangThai = u.TrangThai.ToString()
        };

        [HttpGet]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var query = _context.NguoiDungs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u => u.HoTen.Contains(search) || u.Email.Contains(search));

            var list = await query.OrderBy(u => u.Id).ToListAsync();
            return Ok(list.Select(ToDto));
        }

        [HttpPost]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> Create([FromBody] NguoiDungCreateDto dto)
        {
            if (await _context.NguoiDungs.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Email đã được sử dụng.");

            if (!Enum.TryParse<VaiTro>(dto.VaiTro, true, out var vaiTro))
                vaiTro = VaiTro.DuocSi;

            if (!Enum.TryParse<TrangThaiTaiKhoan>(dto.TrangThai, true, out var trangThai))
                trangThai = TrangThaiTaiKhoan.HoatDong;

            var user = new NguoiDung
            {
                HoTen = dto.HoTen,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                VaiTro = vaiTro,
                TrangThai = trangThai
            };

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            return Ok(ToDto(user));
        }

        [HttpPut("{id}")]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> Update(int id, [FromBody] NguoiDungUpdateDto dto)
        {
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return NotFound("Không tìm thấy tài khoản.");

            if (!Enum.TryParse<VaiTro>(dto.VaiTro, true, out var vaiTro))
                vaiTro = VaiTro.DuocSi;

            if (!Enum.TryParse<TrangThaiTaiKhoan>(dto.TrangThai, true, out var trangThai))
                trangThai = TrangThaiTaiKhoan.HoatDong;

            user.HoTen = dto.HoTen;
            user.VaiTro = vaiTro;
            user.TrangThai = trangThai;

            await _context.SaveChangesAsync();
            return Ok(ToDto(user));
        }

        [HttpDelete("{id}")]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return NotFound("Không tìm thấy tài khoản.");

            _context.NguoiDungs.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xoá tài khoản." });
        }
    }
}
