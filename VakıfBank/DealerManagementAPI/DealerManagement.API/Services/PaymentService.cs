using DealerManagement.Data.Context;
using DealerManagement.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace DealerManagement.API.Services
{
    public class PaymentService : BackgroundService
    {
        private readonly DealerManagementDBContext dbContext;
        
        public PaymentService(DealerManagementDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // to complete payment
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<Order> orders = dbContext.Set<Order>().Where(x => x.Status.Equals("Confirmed")).ToList();

                foreach (var order in orders)
                {
                    Payment payment = dbContext.Set<Payment>().FirstOrDefault(x => x.OrderId == order.Id);
                    Account account = dbContext.Set<Account>().Include(x => x.CheckingAccount).FirstOrDefault(x => x.IBAN.Equals(order.SupplierIBAN));
                    account.CheckingAccount.Balance += order.TotalPrice;
                    order.Status = "Paid";
                    dbContext.SaveChanges();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}