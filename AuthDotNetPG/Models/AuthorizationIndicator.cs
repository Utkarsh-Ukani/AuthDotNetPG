namespace AuthDotNetPG.Models
{
    public class AuthorizationIndicator
    {
        public int Id { get; set; }
        public string AuthorizationIndicatorType { get; set; }

        // Foreign Key
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}