using System.Collections.Generic;
using MunicipalServicesMVC.Application.Dtos;

namespace MunicipalServicesMVC.Application.Services
{
    public interface IServiceRequestService
    {
        ServiceRequestDto Create(
            int userId,
            CreateServiceRequestDto dto);

        List<ServiceRequestDto> GetMyRequests(int userId);

        List<ServiceRequestDto> GetAll();
    }
}