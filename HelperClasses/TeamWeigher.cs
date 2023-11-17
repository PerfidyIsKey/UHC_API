namespace UHC_API.HelperClasses;

public static class TeamWeigher
{
    public static List<Team> Weigh(List<Team> teams1, List<Team> teams2)
    {
        var teamDiff1 = DiffCalculator(teams1);
        var teamDiff2 = DiffCalculator(teams2);
        return teamDiff1 <= teamDiff2 ? teams1 : teams2;
    }

    private static double DiffCalculator(List<Team> teams)
    {
        var teamsAverage = teams.Average(t => t.AverageRank);
        var teamsHigh = teams.Max(t => t.AverageRank);
        var teamsLow = teams.Min(t => t.AverageRank);
        var diffHigh = teamsHigh - teamsAverage;
        var diffLow = teamsAverage - teamsLow;
        return diffHigh >= diffLow ? diffHigh : diffLow;
    }
}