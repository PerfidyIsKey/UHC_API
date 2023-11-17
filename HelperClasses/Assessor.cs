namespace UHC_API.TeamGeneration;

public static class Assessor
{
    public static bool AssessRank(IEnumerable<Player> playersToMatch, Player potentialTeamMate, int averagePlayerRank,
        int teamSize)
    {
        const int rankMargin = 5;
        var averageTeamRank = averagePlayerRank * teamSize;
        var totalRankOfPlayersToMatch = playersToMatch.Sum(player => player.Rank ?? 0);
        var rankDiff = averageTeamRank - totalRankOfPlayersToMatch;
        var lowerBound = rankDiff - rankMargin;
        var upperBound = rankDiff + rankMargin;
        if (lowerBound < 0) lowerBound = 0;
        if (upperBound < 0) upperBound = 0;
        return DetermineIfPotentialTeamMate(lowerBound, upperBound, potentialTeamMate.Rank ?? 0);
    }
    
    public static bool AssessTimesPlayedTogether(IEnumerable<Player> playersToMatch, Player potentialTeamMate, IEnumerable<Player> allPlayers)
    {
        var players = GetListWithoutPlayer(potentialTeamMate, allPlayers);
        var highest = HighestTimesPlayedWithAnyone(potentialTeamMate, players);
        return playersToMatch.All(player => TimesPlayedTogether(player, potentialTeamMate) != highest);
    }
    
    public static IEnumerable<Player> GetListWithoutPlayer(Player player, IEnumerable<Player> allPlayers)
    {
        var players = new List<Player>(allPlayers);
        players.Remove(player);
        return players;
    }

    private static bool DetermineIfPotentialTeamMate(int lowerBound, int upperBound, int rankOfPotentialTeamMate)
    {
        return rankOfPotentialTeamMate >= lowerBound && upperBound >= rankOfPotentialTeamMate;
    }
    
    private static int TimesPlayedTogether(Player playerToMatch, Player potentialTeamMate)
    {
        return playerToMatch.Connections.Count(connection => potentialTeamMate.Connections.Any(connection1 => connection.SeasonId == connection1.SeasonId && connection.TeamNumber == connection1.TeamNumber));
    }

    public static int HighestTimesPlayedWithAnyone(Player player, IEnumerable<Player> players)
    {
        return players.Max(p => TimesPlayedTogether(player, p));
    }
}