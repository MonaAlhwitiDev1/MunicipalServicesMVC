using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Data;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AppDbContext _db;

        public ServiceRequestRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(ServiceRequest request)
        {
            _db.ServiceRequests.Add(request);
        }

        public List<ServiceRequest> GetAllWithDetails()
        {
            return _db.ServiceRequests
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Service)
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .ToList();
        }

        public List<ServiceRequest> GetByUserId(int userId)
        {
            return _db.ServiceRequests
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Service)
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .ToList();
        }

        public ServiceRequest? GetByIdWithDetails(int id)
        {
            return _db.ServiceRequests
                .Include(r => r.User)
                .Include(r => r.Items)
                    .ThenInclude(i => i.Service)
                .FirstOrDefault(r => r.Id == id);
        }
    }
}