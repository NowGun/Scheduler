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
        public virtual DbSet<Casegroup> Casegroups { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserHasGroup> UsersHasGroups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=193.33.230.80;database=scheduler;user id=Zhirov;password=64580082", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.26-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

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

                entity.Property(e => e.CaseDate)
                    .HasColumnType("date")
                    .HasColumnName("case_date");

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
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("case_title");

                entity.Property(e => e.UsersIdusers).HasColumnName("users_idusers");

                entity.HasOne(d => d.UsersIdusersNavigation)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.UsersIdusers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_case_users");
            });

            modelBuilder.Entity<Casegroup>(entity =>
            {
                entity.HasKey(e => e.Idcasegroup)
                    .HasName("PRIMARY");

                entity.ToTable("casegroup");

                entity.HasIndex(e => e.CategoryIdcategory, "fk_casegroup_category1_idx");

                entity.HasIndex(e => e.GroupsIdgroups, "fk_casegroup_groups1_idx");

                entity.HasIndex(e => e.Idcasegroup, "idcasegroup_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idcasegroup).HasColumnName("idcasegroup");

                entity.Property(e => e.CategoryIdcategory).HasColumnName("category_idcategory");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(45)
                    .HasColumnName("description");

                entity.Property(e => e.Done).HasColumnName("done");

                entity.Property(e => e.GroupsIdgroups).HasColumnName("groups_idgroups");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("title");

                entity.HasOne(d => d.CategoryIdcategoryNavigation)
                    .WithMany(p => p.Casegroups)
                    .HasForeignKey(d => d.CategoryIdcategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_casegroup_category1");

                entity.HasOne(d => d.GroupsIdgroupsNavigation)
                    .WithMany(p => p.Casegroups)
                    .HasForeignKey(d => d.GroupsIdgroups)
                    .HasConstraintName("fk_casegroup_groups1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idcategory)
                    .HasName("PRIMARY");

                entity.ToTable("category");

                entity.HasIndex(e => e.Idcategory, "idcategory_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idcategory).HasColumnName("idcategory");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("category_name");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Idgroups)
                    .HasName("PRIMARY");

                entity.ToTable("groups");

                entity.HasIndex(e => e.UsersCreate, "fk_groups_users1_idx");

                entity.HasIndex(e => e.Idgroups, "idgroups_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Idgroups).HasColumnName("idgroups");

                entity.Property(e => e.GroupsDescription)
                    .HasMaxLength(45)
                    .HasColumnName("groups_description");

                entity.Property(e => e.GroupsName)
                    .HasMaxLength(45)
                    .HasColumnName("groups_name");

                entity.Property(e => e.UsersCreate).HasColumnName("users_create");

                entity.HasOne(d => d.UsersCreateNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.UsersCreate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_groups_users1");
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
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("users_email");

                entity.Property(e => e.UsersName)
                    .HasMaxLength(45)
                    .HasColumnName("users_name");

                entity.Property(e => e.UsersPassword)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("users_password");

                entity.Property(e => e.UsersPhone)
                    .HasColumnType("text")
                    .HasColumnName("users_phone");

                entity.Property(e => e.UsersSurname)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("users_surname");
            });

            modelBuilder.Entity<UserHasGroup>(entity =>
            {
                entity.HasKey(e => new { e.UsersIdusers, e.GroupsIdgroups })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("users_has_groups");

                entity.HasIndex(e => e.GroupsIdgroups, "fk_users_has_groups_groups1_idx");

                entity.HasIndex(e => e.UsersIdusers, "fk_users_has_groups_users1_idx");

                entity.Property(e => e.UsersIdusers).HasColumnName("users_idusers");

                entity.Property(e => e.GroupsIdgroups).HasColumnName("groups_idgroups");

                entity.HasOne(d => d.GroupsIdgroupsNavigation)
                    .WithMany(p => p.UserHasGroups)
                    .HasForeignKey(d => d.GroupsIdgroups)
                    .HasConstraintName("fk_users_has_groups_groups1");

                entity.HasOne(d => d.UsersIdusersNavigation)
                    .WithMany(p => p.UserHasGroups)
                    .HasForeignKey(d => d.UsersIdusers)
                    .HasConstraintName("fk_users_has_groups_users1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
