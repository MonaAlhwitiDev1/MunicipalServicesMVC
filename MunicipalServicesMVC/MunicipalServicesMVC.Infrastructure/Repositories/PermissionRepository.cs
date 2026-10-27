using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class PermissionRepository
        : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext db) : base(db)
        {
        }
    }
}