using FoodTotem.Payment.UseCase.Ports;
using FoodTotem.Payment.Gateways.MercadoPago;
using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Ports;
using FoodTotem.Payment.Domain.Repositories;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Domain.Core;
using FoodTotem.Payment.UseCase.Utils;
using FoodTotem.Payment.Domain;
using System.Text.Json;

namespace FoodTotem.Payment.UseCase.UseCases
{
    public class PaymentUseCases : IPaymentUseCases
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMercadoPagoPaymentService _mercadoPagoPaymentService;

        private readonly IMessenger _messenger;

        public PaymentUseCases(IPaymentService paymentService, IPaymentRepository paymentRepository, IMercadoPagoPaymentService mercadoPagoPaymentService, IMessenger messenger)
        {
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
            _mercadoPagoPaymentService = mercadoPagoPaymentService;
            _messenger = messenger;
        }

        public async Task<PaymentViewModel> GetPaymentByOrderReference(string orderReference)
        {
            var payment = await _paymentRepository.GetPayment(orderReference) ?? throw new DomainException("Payment not found");
            
            var paymentViewModel = new PaymentViewModel
            {
                Id = payment.Id.ToString(),
                OrderReference = payment.OrderReference,
                ExpirationDate = payment.ExpirationDate,
                QRCode = payment.QRCode,
                Total = payment.Total
            };

            return paymentViewModel;
        }

        public async Task<IEnumerable<PaymentViewModel>> GetPayments()
        {
            var payments = await _paymentRepository.GetAll();

            var paymentViewModels = new List<PaymentViewModel>();

            foreach (var payment in payments)
            {
                paymentViewModels.Add(new PaymentViewModel
                {
                    Id = payment.Id.ToString(),
                    OrderReference = payment.OrderReference,
                    ExpirationDate = payment.ExpirationDate,
                    QRCode = payment.QRCode,
                    Total = payment.Total
                });
            }

            return paymentViewModels;
        }
        public async Task<PaymentViewModel> CreatePayment(OrderViewModel order)
        {
            
            var paymentInfo = PaymentUtils.ProducePaymentInformationViewModel(order);

            var paymentData = await _mercadoPagoPaymentService.GetPaymentQRCode(paymentInfo);

            var payment = new Pay(order.OrderReference, paymentInfo.expiration_date, paymentData.qr_data, paymentInfo.total_amount);
            
            var validPayment = _paymentService.IsValidPayment(payment);

            if (!validPayment)
            {
                throw new DomainException("Invalid payment");
            }

            var succesfullySaved = await _paymentRepository.SavePayment(payment);

            if (!succesfullySaved)
            {
                throw new DomainException("An error occurred while saving the payment");
            }

            var paymentViewModel = new PaymentViewModel
            {
                Id = payment.Id.ToString(),
                OrderReference = payment.OrderReference,
                ExpirationDate = payment.ExpirationDate,
                QRCode = payment.QRCode,
                Total = payment.Total,
                Status = payment.Status.ToString()
            };

            return paymentViewModel;
        }

        public async Task<PaymentViewModel> UpdatePaymentStatus(PaymentStatusViewModel paymentStatus)
        {
            var payment = await _paymentRepository.Get(paymentStatus.Id) ?? throw new DomainException("Payment not found");

            var status = paymentStatus.IsApproved ? PaymentStatus.Paid : PaymentStatus.Cancelled;

            payment.SetStatus(status);

            await _paymentRepository.Update(payment);

            var paymentViewModel = new PaymentViewModel
            {
                Id = payment.Id.ToString(),
                OrderReference = payment.OrderReference,
                ExpirationDate = payment.ExpirationDate,
                QRCode = payment.QRCode,
                Total = payment.Total,
                Status = status.ToString()
            };

            if (status == PaymentStatus.Paid)
            {
                _messenger.Send(JsonSerializer.Serialize(paymentViewModel), "payment-paid-event");
            }
            else {
                _messenger.Send(JsonSerializer.Serialize(paymentViewModel), "payment-cancelled-event");
            }

            return paymentViewModel;
        }
    }
}