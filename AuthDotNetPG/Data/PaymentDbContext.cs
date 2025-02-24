using AuthDotNetPG.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthDotNetPG.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<BillingAddress> BillingAddresses { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserField> UserFields { get; set; }
        public DbSet<AuthorizationIndicator> AuthorizationIndicators { get; set; }
        public DbSet<ProcessingOptions> ProcessingOptions { get; set; }
        public DbSet<SubsequentAuthInformation> SubsequentAuthInformations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Transaction)
                .WithOne(t => t.Order)
                .HasForeignKey<Order>(o => o.TransactionId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillTo)
                .WithOne(b => b.Order)
                .HasForeignKey<BillingAddress>(b => b.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShipTo)
                .WithOne(s => s.Order)
                .HasForeignKey<ShippingAddress>(s => s.OrderId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.LineItems)
                .WithOne(li => li.Order)
                .HasForeignKey(li => li.OrderId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.UserFields)
                .WithOne(uf => uf.Order)
                .HasForeignKey(uf => uf.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Tax)
                .WithOne(t => t.Order)
                .HasForeignKey<Tax>(t => t.OrderId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.CreditCard)
                .WithOne(c => c.Transaction)
                .HasForeignKey<CreditCard>(c => c.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ProcessingOptions)
                .WithOne(po => po.Transaction)
                .HasForeignKey<ProcessingOptions>(po => po.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SubsequentAuthInfo)
                .WithOne(sa => sa.Transaction)
                .HasForeignKey<SubsequentAuthInformation>(sa => sa.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.AuthorizationIndicator)
                .WithOne(ai => ai.Transaction)
                .HasForeignKey<AuthorizationIndicator>(ai => ai.TransactionId);
        }

    }
}
