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

            base.OnModelCreating(modelBuilder);
        }
    }
}
