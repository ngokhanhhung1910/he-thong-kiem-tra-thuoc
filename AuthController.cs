using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.DTOs;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email và mật khẩu là bắt buộc.");

            bool emailExists = await _context.NguoiDungs.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
                return BadRequest("Email đã được sử dụng.");

            if (!Enum.TryParse<VaiTro>(dto.VaiTro, true, out var vaiTro))
                vaiTro = VaiTro.DuocSi;

            var user = new NguoiDung
            {
                HoTen = dto.HoTen,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                VaiTro = vaiTro,
                TrangThai = TrangThaiTaiKhoan.HoatDong
            };

            _context.NguoiDungs.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Email hoặc mật khẩu không đúng.");

            if (user.TrangThai == TrangThaiTaiKhoan.TamKhoa)
                return Unauthorized("Tài khoản đang bị tạm khoá.");

            var token = GenerateJwtToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                HoTen = user.HoTen,
                Email = user.Email,
                VaiTro = user.VaiTro.ToString()
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return NotFound("Email không tồn tại trong hệ thống.");

            // Sinh mã xác thực 6 số, hết hạn sau 15 phút
            var code = new Random().Next(100000, 999999).ToString();
            user.MaXacThucResetPassword = code;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mã xác thực đã được gửi.", code });
        }

        private string GenerateJwtToken(NguoiDung user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Role, user.VaiTro.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["ExpiresMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
