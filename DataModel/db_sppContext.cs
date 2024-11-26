using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataModel
{
    public partial class db_sppContext : DbContext
    {
        public db_sppContext()
        {
        }

        public db_sppContext(DbContextOptions<db_sppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbMAdmin> TbMAdmins { get; set; } = null!;
        public virtual DbSet<TbMBiodatum> TbMBiodata { get; set; } = null!;
        public virtual DbSet<TbMJurusan> TbMJurusans { get; set; } = null!;
        public virtual DbSet<TbMKela> TbMKelas { get; set; } = null!;
        public virtual DbSet<TbMRole> TbMRoles { get; set; } = null!;
        public virtual DbSet<TbMSiswa> TbMSiswas { get; set; } = null!;
        public virtual DbSet<TbMUser> TbMUsers { get; set; } = null!;
        public virtual DbSet<TbSekolah> TbSekolahs { get; set; } = null!;
        public virtual DbSet<TbTPembayaran> TbTPembayarans { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=db_spp;user id=sa;Password=P@ssw0rd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbMAdmin>(entity =>
            {
                entity.ToTable("tb_m_admin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BiodataId).HasColumnName("biodata_id");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");
            });

            modelBuilder.Entity<TbMBiodatum>(entity =>
            {
                entity.ToTable("tb_m_biodata");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("fullname");

                entity.Property(e => e.Images)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("images");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mobile_phone");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");
            });

            modelBuilder.Entity<TbMJurusan>(entity =>
            {
                entity.ToTable("tb_m_jurusan");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.NamaJurusan)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nama_jurusan");
            });

            modelBuilder.Entity<TbMKela>(entity =>
            {
                entity.ToTable("tb_m_kelas");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.JurusanId).HasColumnName("jurusan_id");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.NamaKelas)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nama_kelas");
            });

            modelBuilder.Entity<TbMRole>(entity =>
            {
                entity.ToTable("tb_m_role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbMSiswa>(entity =>
            {
                entity.ToTable("tb_m_siswa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BiodataId).HasColumnName("biodata_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.JurusanId).HasColumnName("jurusan_id");

                entity.Property(e => e.KelasId).HasColumnName("kelas_id");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.Nis)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nis");

                entity.Property(e => e.Nisn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nisn");

                entity.Property(e => e.TahunMasuk)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tahun_masuk");
            });

            modelBuilder.Entity<TbMUser>(entity =>
            {
                entity.ToTable("tb_m_user");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BiodataId).HasColumnName("biodata_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.IsLocked).HasColumnName("is_locked");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login");

                entity.Property(e => e.LoginAttempt).HasColumnName("login_attempt");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.TahunMasuk)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tahun_masuk");
            });

            modelBuilder.Entity<TbSekolah>(entity =>
            {
                entity.ToTable("tb_sekolah");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Alamat)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("alamat");

                entity.Property(e => e.BiayaSpp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("biaya_spp");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.Email)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsDelete).HasColumnName("is_delete");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.NamaSekolah)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("nama_sekolah");

                entity.Property(e => e.NoTlp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("no_tlp");

                entity.Property(e => e.Npsn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("npsn");

                entity.Property(e => e.StatusSekolah)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("status_sekolah");
            });

            modelBuilder.Entity<TbTPembayaran>(entity =>
            {
                entity.ToTable("tb_t_pembayaran");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bulan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("bulan");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");

                entity.Property(e => e.DeletedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("deleted_on");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Jumlah).HasColumnName("jumlah");

                entity.Property(e => e.KelasId).HasColumnName("kelas_id");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.SiswaId).HasColumnName("siswa_id");

                entity.Property(e => e.Tahun)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("tahun");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
