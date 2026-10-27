using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        List<int> GetSelectedPermissionIds(int roleId);

        IEnumerable<RolePermission> GetRolePermissions(int roleId);

        void DeleteRolePermissions(IEnumerable<RolePermission> rolePermissions);

        void AddRolePermission(RolePermission rolePermission);
    }
}