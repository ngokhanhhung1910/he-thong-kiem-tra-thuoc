using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Models;

namespace MediCheck.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
        public DbSet<Thuoc> Thuocs => Set<Thuoc>();
        public DbSet<NhomThuoc> NhomThuocs => Set<NhomThuoc>();
        public DbSet<GioiHanTuoiThuoc> GioiHanTuoiThuocs => Set<GioiHanTuoiThuoc>();
        public DbSet<LieuLuongTheoNhomTuoi> LieuLuongTheoNhomTuois => Set<LieuLuongTheoNhomTuoi>();
        public DbSet<BenhNhan> BenhNhans => Set<BenhNhan>();
        public DbSet<DonThuoc> DonThuocs => Set<DonThuoc>();
        public DbSet<DonKeThuoc> DonKeThuocs => Set<DonKeThuoc>();
        public DbSet<ChiTietDonThuoc> ChiTietDonThuocs => Set<ChiTietDonThuoc>();
        public DbSet<CanhBao> CanhBaos => Set<CanhBao>();
        public DbSet<VaiTroEntity> VaiTroDanhSach => Set<VaiTroEntity>();
        public DbSet<QuyenEntity> Quyens => Set<QuyenEntity>();
        public DbSet<VaiTroQuyen> VaiTroQuyens => Set<VaiTroQuyen>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Thuoc>()
                .HasIndex(t => t.MaThuoc)
                .IsUnique();

            modelBuilder.Entity<GioiHanTuoiThuoc>()
                .HasOne(g => g.Thuoc)
                .WithMany()
                .HasForeignKey(g => g.ThuocId);

            modelBuilder.Entity<LieuLuongTheoNhomTuoi>()
                .HasOne(l => l.Thuoc)
                .WithMany()
                .HasForeignKey(l => l.ThuocId);

            modelBuilder.Entity<BenhNhan>()
                .HasIndex(b => b.MaBenhNhan)
                .IsUnique();

            modelBuilder.Entity<DonThuoc>()
                .HasOne(d => d.BenhNhan)
                .WithMany()
                .HasForeignKey(d => d.BenhNhanId);

            modelBuilder.Entity<DonThuoc>()
                .HasOne(d => d.Thuoc)
                .WithMany()
                .HasForeignKey(d => d.ThuocId);

            modelBuilder.Entity<DonThuoc>()
                .HasOne(d => d.BacSiKe)
                .WithMany()
                .HasForeignKey(d => d.BacSiKeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DonKeThuoc>()
                .HasOne(d => d.BenhNhan)
                .WithMany()
                .HasForeignKey(d => d.BenhNhanId);

            modelBuilder.Entity<DonKeThuoc>()
                .HasOne(d => d.BacSiKe)
                .WithMany()
                .HasForeignKey(d => d.BacSiKeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ChiTietDonThuoc>()
                .HasOne(c => c.DonKeThuoc)
                .WithMany(d => d.ChiTiets)
                .HasForeignKey(c => c.DonKeThuocId);

            modelBuilder.Entity<ChiTietDonThuoc>()
                .HasOne(c => c.Thuoc)
                .WithMany()
                .HasForeignKey(c => c.ThuocId);

            modelBuilder.Entity<CanhBao>()
                .HasOne(c => c.Thuoc)
                .WithMany()
                .HasForeignKey(c => c.ThuocId);

            modelBuilder.Entity<CanhBao>()
                .HasOne(c => c.BenhNhan)
                .WithMany()
                .HasForeignKey(c => c.BenhNhanId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NhomThuoc>().HasData(
                new NhomThuoc { Id = 1, TenNhomThuoc = "Giảm đau - Hạ sốt" },
                new NhomThuoc { Id = 2, TenNhomThuoc = "Kháng sinh" },
                new NhomThuoc { Id = 3, TenNhomThuoc = "Kháng viêm" },
                new NhomThuoc { Id = 4, TenNhomThuoc = "Dị ứng - Kháng Histamin" },
                new NhomThuoc { Id = 5, TenNhomThuoc = "Tiêu hoá" }
            );

            modelBuilder.Entity<VaiTroQuyen>()
                .HasKey(vq => new { vq.VaiTroId, vq.QuyenId });

            modelBuilder.Entity<VaiTroQuyen>()
                .HasOne(vq => vq.VaiTro)
                .WithMany(v => v.VaiTroQuyens)
                .HasForeignKey(vq => vq.VaiTroId);

            modelBuilder.Entity<VaiTroQuyen>()
                .HasOne(vq => vq.Quyen)
                .WithMany(q => q.VaiTroQuyens)
                .HasForeignKey(vq => vq.QuyenId);

            modelBuilder.Entity<VaiTroEntity>().HasData(
                new VaiTroEntity { Id = 1, TenVaiTro = "Admin", MoTa = "Quản trị viên hệ thống" },
                new VaiTroEntity { Id = 2, TenVaiTro = "BacSi", MoTa = "Bác sĩ" },
                new VaiTroEntity { Id = 3, TenVaiTro = "DuocSi", MoTa = "Dược sĩ" }
            );

            modelBuilder.Entity<QuyenEntity>().HasData(
                new QuyenEntity { Id = 1, MaQuyen = "THUOC_XEM", TenQuyen = "Xem danh mục thuốc", Module = "DanhMucThuoc" },
                new QuyenEntity { Id = 2, MaQuyen = "THUOC_QUANLY", TenQuyen = "Thêm/Sửa/Xoá thuốc", Module = "DanhMucThuoc" },
                new QuyenEntity { Id = 3, MaQuyen = "KIEMTRA_THUOC", TenQuyen = "Kiểm tra thuốc theo độ tuổi", Module = "KiemTra" },
                new QuyenEntity { Id = 4, MaQuyen = "BENHNHAN_XEM", TenQuyen = "Xem hồ sơ bệnh nhân", Module = "BenhNhan" },
                new QuyenEntity { Id = 5, MaQuyen = "BENHNHAN_QUANLY", TenQuyen = "Thêm/Sửa hồ sơ bệnh nhân", Module = "BenhNhan" },
                new QuyenEntity { Id = 6, MaQuyen = "TAIKHOAN_QUANLY", TenQuyen = "Quản lý tài khoản người dùng", Module = "TaiKhoan" },
                new QuyenEntity { Id = 7, MaQuyen = "BAOCAO_XEM", TenQuyen = "Xem báo cáo & thống kê", Module = "BaoCao" }
            );

            modelBuilder.Entity<VaiTroQuyen>().HasData(
                new VaiTroQuyen { VaiTroId = 1, QuyenId = 1 }, new VaiTroQuyen { VaiTroId = 1, QuyenId = 2 },
                new VaiTroQuyen { VaiTroId = 1, QuyenId = 3 }, new VaiTroQuyen { VaiTroId = 1, QuyenId = 4 },
                new VaiTroQuyen { VaiTroId = 1, QuyenId = 5 }, new VaiTroQuyen { VaiTroId = 1, QuyenId = 6 },
                new VaiTroQuyen { VaiTroId = 1, QuyenId = 7 },
                new VaiTroQuyen { VaiTroId = 2, QuyenId = 1 }, new VaiTroQuyen { VaiTroId = 2, QuyenId = 3 },
                new VaiTroQuyen { VaiTroId = 2, QuyenId = 4 }, new VaiTroQuyen { VaiTroId = 2, QuyenId = 5 },
                new VaiTroQuyen { VaiTroId = 2, QuyenId = 7 },
                new VaiTroQuyen { VaiTroId = 3, QuyenId = 1 }, new VaiTroQuyen { VaiTroId = 3, QuyenId = 2 },
                new VaiTroQuyen { VaiTroId = 3, QuyenId = 3 }, new VaiTroQuyen { VaiTroId = 3, QuyenId = 4 }
            );

            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung { Id = 1, HoTen = "Quản trị viên", Email = "admin@medicheck.com", PasswordHash = "$2b$11$SbZ5gOYejRWZvBm8eKlrOeJy7S/atGER.EoUob7x8uC7flq1AWsGa", VaiTro = VaiTro.Admin, TrangThai = TrangThaiTaiKhoan.HoatDong },
                new NguoiDung { Id = 2, HoTen = "Bác sĩ Nguyễn An", Email = "bacsi@medicheck.com", PasswordHash = "$2b$11$.X61ltPWJLaNMyprtN9YZ.FNDkMOvU6OfKkmw3I27wEEcxVxC22Tu", VaiTro = VaiTro.BacSi, TrangThai = TrangThaiTaiKhoan.HoatDong },
                new NguoiDung { Id = 3, HoTen = "Dược sĩ Trần Bình", Email = "duocsi@medicheck.com", PasswordHash = "$2b$11$TP1r5FPMtTIsFNNwEOoTau3bm.LfB3ErZ.50zNKw.p3gxPo30iaFq", VaiTro = VaiTro.DuocSi, TrangThai = TrangThaiTaiKhoan.HoatDong }
            );

            modelBuilder.Entity<BenhNhan>().HasData(
                new BenhNhan { Id = 1, MaBenhNhan = "BN0001", HoTen = "Nguyễn Văn A", NgaySinh = new DateTime(2019, 3, 10), GioiTinh = GioiTinh.Nam, CCCD = "079200000001", TienSuBenh = "Dị ứng với thành phần Aspirin" },
                new BenhNhan { Id = 2, MaBenhNhan = "BN0002", HoTen = "Trần Gia Bảo", NgaySinh = new DateTime(2011, 7, 22), GioiTinh = GioiTinh.Nam, CCCD = "079200000002", TienSuBenh = null },
                new BenhNhan { Id = 3, MaBenhNhan = "BN0003", HoTen = "Lê Thị Mai", NgaySinh = new DateTime(1990, 1, 15), GioiTinh = GioiTinh.Nu, CCCD = "079200000003", TienSuBenh = "Tiền sử viêm gan" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
