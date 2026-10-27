using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeFile> EmployeeFiles { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceRequestItem> ServiceRequestItems { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ربط المستخدم بالدور
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ربط الدور بصلاحياته
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // ربط الصلاحية بالأدوار
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // منع تكرار الصلاحية لنفس الدور
            modelBuilder.Entity<RolePermission>()
                .HasIndex(rp => new
                {
                    rp.RoleId,
                    rp.PermissionId
                })
                .IsUnique();

            // ربط الطلب بصاحب الطلب
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(request => request.User)
                .WithMany()
                .HasForeignKey(request => request.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ربط الطلب بالخدمات المختارة داخله
            modelBuilder.Entity<ServiceRequestItem>()
                .HasOne(item => item.ServiceRequest)
                .WithMany(request => request.Items)
                .HasForeignKey(item => item.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // ربط عنصر الطلب بالخدمة
            modelBuilder.Entity<ServiceRequestItem>()
                .HasOne(item => item.Service)
                .WithMany()
                .HasForeignKey(item => item.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // منع تكرار الخدمة داخل نفس الطلب
            modelBuilder.Entity<ServiceRequestItem>()
                .HasIndex(item => new
                {
                    item.ServiceRequestId,
                    item.ServiceId
                })
                .IsUnique();

            // ربط عنصر السلة بالمستخدم
            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.User)
                .WithMany()
                .HasForeignKey(item => item.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ربط عنصر السلة بالخدمة
            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.Service)
                .WithMany()
                .HasForeignKey(item => item.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // منع تكرار الخدمة في سلة المستخدم
            modelBuilder.Entity<CartItem>()
                .HasIndex(item => new
                {
                    item.UserId,
                    item.ServiceId
                })
                .IsUnique();
        }
    }
}