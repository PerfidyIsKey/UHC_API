namespace UHC_API.DTOs;

public class TeamDto
{
    public string Name { get; set; }
    public Color Color { get; set; }
    public int Size { get; set; }
    
    public int Diff { get; set; }
    public int AverageRank { get; set; }
    public List<Player> Players { get; set; }
}