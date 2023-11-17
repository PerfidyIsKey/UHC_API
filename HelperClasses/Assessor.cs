namespace UHC_API.TeamGeneration;

public static class Assessor
{
    public static bool AssessRank(IEnumerable<Player> playersToMatch, Player potentialTeamMate, int averagePlayerRank,
        int teamSize)
    {
        var rankMargin = 5;
        var averageTeamRank = averagePlayerRank * teamSize;
        var totalRankOfPlayersToMatch = playersToMatch.Sum(player => player.Rank ?? 0);
        var rankDiff = averageTeamRank - totalRankOfPlayersToMatch;
        var lowerBound = rankDiff - rankMargin;
        var upperBound = rankDiff + rankMargin;
        if (lowerBound < 0) lowerBound = 0;
        if (upperBound < 0) upperBound = 0;
        return DetermineIfPotentialTeamMate(lowerBound, upperBound, potentialTeamMate.Rank ?? 0);
    }

    private static bool DetermineIfPotentialTeamMate(int lowerBound, int upperBound, int rankOfPotentialTeamMate)
    {
        return rankOfPotentialTeamMate >= lowerBound && upperBound >= rankOfPotentialTeamMate;
    }

    public static bool AssessTimesPlayedTogether(IEnumerable<Player> playersToMatch, Player potentialTeamMate)
    {
        return true;
    }
}