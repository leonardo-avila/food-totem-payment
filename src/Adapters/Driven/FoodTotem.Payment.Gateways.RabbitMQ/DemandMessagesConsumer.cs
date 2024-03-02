using System.Text;
using System.Text.Json;
using FoodTotem.Payment.Domain;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.Ports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace FoodTotem.Demand.Gateways.RabbitMQ.PaymentMessages
{
    public class DemandMessagesConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IPaymentUseCases? _paymentUseCases;
        public DemandMessagesConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => 
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
                _paymentUseCases = scope.ServiceProvider.GetRequiredService<IPaymentUseCases>();

                messenger.Consume("order-created-event",
                    (e) => ProccessMessage(this, (BasicDeliverEventArgs)e));
            }, stoppingToken);
        }


        private void ProccessMessage(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            var order = JsonSerializer.Deserialize<OrderViewModel>(message);
            _paymentUseCases.CreatePayment(order!);
        }
    }
}