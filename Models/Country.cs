namespace E_Technology_Task.Models
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class BlockedCountry : Country
    {
        public DateTime? BlockedUntil { get; set; }
    }
}
