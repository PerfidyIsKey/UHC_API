namespace UHC_API.TeamGeneration.Interfaces;

public interface ITeamMatePicker
{
    public Player PickTeamMate(List<Player> playersToMatch, List<Player> availablePlayers);
}