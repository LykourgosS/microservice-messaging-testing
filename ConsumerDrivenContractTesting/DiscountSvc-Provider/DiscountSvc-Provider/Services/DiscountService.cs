namespace DiscountSvc_Provider.Services
{
    public class DiscountService
    {
        public double GetDiscountAmount(double customerRating)
        {
            return customerRating / 10;
        }
    }
}
