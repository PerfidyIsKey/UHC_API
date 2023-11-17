using Microsoft.IdentityModel.Tokens;
using UHC_API.Constants;
using UHC_API.HelperClasses;
using UHC_API.TeamGeneration.Interfaces;

namespace UHC_API.TeamGeneration.Implementations;

public class BruteForceTeamGenerator : ITeamGenerator
{
    private readonly ApplicationDbContext _context;
    private readonly List<Player> _players;
    private readonly int _averagePlayerRank;
    private readonly int _teamSize;
    private readonly int _teamAmount;
    private readonly List<string> _nameList;

    public BruteForceTeamGenerator(ApplicationDbContext context, ITeamInitialisationPicker teamInitialisationPicker)
    {
        _context = context;
        var query = _context.Players.Where(player => player.Connections.Any(connection => connection.SeasonId == teamInitialisationPicker.GetSeasonId()));
        query = query.Include(player => player.Connections);
        _players = query.ToList();
        _averagePlayerRank = (int) query.Average(player => player.Rank ?? 0);
        _teamSize = teamInitialisationPicker.PickSize();
        _teamAmount = teamInitialisationPicker.PickAmount();
        _nameList = NamingConstants.NameList.OrderBy(x => Random.Shared.Next()).ToList();
    }

    public List<Team> GenerateTeams()
    {
        var teams = new List<Team>();
        var players = new List<Player>(_players);
        for (var i = 0; i < _teamAmount; i++)
        {
            players = TeamGeneratorHelper.ExcludeTeamsFromPlayerList(players, teams);
            var team = GenerateNewTeam(i, players);
            if(team != null) teams.Add(team);
        }

        players = TeamGeneratorHelper.ExcludeTeamsFromPlayerList(players, teams);
        while (players.Any() && players.Count < _teamSize)
        {
            var team = teams.MinBy(team => team.Players.Average(player => player.Rank ?? 0));
            if (team != null)
            {
                team.Players.Add(players.MaxBy(player => player.Rank ?? 0) ?? throw new InvalidOperationException());
                team.AverageRank = team.Players.Sum(player => player.Rank ?? 0) / _teamSize + 15;
                team.Size++;
            }
            players = TeamGeneratorHelper.ExcludeTeamsFromPlayerList(players, teams);
        }

        return teams;
    }

    private Team? GenerateNewTeam(int teamNumber, List<Player> players)
    {
        var playersInTeam = GeneratePlayerListForTeam(players);
        if (playersInTeam.IsNullOrEmpty()) return null;
        var newTeam = new Team
        {
            Players = playersInTeam,
            Color = (Color)teamNumber,
            Name = NamingConstants.ColorNameList[teamNumber] + " " + _nameList[teamNumber],
            AverageRank = playersInTeam.Sum(player => player.Rank ?? 0) / _teamSize,
            Size = playersInTeam.Count,
            TeamSize = _teamSize
        };
        return newTeam;
    }

    private List<Player>? GeneratePlayerListForTeam(List<Player> availablePlayers)
    {
        availablePlayers = availablePlayers.OrderBy(x => Random.Shared.Next()).ToList();
        var players = new List<Player>();
        if (!availablePlayers.Any()) return null;
        var player = availablePlayers[0];
        players.Add(player);
        for (var i = 0; i < _teamSize - 1; i++)
        {
            availablePlayers = TeamGeneratorHelper.ExcludePlayersFromPlayerList(availablePlayers, players);
            availablePlayers = availablePlayers.OrderBy(x => Random.Shared.Next()).ToList();
            players.Add(PickTeamMate(players, availablePlayers));
        }

        return players;
    }

    private Player PickTeamMate(List<Player> playersToMatch, List<Player> availablePlayers)
    {
        ITeamMatePicker picker = new BalancedTeamMatePicker(_averagePlayerRank, _teamAmount, _players);
        return picker.PickTeamMate(playersToMatch, availablePlayers);
    }
}