using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class CalculatedTeamInitialisationPicker : ITeamInitialisationPicker
{
    private readonly int _seasonId;
    private readonly int _amountOfPlayers;
    private const int MaximumAmountOfTeams = 13;
    private readonly int _teamSize;
    private readonly int _teamAmount;

    public CalculatedTeamInitialisationPicker(int seasonId, int amountOfPlayers)
    {
        _seasonId = seasonId;
        _amountOfPlayers = amountOfPlayers;
        _teamSize = ModifiedPicker(Picker());
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

    private int Picker()
    {
        if (_amountOfPlayers > 7)
        {
            switch (_amountOfPlayers % 3)
            {
                case 1:
                case 0:
                    return 3;
            }
        }

        switch (_amountOfPlayers % 2)
        {
            case 1:
            case 0:
                return 2;
            default:
                return 2;
        }
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