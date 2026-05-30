namespace Oxygen.Models
{
    public class Setting
    {
        public Guid Id { get; set; }
        public required string SettingKey { get; set; }
        public required string SettingValue { get; set; }
    }
}
