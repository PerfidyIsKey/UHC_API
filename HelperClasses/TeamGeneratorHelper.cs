namespace UHC_API.HelperClasses;

public static class TeamGeneratorHelper
{
    public static List<Player> ExcludePlayersFromPlayerList(List<Player> playerList, List<Player> playersToExclude)
    {
        foreach (var player in playersToExclude)  playerList.Remove(player);
        return playerList;
    }

    public static List<Player> ExcludeTeamsFromPlayerList(List<Player> playerList, List<Team> teamList)
    {
        return teamList.Aggregate(playerList, (current, team) => ExcludePlayersFromPlayerList(current, team.Players));
    }
}