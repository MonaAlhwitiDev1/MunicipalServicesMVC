using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Infrastructure.Data;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAllWithRoleAndEmployee()
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .ToList();
        }

        public User? GetByIdWithRoleAndEmployee(int id)
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetByEmailWithRoleAndEmployee(string email)
        {
            return _db.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefault(u => u.Email == email);
        }
    }
}