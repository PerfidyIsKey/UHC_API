using Microsoft.IdentityModel.Tokens;
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

    public BruteForceTeamGenerator(ApplicationDbContext context, ITeamInitialisationPicker teamInitialisationPicker)
    {
        _context = context;
        var query = _context.Players.Where(player => player.Connections.Any(connection => connection.SeasonId == teamInitialisationPicker.GetSeasonId()));
        _players = query.ToList();
        _averagePlayerRank = (int) query.Average(player => player.Rank ?? 0);
        _teamSize = teamInitialisationPicker.PickSize();
        _teamAmount = teamInitialisationPicker.PickAmount();

        _nameList = _nameList.OrderBy(x => Random.Shared.Next()).ToList();
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

    private readonly List<string> _colorNameList = new ()
    {
        "Yellow", "Blue", "Red", "Purple", "Green", "Pink", "Black", "Orange", "Gray", "Aqua", "Dark_Red", "Dark_Blue",
        "Dark_Aqua"
    };

    private readonly List<string> _nameList = new ()
    {
        "Buffalo's",
        "Robots",
        "Veterans",
        "Players",
        "Barons",
        "Cats",
        "Dogs",
        "Lions",
        "Trains",
        "Friends",
        "Bubbles",
        "Programmers",
        "Fingers",
        "Noobs",
        "Moms",
        "Husbands",
        "Kids",
        "Singers",
        "Archers",
        "Fighters",
        "Tanks",
        "Therapists",
        "Politicians",
        "Farmers",
        "Brotherhood",
        "Gang",
        "Mice",
        "Forks",
        "Astronauts"
    };

    private Team? GenerateNewTeam(int teamNumber, List<Player> players)
    {
        var playersInTeam = GeneratePlayerListForTeam(players);
        if (playersInTeam.IsNullOrEmpty()) return null;
        var newTeam = new Team
        {
            Players = playersInTeam,
            Color = (Color)teamNumber,
            Name = _colorNameList[teamNumber] + " " + _nameList[teamNumber],
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
        ITeamMatePicker picker = new BalancedTeamMatePicker(_averagePlayerRank, _teamAmount);
        return picker.PickTeamMate(playersToMatch, availablePlayers);
    }
}