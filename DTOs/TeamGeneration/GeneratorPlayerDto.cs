namespace UHC_API.DTOs;

public class GeneratorPlayerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Rank { get; set; }
    public int? HighestConnectionInTeam { get; set; }
}