namespace AuthDotNetPG.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CardCode { get; set; }

        // Foreign Key
        public int TransactionId { get; set; }

        public Transaction Transaction { get; set; }
    }
}