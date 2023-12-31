using FoodTotem.Payment.UseCase.Ports;
using FoodTotem.Payment.Gateways.MercadoPago;
using FoodTotem.Payment.Domain.Models;
using FoodTotem.Payment.Domain.Ports;
using FoodTotem.Payment.Domain.Repositories;
using FoodTotem.Payment.UseCase.InputViewModels;
using FoodTotem.Payment.UseCase.OutputViewModels;
using FoodTotem.Domain.Core;
using FoodTotem.Payment.UseCase.Utils;

namespace FoodTotem.Payment.UseCase.UseCases
{
    public class PaymentUseCases : IPaymentUseCases
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMercadoPagoPaymentService _mercadoPagoPaymentService;

        public PaymentUseCases(IPaymentService paymentService, IPaymentRepository paymentRepository, IMercadoPagoPaymentService mercadoPagoPaymentService)
        {
            _paymentService = paymentService;
            _paymentRepository = paymentRepository;
            _mercadoPagoPaymentService = mercadoPagoPaymentService;
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

            var payment = new Pay(paymentData.in_store_order_id, paymentInfo.expiration_date, paymentData.qr_data, paymentInfo.total_amount);
            
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
                Total = payment.Total
            };

            return paymentViewModel;
        }
    }
}