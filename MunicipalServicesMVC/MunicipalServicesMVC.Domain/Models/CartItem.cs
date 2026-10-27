namespace MunicipalServicesMVC.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ServiceId { get; set; }

        public int Quantity { get; set; } = 1;

        public User User { get; set; } = null!;

        public Service Service { get; set; } = null!;
    }
}