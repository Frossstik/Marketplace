namespace Marketplace.Payment.Domain.Entities
{
    public class CardPaymentDetails
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
    }
}
