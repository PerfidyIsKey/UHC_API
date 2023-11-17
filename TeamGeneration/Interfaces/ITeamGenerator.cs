using UHC_API.DTOs;
using UHC_API.HelperModels;

namespace UHC_API.TeamGeneration.Interfaces;

public interface ITeamGenerator
{
    public List<Team> GenerateTeams();
}