namespace FoodTotem.Domain.Core
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        { }
    }
}