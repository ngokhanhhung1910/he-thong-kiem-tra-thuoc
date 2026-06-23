using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Data;
using MediCheck.Api.Models;
using MediCheck.Api.DTOs;

namespace MediCheck.Api.Controllers
{
    [ApiController]
    [Route("api/donkethuoc")]
    public class DonKeThuocController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DonKeThuocController(AppDbContext context)
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

        private async Task<(KetQuaKiemTra, string?)> KiemTraTuoi(Thuoc thuoc, int tuoi)
        {
            var gioiHan = await _context.GioiHanTuoiThuocs
                .Where(g => g.ThuocId == thuoc.Id && tuoi >= g.TuoiTu && tuoi <= g.TuoiDen)
                .OrderByDescending(g => g.MucDo)
                .FirstOrDefaultAsync();

            if (gioiHan != null)
            {
                var ketQua = gioiHan.MucDo switch
                {
                    MucDoCanhBao.AnToan => KetQuaKiemTra.AnToan,
                    MucDoCanhBao.ThanTrong => KetQuaKiemTra.CanhBao,
                    _ => KetQuaKiemTra.NguyHiem
                };
                return (ketQua, gioiHan.LyDo);
            }

            if (tuoi >= thuoc.TuoiApDungTu && tuoi <= thuoc.TuoiApDungDen)
                return (KetQuaKiemTra.AnToan, null);

            var lyDo = thuoc.GhiChuChongChiDinh
                ?? $"Thuốc {thuoc.TenThuoc} chống chỉ định cho độ tuổi này. Khuyến nghị từ {thuoc.TuoiApDungTu} đến {thuoc.TuoiApDungDen} tuổi.";
            return (KetQuaKiemTra.NguyHiem, lyDo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DonKeThuocCreateDto dto)
        {
            var benhNhan = await _context.BenhNhans.FindAsync(dto.BenhNhanId);
            if (benhNhan == null) return NotFound("Không tìm thấy bệnh nhân.");

            if (dto.ThuocIds == null || dto.ThuocIds.Count == 0)
                return BadRequest("Đơn thuốc phải có ít nhất 1 loại thuốc.");

            var tuoi = TinhTuoi(benhNhan.NgaySinh);
            var count = await _context.DonKeThuocs.CountAsync();

            var donKeThuoc = new DonKeThuoc
            {
                MaDon = $"DKT{(count + 1):D5}",
                BenhNhanId = dto.BenhNhanId,
                BacSiKeId = dto.BacSiKeId,
                NgayKe = DateTime.Now
            };

            foreach (var thuocId in dto.ThuocIds)
            {
                var thuoc = await _context.Thuocs.FindAsync(thuocId);
                if (thuoc == null) continue;

                var (ketQua, lyDo) = await KiemTraTuoi(thuoc, tuoi);

                donKeThuoc.ChiTiets.Add(new ChiTietDonThuoc
                {
                    ThuocId = thuocId,
                    KetQuaKiemTra = ketQua,
                    LyDoCanhBao = lyDo
                });
            }

            _context.DonKeThuocs.Add(donKeThuoc);
            await _context.SaveChangesAsync();

            return Ok(await BuildResponse(donKeThuoc.Id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await BuildResponse(id);
            if (result == null) return NotFound("Không tìm thấy đơn thuốc.");
            return Ok(result);
        }

        private async Task<DonKeThuocResponseDto?> BuildResponse(int id)
        {
            var don = await _context.DonKeThuocs
                .Include(d => d.BenhNhan)
                .Include(d => d.ChiTiets).ThenInclude(c => c.Thuoc)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (don == null) return null;

            return new DonKeThuocResponseDto
            {
                Id = don.Id,
                MaDon = don.MaDon,
                NgayKe = don.NgayKe,
                BenhNhanId = don.BenhNhanId,
                TenBenhNhan = don.BenhNhan.HoTen,
                TuoiBenhNhan = TinhTuoi(don.BenhNhan.NgaySinh),
                ChiTiets = don.ChiTiets.Select(c => new ChiTietDonThuocResponseDto
                {
                    ThuocId = c.ThuocId,
                    TenThuoc = c.Thuoc.TenThuoc,
                    KetQuaKiemTra = c.KetQuaKiemTra.ToString(),
                    LyDoCanhBao = c.LyDoCanhBao
                }).ToList()
            };
        }
    }
}
