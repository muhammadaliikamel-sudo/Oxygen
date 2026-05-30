namespace Oxygen.DTOs
{
    public class UpdateSettingsRequestDto
    {
        public List<SettingUpdateDto> Settings { get; set; } = new();
    }
}
