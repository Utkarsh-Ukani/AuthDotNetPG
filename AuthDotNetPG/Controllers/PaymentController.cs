using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthDotNetPG.DTOs;
using AuthorizeNet.Api.Contracts.V1;
using AuthDotNetPG.Models;
using Microsoft.EntityFrameworkCore;
using AuthorizeNet.Api.Controllers;
using AuthDotNetPG.Data;
using AuthorizeNet.Api.Controllers.Bases;

namespace AuthDotNetPG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string _apiLoginId;
        private readonly string _transactionKey;
        private readonly bool _isProduction;
        private readonly ILogger<PaymentController> _logger;
        private readonly PaymentDbContext _context;

        public PaymentController(
            IConfiguration configuration,
            ILogger<PaymentController> logger,
            PaymentDbContext context)
        {
            _apiLoginId = configuration["AuthorizeNet:ApiLoginId"];
            _transactionKey = configuration["AuthorizeNet:TransactionKey"];
            _isProduction = configuration.GetValue<bool>("AuthorizeNet:IsProduction");
            _logger = logger;
            _context = context;
        }

        [HttpPost("charge")]
public async Task<IActionResult> ChargeCard([FromBody] ChargeCreditCardDto.OrderDTO order)
{
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            try
    {
        // Save Order First
        var savedOrder = await SaveOrderAsync(order);

        var merchantAuth = new merchantAuthenticationType
        {
            name = _apiLoginId,
            ItemElementName = ItemChoiceType.transactionKey,
            Item = _transactionKey
        };

        var creditCard = new creditCardType
        {
            cardNumber = order.Transaction.CreditCard.CardNumber,
            expirationDate = order.Transaction.CreditCard.ExpirationDate,
            cardCode = order.Transaction.CreditCard.CardCode
        };

        var transactionRequest = new transactionRequestType
        {
            transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),
            amount = order.Amount,
            payment = new paymentType { Item = creditCard },
            billTo = new customerAddressType
            {
                firstName = order.BillTo.FirstName,
                lastName = order.BillTo.LastName,
                address = order.BillTo.Address,
                city = order.BillTo.City,
                state = order.BillTo.State,
                zip = order.BillTo.Zip,
                country = order.BillTo.Country
            },
            customerIP = order.CustomerIP,
            order = new orderType
            {
                invoiceNumber = savedOrder.RefId,
                description = $"Order {savedOrder.RefId}"
            }
        };

        var request = new createTransactionRequest
        {
            merchantAuthentication = merchantAuth,
            transactionRequest = transactionRequest
        };

        var controller = new createTransactionController(request);
        controller.Execute();
        var response = controller.GetApiResponse();

        if (response != null && response.transactionResponse.responseCode=="1" && response.messages.resultCode == messageTypeEnum.Ok)
        {
            // Save transaction first
            var transaction = await SaveTransactionAsync(response, savedOrder.Id, order);

            // Update order after transaction is saved
            savedOrder.OrderStatus = "Completed";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                OrderId = savedOrder.Id,
                transaction.TransactionId,
                Status = "Success",
                AuthCode = response.transactionResponse.authCode,
                ResponseCode = response.transactionResponse.responseCode,
                Messages = response.transactionResponse.messages.Select(m => new
                {
                    Code = m.code,
                    Description = m.description
                }).ToArray()
            });
        }
        else
        {
            // Update order status if transaction fails
            savedOrder.OrderStatus = "Failed";
            await _context.SaveChangesAsync();

                    // error if card number is wrong

                    foreach (var error in response.transactionResponse.errors)
                    {
                        if (error.errorCode == "6") // Error code 6 = invalid card number
                        {
                            return BadRequest(new
                            {
                                Message = "Invalid card number. Please check and try again."
                            });
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                Message = $"Transaction error: {error.errorText}"
                            });
                        }
                    }

                    //error if cvv is wrong

                    var cvvStatus = response.transactionResponse.cvvResultCode;

                    if (cvvStatus != "M")
                    {
                        if (cvvStatus == "N")
                        {
                            return BadRequest(new
                            {
                                Status = "Failed",
                                Message = "The CVV code provided is invalid"
                            });
                        }else if(cvvStatus == "P")
                        {
                            return BadRequest(new
                            {
                                Status = "Failed",
                                Message = "The CVV code provided is not processed"
                            });
                        }
                    }

            return BadRequest(new
            {
                Status = "Failed",
                Message = response?.messages?.message[0]?.text ?? "Unknown error"
            });
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error processing payment");
        return StatusCode(500, new { Status = "Error", Message = "Internal server error" });
    }
}


        private async Task<Order> SaveOrderAsync(ChargeCreditCardDto.OrderDTO orderDto)
        {
            var order = new Order
            {
                //RefId = string.IsNullOrEmpty(orderDto.RefId) ? order.RefId : orderDto.RefId,
                Amount = orderDto.Amount,
                Currency = orderDto.Currency,
                CreatedAt = DateTime.UtcNow,
                CustomerIP = orderDto.CustomerIP,
                //PONumber = orderDto.PONumber,
                OrderStatus = "Pending",

                Customer = new Customer
                {
                    CustomerId = orderDto.Customer.CustomerId,
                    Name = orderDto.Customer.Name,
                    Email = orderDto.Customer.Email,
                    Phone = orderDto.Customer.Phone,
                    Address = orderDto.Customer.Address
                },

                BillTo = new BillingAddress
                {
                    FirstName = orderDto.BillTo.FirstName,
                    LastName = orderDto.BillTo.LastName,
                    Company = orderDto.BillTo.Company,
                    Address = orderDto.BillTo.Address,
                    City = orderDto.BillTo.City,
                    State = orderDto.BillTo.State,
                    Zip = orderDto.BillTo.Zip,
                    Country = orderDto.BillTo.Country
                },

                ShipTo = new ShippingAddress
                {
                    FirstName = orderDto.ShipTo.FirstName,
                    LastName = orderDto.ShipTo.LastName,
                    Company = orderDto.ShipTo.Company,
                    Address = orderDto.ShipTo.Address,
                    City = orderDto.ShipTo.City,
                    State = orderDto.ShipTo.State,
                    Zip = orderDto.ShipTo.Zip,
                    Country = orderDto.ShipTo.Country
                },

                LineItems = orderDto.LineItems?.Select(item => new LineItem
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList(),

                Tax = new Tax
                {
                    Amount = orderDto.Tax.Amount,
                    Name = orderDto.Tax.Name,
                    Description = orderDto.Tax.Description
                }
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        private async Task<Transaction> SaveTransactionAsync(createTransactionResponse response, int orderId, ChargeCreditCardDto.OrderDTO orderDto)
        {
            var transaction = new Transaction
            {
                TransactionId = response.transactionResponse.transId,
                TransactionType = "authCaptureTransaction",
                AuthorizationCode = response.transactionResponse.authCode,
                TransactionStatus = "Completed",
                TransactionDate = DateTime.UtcNow,
                ResponseCode = response.transactionResponse.responseCode,
                ResponseMessage = response.transactionResponse.messages[0].description,
                NetworkTransactionId = response.transactionResponse.networkTransId,

                CreditCard = new CreditCard
                {
                    ExpirationDate = orderDto.Transaction.CreditCard.ExpirationDate,
                    CardNumber = orderDto.Transaction.CreditCard.CardNumber,
                    CardCode = orderDto.Transaction.CreditCard.CardCode
                },

                ProcessingOptions = new ProcessingOptions
                {
                    IsSubsequentAuth = false
                },

                AuthorizationIndicator = new AuthorizationIndicator
                {
                    AuthorizationIndicatorType = orderDto.Transaction.AuthorizationIndicator.AuthorizationIndicatorType
                }
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            //  Update order with the transaction's ID
            var savedOrder = await _context.Orders.FindAsync(orderId);
            savedOrder.TransactionId = transaction.Id;
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}