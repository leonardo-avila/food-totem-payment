namespace FoodTotem.Payment.UseCase.InputViewModels
{
    public class OrderItemViewModel
    {
        public string ItemId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}