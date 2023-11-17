namespace UHC_API.HelperModels;

public class Team
{
    public List<Player> Players { get; set; }
    public string Name { get; set; } //generate using data? (Blue Jet-ski's, Red Panthers, etc..)
    public Color Color { get; set; }
    
    public int TeamSize { get; set; }
    
    public int Size { get; set; }
    
    public int AverageRank { get; set; }
}