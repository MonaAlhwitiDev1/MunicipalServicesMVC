using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<int> GetSelectedPermissionIds(int roleId)
        {
            return _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToList();
        }

        public IEnumerable<RolePermission> GetRolePermissions(int roleId)
        {
            return _db.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToList();
        }

        public void DeleteRolePermissions(
            IEnumerable<RolePermission> rolePermissions)
        {
            _db.RolePermissions.RemoveRange(rolePermissions);
        }

        public void AddRolePermission(RolePermission rolePermission)
        {
            _db.RolePermissions.Add(rolePermission);
        }
    }
}