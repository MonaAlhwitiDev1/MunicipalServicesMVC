using System;
using System.Collections.Generic;
using System.Linq;
using MunicipalServicesMVC.Application.Dtos;
using MunicipalServicesMVC.Domain.Models;
using MunicipalServicesMVC.Infrastructure.Repositories;

namespace MunicipalServicesMVC.Application.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceRequestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceRequestDto Create(
            int userId,
            CreateServiceRequestDto dto)
        {
            var user = _unitOfWork.Users.GetById(userId);

            if (user == null)
                throw new ArgumentException("المستخدم غير موجود");

            if (dto.ServiceIds == null || dto.ServiceIds.Count == 0)
                throw new ArgumentException("اختر خدمة واحدة على الأقل");

            var notes = dto.Notes?.Trim();

            if (notes?.Length > 1000)
                throw new ArgumentException(
                    "الملاحظات يجب ألا تتجاوز 1000 حرف");

            var request = new ServiceRequest
            {
                UserId = userId,
                User = user,
                CreatedAt = DateTime.UtcNow,
                Status = "جديد",
                Notes = notes
            };

            foreach (var serviceId in dto.ServiceIds.Distinct())
            {
                var service = _unitOfWork.Services.GetById(serviceId);

                if (service == null)
                    throw new ArgumentException(
                        $"الخدمة رقم {serviceId} غير موجودة");

                request.Items.Add(new ServiceRequestItem
                {
                    ServiceId = serviceId,
                    Service = service
                });
            }

            _unitOfWork.ServiceRequests.Add(request);
            _unitOfWork.Save();

            return MapToDto(request);
        }

        public List<ServiceRequestDto> GetMyRequests(int userId)
        {
            return _unitOfWork.ServiceRequests
                .GetByUserId(userId)
                .Select(MapToDto)
                .ToList();
        }

        public List<ServiceRequestDto> GetAll()
        {
            return _unitOfWork.ServiceRequests
                .GetAllWithDetails()
                .Select(MapToDto)
                .ToList();
        }

        private static ServiceRequestDto MapToDto(
            ServiceRequest request)
        {
            return new ServiceRequestDto
            {
                Id = request.Id,
                UserName = request.User?.Name ?? "",
                CreatedAt = request.CreatedAt,
                Status = request.Status,
                Notes = request.Notes,
                Items = request.Items
                    .Select(item => new ServiceRequestItemDto
                    {
                        ServiceId = item.ServiceId,
                        ServiceName = item.Service?.Name ?? "",
                        Description = item.Service?.Description ?? "",
                        Quantity = item.Quantity
                    })
                    .ToList()
            };
        }
    }
}