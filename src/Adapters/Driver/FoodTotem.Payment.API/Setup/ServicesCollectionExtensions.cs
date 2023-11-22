using FoodTotem.Payment.UseCase.UseCases;
using FoodTotem.Payment.UseCase.Ports;
using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Models.Validators;
using FoodTotem.Payment.Domain.Repositories;
using FoodTotem.Payment.Domain.Ports;
using FoodTotem.Payment.Domain.Services;
using FoodTotem.Payment.Gateways.MongoDB.Repositories;
using FoodTotem.Payment.Gateways.Http;
using FluentValidation;
using FoodTotem.Payment.Gateways.MercadoPago;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServicesColletionExtensions
    {
        public static IServiceCollection AddPaymentServices(
            this IServiceCollection services)
        {
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IPaymentUseCases, PaymentUseCases>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IValidator<Pay>, PayValidator>();

            services.AddScoped<IMercadoPagoPaymentService, MercadoPagoPaymentService>();

            return services;
        }

        public static IServiceCollection AddCommunicationServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpHandler, HttpHandler>();

            return services;
        }
    }
}