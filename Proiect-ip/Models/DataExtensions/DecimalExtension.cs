namespace Proiect_ip.Models.DataExtensions
{
    public static class DecimalExtensions
    {
        public static decimal TruncateTo(this decimal value, int decimals)
        {
            decimal factor = (decimal)Math.Pow(10, decimals);
            return Math.Truncate(value * factor) / factor;
        }
    }
}
