namespace CSRMGMT.Models
{
    public class PaymentForm
    {
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string RedirectUrl { get; set; }
        public string CancelUrl { get; set; }
        public string Checksum { get; set; }
    }
}
