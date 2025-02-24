namespace AuthDotNetPG.Models
{
    public class Tax
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}