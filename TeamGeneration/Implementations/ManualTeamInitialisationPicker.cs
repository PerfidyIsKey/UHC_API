using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class ManualTeamInitialisationPicker : ITeamInitialisationPicker
{
    private readonly int _seasonId;
    private readonly int _amountOfPlayers;
    private const int MaximumAmountOfTeams = 13;
    private readonly int _teamSize;
    private readonly int _teamAmount;
    
    public ManualTeamInitialisationPicker(int seasonId, int playerAmount, int desiredTeamSize)
    {
        _seasonId = seasonId;
        _amountOfPlayers = playerAmount;
        _teamSize = ModifiedPicker(desiredTeamSize);
        _teamAmount = _amountOfPlayers / _teamSize;
    }
    
    public int PickSize()
    {
        return _teamSize;
    }

    public int PickAmount()
    {
        return _teamAmount;
    }

    public int GetSeasonId()
    {
        return _seasonId;
    }
    
    private int ModifiedPicker(int currentAmount)
    {
        return IsValidTeamSize(currentAmount) ? currentAmount : ModifiedPicker(currentAmount + 1);
    }
    
    private bool IsValidTeamSize(int teamSize)
    {
        return _amountOfPlayers / teamSize <= MaximumAmountOfTeams;
    }
}