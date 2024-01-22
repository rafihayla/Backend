namespace DataAccessLayer.Entities
{
    public enum OrderStatus
    {
        InProcess, Delivered
    }

    public class OrderCreateDto
    {
        public DateTime DeliveryTime { get; set; }
        public string Address { get; set; } = "";
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required DateTime DeliveryTime { get; set; }
        public required DateTime OrderTime { get; set; }
        public required OrderStatus Status { get; set; }
        public required double Price { get; set; }
        //public required DishBasketDto[] Dishes { get; set; }
        public required string Address { get; set; } = "";
    }

    public class OrderInfoDto
    {
        public Guid Id { get; set; }
        public required DateTime DeliveryTime { get; set; }
        public required DateTime OrderTime { get; set; }
        public required OrderStatus Status { get; set; }
        public required double Price { get; set; }
    }
}
