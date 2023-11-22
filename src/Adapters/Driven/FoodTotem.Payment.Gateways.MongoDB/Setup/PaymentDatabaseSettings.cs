namespace FoodTotem.Payment.Gateways.MongoDB.Setup;

public class PaymentDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}