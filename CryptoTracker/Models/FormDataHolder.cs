namespace CryptoTracker.Models
{
    public class FormDataHolder
    {
        public string SelectedCoin { get; set; } = "bitcoin";
        public string SelectedCurrency { get; set; } = "usd";
        public int SelectedDays { get; set; } = 7;
    }
}
