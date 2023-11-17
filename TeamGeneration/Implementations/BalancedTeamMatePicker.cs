using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class BalancedTeamMatePicker : ITeamMatePicker
{
    private readonly int _averagePlayerRank;
    private readonly int _teamSize;
    
    public BalancedTeamMatePicker(int averagePlayerRank, int teamSize)
    {
        _averagePlayerRank = averagePlayerRank;
        _teamSize = teamSize;
    }
    
    public Player PickTeamMate(List<Player> playersToMatch, List<Player> availablePlayers)
    {
        var potentialCandidates = FilterPotentialCandidatesByRank(playersToMatch, availablePlayers, _averagePlayerRank, _teamSize);
        potentialCandidates = FilterPotentialCandidatesByTimesPlayedTogether(playersToMatch, potentialCandidates);
        return potentialCandidates.First();
    }

    private static List<Player> FilterPotentialCandidatesByRank(List<Player> playersToMatch, List<Player> potentialCandidates, int averagePlayerRank, int teamSize)
    {
        if (HasOneCandidate(potentialCandidates)) return potentialCandidates;
        var players = potentialCandidates.Where(potentialTeamMate => Assessor.AssessRank(playersToMatch, potentialTeamMate, averagePlayerRank, teamSize)).ToList();
        return players.Any() ? players : potentialCandidates;
    }
    
    private static List<Player> FilterPotentialCandidatesByTimesPlayedTogether(List<Player> playersToMatch, List<Player> potentialCandidates)
    {
        if (HasOneCandidate(potentialCandidates)) return potentialCandidates;
        var players = potentialCandidates.Where(potentialTeamMate => Assessor.AssessTimesPlayedTogether(playersToMatch, potentialTeamMate)).ToList();
        return players.Any() ? players : potentialCandidates;
    }

    private static bool HasOneCandidate(IEnumerable<Player> players)
    {
        return players.Count() == 1;
    }
}