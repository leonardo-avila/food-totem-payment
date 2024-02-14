using System.Diagnostics.CodeAnalysis;
using FoodTotem.Domain.Core;

namespace FoodTotem.Payment.Domain.Models
{
    [ExcludeFromCodeCoverage]
    [BsonCollection("pays")]
    public class Pay : Document
    {
        public string OrderReference { get; private set; }
        public string ExpirationDate { get; private set; }
        public string QRCode { get; private set; }
        public double Total { get; private set; }
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;

        public Pay(string orderReference, string expirationDate, string qRCode, double total)
        {
            OrderReference = orderReference;
            ExpirationDate = expirationDate;
            QRCode = qRCode;
            Total = total;
        }

        public void SetStatus(PaymentStatus status)
        {
            Status = status;
        }
    }
}