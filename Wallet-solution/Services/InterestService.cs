using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Wallet_solution.Models;

namespace Wallet_solution.Services
{
    public class InterestService
    {
        private readonly IServiceScopeFactory _factory;


        public InterestService(IServiceScopeFactory _factory)
        {
            this._factory = _factory;
        }

        public async Task WalletYealyInterest()
        {

            using (var scope = _factory.CreateScope())
            {
                WalletDbContext context = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

                int index = 0;

                while (true)
                {
                    index++;

                    Console.WriteLine($"batch: {index}");

                    var wallets = await context.Wallets.OrderBy(w => w.AccountNumber).Skip((index - 1) * 10).Take(10).ToListAsync();

                    if (!wallets.Any()) break;

                    SendWalletsForInterest("interestQueue", JsonConvert.SerializeObject(wallets).ToString());

                    Console.WriteLine($"Sent total of {wallets.Count} for processing");

                    Console.WriteLine();
                }


            }

        }

        protected static void SendWalletsForInterest(string queueName, string walletData)
        {
            using (IConnection connetion = new ConnectionFactory().CreateConnection())
            {
                using (IModel channel = connetion.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, false, false, false, null);

                    channel.BasicPublish(string.Empty, queueName, null, Encoding.UTF8.GetBytes(walletData));
                }
            }
        }

        public async Task ReceiveWalletForInterest(string queueName)
        {
            using (IConnection connetion = new ConnectionFactory().CreateConnection())
            {
                using (IModel channel = connetion.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += async (channel, ea) =>
                    {

                        List<Wallet> wallets = JsonConvert
                        .DeserializeObject<List<Wallet>>(Encoding.UTF8.GetString(ea.Body.ToArray()));

                        if (wallets.Any())
                        {
                            foreach (Wallet wallet in wallets)
                            {
                                using (var scope = _factory.CreateScope())
                                {
                                    WalletDbContext context = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

                                    decimal currentBalance = wallet.Balance;

                                    decimal interest = ((currentBalance * 10 * 1) / 100);

                                    wallet.Balance += interest;

                                    context.Wallets.Update(wallet);

                                    context.Transactions.Add(new Transaction
                                    {
                                        UserId = wallet.UserId,
                                        TransactionAmount = interest,
                                        TransactionType = TransactionType.CREDIT,
                                        AccountNumber = wallet.AccountNumber,
                                    });

                                    await context.SaveChangesAsync();
                                }
                            }
                            
                        }

                    };

                    channel.BasicConsume(queueName, autoAck: true, consumer);
                }

            }




        }
    }
}
