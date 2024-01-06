namespace FoodTotem.Payment.UseCase.InputViewModels
{
    [ExcludeFromCodeCoverage]
    public class OrderItemViewModel
    {
        public string ItemId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}