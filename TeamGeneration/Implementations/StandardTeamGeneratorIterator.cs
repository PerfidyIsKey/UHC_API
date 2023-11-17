using UHC_API.HelperClasses;
using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class StandardTeamGeneratorIterator : ITeamGeneratorIterator
{
    public List<Team> Iterate(ITeamGenerator teamGenerator, int iterations)
    {
        var teams = teamGenerator.GenerateTeams();
        for (var i = 0; i < iterations; i++)
        {
            teams = TeamWeigher.Weigh(teams, teamGenerator.GenerateTeams());
        }

        return teams;
    }
    
    
}