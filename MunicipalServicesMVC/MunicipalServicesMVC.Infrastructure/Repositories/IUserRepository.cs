using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        // جميع المستخدمين مع الدور والموظف
        IEnumerable<User> GetAllWithRoleAndEmployee();

        // جلب مستخدم مع الدور والموظف
        User? GetByIdWithRoleAndEmployee(int id);

        // جلب مستخدم بالإيميل مع الدور والموظف
        User? GetByEmailWithRoleAndEmployee(string email);
    }
}