namespace AuthDotNetPG.DTOs
{
    public class ChargeCreditCardDto
    {
        public class OrderDTO
        {
            public int Id { get; set; }
            //public string? RefId { get; set; }
            public decimal Amount { get; set; }
            public string Currency { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? CustomerIP { get; set; }
            //public string? PONumber { get; set; }
            public string? OrderStatus { get; set; } 

            public TransactionDTO Transaction { get; set; }
            public CustomerDTO Customer { get; set; }
            public BillingAddressDTO BillTo { get; set; }
            public ShippingAddressDTO ShipTo { get; set; }
            public List<LineItemDTO> LineItems { get; set; }
            public TaxDTO Tax { get; set; }
            public List<UserFieldDTO>? UserFields { get; set; }
        }

        public class LineItemDTO
        {
            public int Id { get; set; }
            public string ItemId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class CustomerDTO
        {
            public int Id { get; set; }
            public string CustomerId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
        }

        public class CreditCardDTO
        {
            public int Id { get; set; }
            public string ExpirationDate { get; set; }

            public string CardNumber { get; set; }

            public string CardCode { get; set; }
        }

        public class BillingAddressDTO
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Company { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
        }

        public class ShippingAddressDTO
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Company { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
        }

        public class AuthorizationIndicatorDTO
        {
            public int Id { get; set; }
            public string AuthorizationIndicatorType { get; set; }
        }

        public class ProcessingOptionsDTO
        {
            public int Id { get; set; }
            public bool IsSubsequentAuth { get; set; }
        }

        public class SubsequentAuthInformationDTO
        {
            public int Id { get; set; }
            public string OriginalNetworkTransId { get; set; }
            public decimal OriginalAuthAmount { get; set; }
            public string Reason { get; set; }
        }

        public class TaxDTO
        {
            public int Id { get; set; }
            public decimal Amount { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class TransactionDTO
        {
            public int Id { get; set; }
            public string? TransactionId { get; set; }
            public string TransactionType { get; set; }
            public string? AuthorizationCode { get; set; }
            public string? TransactionStatus { get; set; }
            public DateTime? TransactionDate { get; set; }
            public string? ResponseCode { get; set; }
            public string? ResponseMessage { get; set; }
            public string? NetworkTransactionId { get; set; }

            public CreditCardDTO CreditCard { get; set; }
            public ProcessingOptionsDTO? ProcessingOptions { get; set; }
            public SubsequentAuthInformationDTO? SubsequentAuthInfo { get; set; }
            public AuthorizationIndicatorDTO? AuthorizationIndicator { get; set; }
        }

        public class UserFieldDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
