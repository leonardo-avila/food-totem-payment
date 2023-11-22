namespace FoodTotem.Payment.UseCase.InputViewModels
{
    public class OrderViewModel
    {
        public string OrderReference { get; set; }
        public double Total { get; set; }
        public IEnumerable<OrderItemViewModel> OrderItems { get; set; }
    }
}