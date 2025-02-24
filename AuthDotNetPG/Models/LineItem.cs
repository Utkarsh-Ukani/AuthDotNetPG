namespace AuthDotNetPG.Models
{
    public class LineItem
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}