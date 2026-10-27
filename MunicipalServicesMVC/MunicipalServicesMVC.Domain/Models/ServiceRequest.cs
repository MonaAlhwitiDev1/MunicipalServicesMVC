using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesMVC.Domain.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "جديد";

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public ICollection<ServiceRequestItem> Items { get; set; }
            = new List<ServiceRequestItem>();
    }
}