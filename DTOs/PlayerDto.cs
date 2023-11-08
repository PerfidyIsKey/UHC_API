namespace UHC_API.DTOs;

public class PlayerDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? DiscordId { get; set; }
    public int? Rank { get; set; }
    public int? TimesPlayed { get; set; }
    
    public int? TimesWon { get; set; }
    
    public int? TimesBeenTraitor { get; set; }
}