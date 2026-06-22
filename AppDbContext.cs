using Microsoft.EntityFrameworkCore;
using MediCheck.Api.Models;

namespace MediCheck.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs => Set<NguoiDung>();
        public DbSet<Thuoc> Thuocs => Set<Thuoc>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Thuoc>()
                .HasIndex(t => t.MaThuoc)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}