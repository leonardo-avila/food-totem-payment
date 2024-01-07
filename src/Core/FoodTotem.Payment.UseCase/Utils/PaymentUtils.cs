using FoodTotem.Payment.Gateways.MercadoPago.ViewModels;
using FoodTotem.Payment.UseCase.InputViewModels;

namespace FoodTotem.Payment.UseCase.Utils;

public static class PaymentUtils
{
    public static PaymentInformationViewModel ProducePaymentInformationViewModel(OrderViewModel orderViewModel)
    {
        var expiration = DateTimeOffset.Now.AddMinutes(30).ToString("yyyy-MM-ddTHH:mm:ss.fffK");
        return new PaymentInformationViewModel()
        {
            expiration_date = $"{expiration}",
            total_amount = orderViewModel.Total,
            external_reference = orderViewModel.OrderReference,
            title = "Food Totem Order",
            items = ProducePaymentItemViewModelCollection(orderViewModel.OrderItems),
            description = $"Food Totem Order {orderViewModel.OrderReference}"
        };
    }

    private static IEnumerable<PaymentItemViewModel> ProducePaymentItemViewModelCollection(IEnumerable<OrderItemViewModel> orderItemViewModel)
    {
        foreach (var orderItem in orderItemViewModel)
        {
            yield return new PaymentItemViewModel()
            {
                sku_number = orderItem.ItemId,
                unit_measure = "unit",
                unit_price = orderItem.Price,
                quantity = orderItem.Quantity,
                total_amount = orderItem.Price * orderItem.Quantity,
                title = "Food"
            };
        }
    }
}