using UHC_API.TeamGeneration;

namespace UHC_API.HelperClasses;

public static class TeamWeigher
{
    public static List<Team> Weigh(List<Team> teams1, List<Team> teams2)
    {
        var weighedRank = WeighRank(teams1, teams2);
        var weighedTimesPlayedTogether = WeighTimesPlayedTogether(teams1, teams2);
        if (weighedRank && weighedTimesPlayedTogether) return teams1;
        if (!weighedRank && !weighedTimesPlayedTogether)return teams2;
        
        return weighedRank ? teams1 : teams2;
    }

    private static bool WeighRank(IReadOnlyCollection<Team> teams1, IReadOnlyCollection<Team> teams2)
    {
        return DiffCalculator(teams1) <= DiffCalculator(teams2);
    }

    private static bool WeighTimesPlayedTogether(IEnumerable<Team> teams1, IEnumerable<Team> teams2)
    {
        return HighestConnection(teams1) <= HighestConnection(teams2);
    }

    private static int HighestConnection(IEnumerable<Team> teams)
    {
        return teams.Max(HighestConnection);
    }

    private static int HighestConnection(Team team)
    {
        return team.Players.Max(player => Assessor.HighestTimesPlayedWithAnyone(player, GetListWithoutPlayer(player, team.Players)));
    }

    private static IEnumerable<Player> GetListWithoutPlayer(Player player, IEnumerable<Player> allPlayers)
    {
        var players = new List<Player>(allPlayers);
        players.Remove(player);
        return players;
    }

    private static double DiffCalculator(IReadOnlyCollection<Team> teams)
    {
        var teamsAverage = teams.Average(t => t.AverageRank);
        var teamsHigh = teams.Max(t => t.AverageRank);
        var teamsLow = teams.Min(t => t.AverageRank);
        var diffHigh = teamsHigh - teamsAverage;
        var diffLow = teamsAverage - teamsLow;
        return diffHigh >= diffLow ? diffHigh : diffLow;
    }
}