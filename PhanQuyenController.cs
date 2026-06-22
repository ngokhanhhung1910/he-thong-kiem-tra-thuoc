using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.Authorization;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/phan-quyen")]
    [Authorize]
    public class PhanQuyenController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhanQuyenController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("matrix")]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> GetMatrix()
        {
            var vaiTros = await _context.VaiTroDanhSach.OrderBy(v => v.Id).ToListAsync();
            var quyens = await _context.Quyens.OrderBy(q => q.Id).ToListAsync();
            var ganQuyen = await _context.VaiTroQuyens.ToListAsync();

            var result = new
            {
                vaiTros = vaiTros.Select(v => new { v.Id, v.TenVaiTro, v.MoTa }),
                quyens = quyens.Select(q => new { q.Id, q.MaQuyen, q.TenQuyen, q.Module }),
                ganQuyen = ganQuyen.Select(g => new { g.VaiTroId, g.QuyenId })
            };

            return Ok(result);
        }

        public class CapNhatQuyenDto
        {
            public int VaiTroId { get; set; }
            public int QuyenId { get; set; }
            public bool ChoPhep { get; set; } 
        }

        [HttpPut("cap-nhat")]
        [RequirePermission("TAIKHOAN_QUANLY")]
        public async Task<IActionResult> CapNhatQuyen([FromBody] CapNhatQuyenDto dto)
        {
            var existed = await _context.VaiTroQuyens
                .FirstOrDefaultAsync(vq => vq.VaiTroId == dto.VaiTroId && vq.QuyenId == dto.QuyenId);

            if (dto.ChoPhep && existed == null)
            {
                _context.VaiTroQuyens.Add(new VaiTroQuyen { VaiTroId = dto.VaiTroId, QuyenId = dto.QuyenId });
            }
            else if (!dto.ChoPhep && existed != null)
            {
                _context.VaiTroQuyens.Remove(existed);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã cập nhật quyền." });
        }
    }
}
