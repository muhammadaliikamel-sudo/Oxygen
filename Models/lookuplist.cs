namespace Oxygen.Models
{
    public class LookupList
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
        public required string Value { get; set; }
    }
}
