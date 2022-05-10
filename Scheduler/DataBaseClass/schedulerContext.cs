using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Scheduler.DataBaseClass
{
    public partial class schedulerContext : DbContext
    {
        public schedulerContext()
        {
        }

        public schedulerContext(DbContextOptions<schedulerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Case> Cases { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=80.240.250.128;database=scheduler;user id=Vasiliev;password=aaaa", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.26-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasKey(e => e.Idcase)
                    .HasName("PRIMARY");

                entity.ToTable("case");

                entity.HasIndex(e => e.UsersIdusers, "fk_case_users_idx");

                entity.HasIndex(e => e.Idcase, "idcase_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idcase).HasColumnName("idcase");

                entity.Property(e => e.CaseBookmark).HasColumnName("case_bookmark");

                entity.Property(e => e.CaseDate).HasColumnName("case_date");

                entity.Property(e => e.CaseDescription)
                    .HasColumnType("text")
                    .HasColumnName("case_description");

                entity.Property(e => e.CaseDone).HasColumnName("case_done");

                entity.Property(e => e.CaseTimeend)
                    .HasColumnType("time")
                    .HasColumnName("case_timeend");

                entity.Property(e => e.CaseTimestart)
                    .HasColumnType("time")
                    .HasColumnName("case_timestart");

                entity.Property(e => e.CaseTitle)
                    .HasColumnType("text")
                    .HasColumnName("case_title");

                entity.Property(e => e.UsersIdusers).HasColumnName("users_idusers");

                entity.HasOne(d => d.UsersIdusersNavigation)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.UsersIdusers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_case_users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Idusers)
                    .HasName("PRIMARY");

                entity.ToTable("users");

                entity.HasIndex(e => e.Idusers, "idusers_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idusers).HasColumnName("idusers");

                entity.Property(e => e.UsersEmail)
                    .HasColumnType("text")
                    .HasColumnName("users_email");

                entity.Property(e => e.UsersLogin)
                    .HasColumnType("text")
                    .HasColumnName("users_login");

                entity.Property(e => e.UsersPassword)
                    .HasColumnType("text")
                    .HasColumnName("users_password");

                entity.Property(e => e.UsersPhone)
                    .HasColumnType("text")
                    .HasColumnName("users_phone");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
