using System.Collections.Generic;
using MunicipalServicesMVC.Domain.Models;

namespace MunicipalServicesMVC.Infrastructure.Repositories
{
    public interface IServiceRequestRepository
    {
        void Add(ServiceRequest request);

        List<ServiceRequest> GetAllWithDetails();

        List<ServiceRequest> GetByUserId(int userId);

        ServiceRequest? GetByIdWithDetails(int id);
    }
}