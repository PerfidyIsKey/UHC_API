using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class SeasonPlayerPicker : IPlayerPicker
{
    private readonly int _seasonId;
    private readonly ApplicationDbContext _context;

    public SeasonPlayerPicker(ApplicationDbContext context, int seasonId)
    {
        _context = context;
        _seasonId = seasonId;
    }
    
    public List<Player> PickPlayers()
    {
        return _context.Players.Where(player => player.Connections.Any(connection => connection.SeasonId == _seasonId)).ToList();
    }

}