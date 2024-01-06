using FoodTotem.Payment.Gateways.Http;
using FoodTotem.Payment.Gateways.MercadoPago.ViewModels;
using Microsoft.Extensions.Configuration;

namespace FoodTotem.Payment.Gateways.MercadoPago
{
    public class MercadoPagoPaymentService : IMercadoPagoPaymentService
    {
        private readonly IHttpHandler _httpHandler;

        private readonly string ACCESS_TOKEN;
        private readonly string EXTERNAL_POS_ID;
        private readonly string USER_ID;
        private readonly string BASE_URL;

        public MercadoPagoPaymentService(IHttpHandler httpHandler, IConfiguration configuration)
        {
            _httpHandler = httpHandler;
            
            ACCESS_TOKEN = configuration["AccessToken"];
            EXTERNAL_POS_ID = configuration["ExternalPosId"];
            USER_ID = configuration["UserId"];
            BASE_URL = configuration["BaseUrl"];
        }

        public Task<QRCodeViewModel> GetPaymentQRCode(PaymentInformationViewModel paymentInformationViewModel)
        {
            var uri = $"{BASE_URL}/instore/orders/qr/seller/collectors/{USER_ID}/pos/{EXTERNAL_POS_ID}/qrs";
            return _httpHandler.PutAsync<PaymentInformationViewModel,QRCodeViewModel>(uri, paymentInformationViewModel, MountMercadoPagoHeader());
        }

        private Dictionary<string, string> MountMercadoPagoHeader()
        {
            return new Dictionary<string, string>()
            {
                { "Authorization", ACCESS_TOKEN }
            };
        }
    }
}