using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class BalancedTeamMatePicker : ITeamMatePicker
{
    private readonly int _averagePlayerRank;
    private readonly int _teamSize;
    private readonly List<Player> _allPlayers;
    
    public BalancedTeamMatePicker(int averagePlayerRank, int teamSize, List<Player> allPlayers)
    {
        _averagePlayerRank = averagePlayerRank;
        _teamSize = teamSize;
        _allPlayers = allPlayers;
    }
    
    public Player PickTeamMate(List<Player> playersToMatch, List<Player> potentialCandidates)
    {
        potentialCandidates = FilterPotentialCandidatesByTimesPlayedTogether(playersToMatch, potentialCandidates, _allPlayers);
        potentialCandidates = FilterPotentialCandidatesByRank(playersToMatch, potentialCandidates, _averagePlayerRank, _teamSize);
        
        return potentialCandidates.First();
    }

    private static List<Player> FilterPotentialCandidatesByRank(List<Player> playersToMatch, List<Player> potentialCandidates, int averagePlayerRank, int teamSize)
    {
        if (HasOneCandidate(potentialCandidates)) return potentialCandidates;
        var players = potentialCandidates.Where(potentialTeamMate => Assessor.AssessRank(playersToMatch, potentialTeamMate, averagePlayerRank, teamSize)).ToList();
        return players.Any() ? players : potentialCandidates;
    }
    
    private static List<Player> FilterPotentialCandidatesByTimesPlayedTogether(List<Player> playersToMatch, List<Player> potentialCandidates, List<Player> allPlayers)
    {
        if (HasOneCandidate(potentialCandidates)) return potentialCandidates;
        var players = potentialCandidates.Where(potentialTeamMate => Assessor.AssessTimesPlayedTogether(playersToMatch, potentialTeamMate, allPlayers)).ToList();
        return players.Any() ? players : potentialCandidates;
    }

    private static bool HasOneCandidate(IEnumerable<Player> players)
    {
        return players.Count() == 1;
    }
}