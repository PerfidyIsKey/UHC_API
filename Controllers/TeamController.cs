using UHC_API.DTOs;
using UHC_API.HelperClasses;
using UHC_API.TeamGeneration;
using UHC_API.TeamGeneration.Implementations;
using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public List<TeamDto> GetTeams()
        {
            return ConvertTeamsToDtOs(StartTeamGeneration(null));
        }

        [HttpPost]
        [Route("{teamSize:int}")]
        public List<TeamDto> GetTeams(int teamSize)
        {
            return ConvertTeamsToDtOs(StartTeamGeneration(teamSize));
        }

        private List<Team> StartTeamGeneration(int? teamSize)
        {
            var seasonId = _context.Seasons.Max(season => season.Id);
            IPlayerPicker playerPicker = new SeasonPlayerPicker(_context, seasonId);
            var playerCount = playerPicker.PickPlayers().Count;

            ITeamInitialisationPicker teamInitialisationPicker;

            if (teamSize != null)
            {
                teamInitialisationPicker = new ManualTeamInitialisationPicker(seasonId,playerCount, (int)teamSize);
            }
            else
            {
                teamInitialisationPicker = new CalculatedTeamInitialisationPicker(seasonId,playerCount);
            }

            ITeamGenerator teamGenerator = new BruteForceTeamGenerator(_context, teamInitialisationPicker);
            ITeamGeneratorIterator iterator = new ThreadedTeamGeneratorIterator();
            return iterator.Iterate(teamGenerator, 10000);
        }

        private static List<TeamDto> ConvertTeamsToDtOs(IReadOnlyCollection<Team> teams)
        {
            var averageTeamRank = (int)teams.Average(team => team.AverageRank);

            return teams.Select(team => ConvertTeamToDto(team, averageTeamRank)).ToList();
        }

        private static TeamDto ConvertTeamToDto(Team team, int averageTeamRank)
        {
            return new TeamDto
            {
                Color = team.Color,
                Name = team.Name,
                Players = ConvertPlayersToDto(team.Players, team),
                AverageRank = team.AverageRank,
                Size = team.Size,
                Diff = team.AverageRank - averageTeamRank
            };
        }

        private static List<GeneratorPlayerDto> ConvertPlayersToDto(IEnumerable<Player> players, Team team)
        {
            return players.Select(player => ConvertPlayerToDto(player, team)).ToList();
        }

        private static GeneratorPlayerDto ConvertPlayerToDto(Player player, Team team)
        {
            return new GeneratorPlayerDto()
            {
                Id = player.Id,
                Name = player.Name,
                Rank = player.Rank,
                HighestConnectionInTeam =
                    Assessor.HighestTimesPlayedWithAnyone(player, Assessor.GetListWithoutPlayer(player, team.Players))
            };
        }
    }
}