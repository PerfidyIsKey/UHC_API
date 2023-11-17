namespace UHC_API.TeamGeneration.Interfaces;

public interface ITeamGeneratorIterator
{
    public List<Team> Iterate(ITeamGenerator teamGenerator, int iterations);
}