namespace ReactiveMicroService.ReportService.API.Enums
{
    public class Enum
    {
        public enum CustomerStatus 
        {
            Unverified =1,
            Verified = 2,
            Paid = 3,
            RefundCancel = 4,
            ChargeBack = 5
        }
    }
}
