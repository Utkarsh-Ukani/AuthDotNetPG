namespace AuthDotNetPG.Models
{
    public class Transaction
    {
        public int Id { get; set; }  
        public Order Order { get; set; }

        public string? TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? TransactionStatus { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public string? NetworkTransactionId { get; set; }

        public CreditCard CreditCard { get; set; }
        public ProcessingOptions? ProcessingOptions { get; set; }
        public SubsequentAuthInformation? SubsequentAuthInfo { get; set; }
        public AuthorizationIndicator? AuthorizationIndicator { get; set; }
    }
}
