using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface ICartRepository
    {
        List<CartItem> GetByUserId(int userId);

        void RemoveRange(IEnumerable<CartItem> items);
    }

    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;

        public CartRepository(AppDbContext db)
        {
            _db = db;
        }

        public List<CartItem> GetByUserId(int userId)
        {
            return _db.CartItems
                .Include(item => item.Service)
                .Where(item => item.UserId == userId)
                .OrderBy(item => item.Id)
                .ToList();
        }

        public void RemoveRange(IEnumerable<CartItem> items)
        {
            _db.CartItems.RemoveRange(items);
        }
    }
}