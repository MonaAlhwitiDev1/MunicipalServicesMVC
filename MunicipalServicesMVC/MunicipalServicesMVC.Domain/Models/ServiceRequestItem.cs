namespace MunicipalServicesMVC.Domain.Models
{
    public class ServiceRequestItem
    {
        public int Id { get; set; }

        public int ServiceRequestId { get; set; }

        public ServiceRequest ServiceRequest { get; set; } = null!;

        public int ServiceId { get; set; }

        public Service Service { get; set; } = null!;

        public int Quantity { get; set; } = 1;
    }
}