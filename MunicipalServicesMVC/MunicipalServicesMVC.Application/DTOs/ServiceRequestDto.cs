using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Application.Dtos
{
    public class CreateServiceRequestDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "اختر خدمة واحدة على الأقل")]
        public List<int> ServiceIds { get; set; } = new();

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }

    public class ServiceRequestDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "";

        public string? Notes { get; set; }

        public List<ServiceRequestItemDto> Items { get; set; } = new();
    }

    public class ServiceRequestItemDto
    {
        public int ServiceId { get; set; }

        public string ServiceName { get; set; } = "";

        public string Description { get; set; } = "";

        public int Quantity { get; set; } = 1;
    }
}