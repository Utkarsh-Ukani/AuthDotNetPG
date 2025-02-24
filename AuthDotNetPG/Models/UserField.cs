namespace AuthDotNetPG.Models
{
    public class UserField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}