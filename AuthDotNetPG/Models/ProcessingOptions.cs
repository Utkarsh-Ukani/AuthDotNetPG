namespace AuthDotNetPG.Models
{
    public class ProcessingOptions
    {
        public int Id { get; set; }
        public bool IsSubsequentAuth { get; set; }

        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; }
    }
}