namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IDepartmentRepository Departments { get; }
        IEmployeeRepository Employees { get; }
        IServiceRepository Services { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IUserRepository Users { get; }
        IServiceRequestRepository ServiceRequests { get; }
        ICartRepository Cart { get; }

        void Save();
    }
}