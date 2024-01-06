namespace FoodTotem.Payment.UseCase.InputViewModels
{
    [ExcludeFromCodeCoverage]
    public class OrderViewModel
    {
        public string OrderReference { get; set; }
        public double Total { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}