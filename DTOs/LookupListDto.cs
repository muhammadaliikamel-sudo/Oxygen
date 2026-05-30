namespace Oxygen.DTOs;

public class LookupListDto
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
}
