using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace HRSys.Model
{
    public partial class HRSysContext : IdentityDbContext<ApplicationUser>
    {
        public HRSysContext(DbContextOptions<HRSysContext> options)
            : base(options)
        {

        }

        //HR SCHEMA

        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Sys_Users> Sys_Users { get; set; }
        public virtual DbSet<Announcements> Announcements { get; set; }
        public virtual DbSet<AnnouncementsGroups> AnnouncementsGroups { get; set; }

        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<Shifts> Shifts { get; set; }
        public virtual DbSet<TasksStatus> TasksStatus { get; set; }
        public virtual DbSet<Nationalities> Nationalities { get; set; }
        public virtual DbSet<VacationTypes> VacationTypes { get; set; }
        public virtual DbSet<VacationsTransactions> VacationsTransactions { get; set; }

        public virtual DbSet<AttendanceTransactions> AttendanceTransactions { get; set; }
        public virtual DbSet<AttendanceStatuses> AttendanceStatuses { get; set; }
        //End HR SCHEMA


        public virtual DbSet<BusinessPermissions> BusinessPermissions { get; set; }
        public virtual DbSet<BusinessRoleNames> BusinessRoleNames { get; set; }
        public virtual DbSet<BusinessRolePermissions> BusinessRolePermissions { get; set; }
        public virtual DbSet<BusinessRoles> BusinessRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["HRSysConnection"].ConnectionString);
                optionsBuilder.EnableSensitiveDataLogging(true);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<BusinessRolePermissions>(entity =>
            {
                entity.HasKey(e => new { e.BusinessRoleId, e.BusinessPermissionId });

                entity.ToTable("BusinessRolePermissions", "Business");

                entity.HasOne(d => d.BusinessPermission)
                    .WithMany(p => p.BusinessRolePermissions)
                    .HasForeignKey(d => d.BusinessPermissionId);

                entity.HasOne(d => d.BusinessRole)
                    .WithMany(p => p.BusinessRolePermissions)
                    .HasForeignKey(d => d.BusinessRoleId);
            });

            modelBuilder.Entity<BusinessRoles>(entity =>
            {
                entity.ToTable("BusinessRoles", "Business");

            });


            //HR SCHEMA
            modelBuilder.Entity<Employees>(entity =>
            {
                entity.ToTable("Employees", "HR");
                entity.Property(e => e.MobileNo).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Nationalities)
                   .WithMany(p => p.Employees)
                   .HasForeignKey(d => d.NationalityId)
                   .HasConstraintName("FK_App_Users_Nationalities");

                entity.HasOne(d => d.AnnouncementsGroups)
                   .WithMany(p => p.Employees)
                   .HasForeignKey(d => d.AnnouncementGroupId)
                   .HasConstraintName("FK_Employees_AnnouncementsGroups");

                entity.HasOne(d => d.Shifts)
                  .WithMany(p => p.Employees)
                  .HasForeignKey(d => d.ShiftId)
                  .HasConstraintName("FK_Employees_Shifts");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.ToTable("Tasks", "HR");
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.EmployeeId).IsRequired();
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.TasksStatus)
                   .WithMany(p => p.Tasks)
                   .HasForeignKey(d => d.StatusId)
                   .HasConstraintName("FK_Tasks_TasksStatus");

                entity.HasOne(d => d.Employees)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(d => d.EmployeeId)
                  .HasConstraintName("FK_Tasks_Employees");
            });


            modelBuilder.Entity<Sys_Users>(entity =>
            {
                entity.ToTable("Sys_Users", "HR");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<AnnouncementsGroups>(entity =>
            {
                entity.ToTable("AnnouncementsGroups", "HR");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Announcements>(entity =>
            {
                entity.ToTable("Announcements", "HR");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.AnnouncementsGroups)
                //  .WithMany(p => p.Announcements)
                //  .HasForeignKey(d => d.TargetGroupId)
                //  .HasConstraintName("FK_Ann_AnnGroups");

                entity.HasOne(d => d.Employees)
                 .WithMany(p => p.Announcements)
                 .HasForeignKey(d => d.OwnerId)
                 .HasConstraintName("FK_Announcements_Employees");
            });

            modelBuilder.Entity<TasksStatus>(entity =>
            {
                entity.ToTable("TasksStatus", "HR");
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Nationalities>(entity =>
                      {
                          entity.ToTable("Nationalities", "Lookup");
                      });


            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.ToTable("Notifications", "HR");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Sys_Users)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.Sys_UserID)
                    .HasConstraintName("FK_Notifications_Sys_Users");
            });


            modelBuilder.Entity<VacationsTransactions>(entity =>
            {
                entity.ToTable("VacationsTransactions", "HR");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.VacationsTransactions)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_VacationsTransactions_Employees");

                entity.HasOne(d => d.VacationTypes)
                    .WithMany(p => p.VacationsTransactions)
                    .HasForeignKey(d => d.VacationTypeId)
                    .HasConstraintName("FK_VacationsTransactions_VacationTypes");
            });

            modelBuilder.Entity<AttendanceTransactions>(entity =>
            {
                entity.ToTable("AttendanceTransactions", "HR");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Employees)
                    .WithMany(p => p.AttendanceTransactions)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_AttendanceTransactions_Employees");

                entity.HasOne(d => d.AttendanceStatuses)
                   .WithMany(p => p.AttendanceTransactions)
                   .HasForeignKey(d => d.AttendanceStatusId)
                   .HasConstraintName("FK_AttendanceTransactions_AttendanceStatuses");


            });


            modelBuilder.Entity<Teams>(entity =>
            {
                entity.ToTable("Teams", "Lookup");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Shifts>(entity =>
            {
                entity.ToTable("Shifts", "HR");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<VacationTypes>(entity =>
            {
                entity.ToTable("VacationTypes", "HR");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            //End SCHEMA
            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
            GlobalFilters(modelBuilder);

        }
        private void GlobalFilters(ModelBuilder builder)
        {
            #region lookup
            builder.Entity<BusinessRoles>().HasQueryFilter(r => !r.IsDeleted);
            builder.Entity<BusinessPermissions>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<BusinessRolePermissions>().HasQueryFilter(rp => !rp.IsDeleted);
            #endregion

        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }

}
