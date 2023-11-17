using UHC_API.DTOs;

namespace UHC_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetPlayers")]
        [Route("")]
        public async Task<IEnumerable<PlayerDto>?> GetAll()
        {
            var query = CreateQuery();
            var result = await query.ToListAsync();
            return PlayersToDto(result);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ObjectResult> Get(int id)
        {
            var query = CreateQueryExtended();
            var player = query.SingleOrDefault(player => player.Id == id);
            if (player == null) return NotFound(null);
            return Ok(await PlayerToDto(player));
        }

        [HttpPost(Name = "PostPlayer")]
        [Route("")]
        public async Task<ObjectResult> Post(Player player)
        {
            if (player.Name == null || player.DiscordId == null) return BadRequest(player);
            var newPlayer = CreateNewPlayer(player);

            await _context.Players.AddAsync(newPlayer);
            await _context.SaveChangesAsync();
            return Ok(newPlayer);
        }
        
        [HttpPost]
        [Route("many")]
        public async Task<ObjectResult> PostMany(List<Player> players)
        {
            var newPlayers = players.Select(CreateNewPlayer).ToList();
            foreach (var newPlayer in newPlayers)
            {
                await _context.Players.AddAsync(newPlayer);
            }
            await _context.SaveChangesAsync();
            return Ok(newPlayers);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ObjectResult> Update(Player player)
        {
            var updatedPlayer = UpdatePlayer(player);
            if (updatedPlayer == null) return NotFound(null);
            await _context.SaveChangesAsync();
            return Ok(updatedPlayer);
        }

        [HttpPost]
        [Route("updates")]
        public async Task<ObjectResult> UpdateMany(List<Player> players)
        {
            foreach (var player in players)
            {
                UpdatePlayer(player);
                
            }
            await _context.SaveChangesAsync();
            return Ok(players);
        }

        private Player? UpdatePlayer(Player player)
        {
            var existingPlayer = GetPlayerById(player.Id);

            if (existingPlayer == null) return null;
            existingPlayer.Name = player.Name ?? existingPlayer.Name;
            existingPlayer.DiscordId = player.DiscordId ?? existingPlayer.DiscordId;
            existingPlayer.Rank = player.Rank ?? existingPlayer.Rank;
            return existingPlayer;
        }

        private Player? GetPlayerById(int playerId)
        {
            return _context.Players.SingleOrDefault(p => p.Id == playerId);
        }

        private IQueryable<Player> CreateQuery() {
            var query = _context.Players.Include(player => player.Connections)
                .AsQueryable();
            return query;
        }
        
        private IQueryable<Player> CreateQueryExtended() {
            var query = _context.Players.Include(player => player.Connections)
                .ThenInclude(connection => connection.Season)
                .AsQueryable();
            return query;
        }

        private static Player CreateNewPlayer(Player player)
        {
            var newPlayer = new Player
            {
                Name = player.Name,
                DiscordId = player.DiscordId
            };
            return newPlayer;
        }
        
        private IEnumerable<PlayerDto>? PlayersToDto(List<Player> players)
        {
            return players?.Select((player) => PlayerToDto(player).Result);
        }

        private async Task<PlayerDto> PlayerToDto(Player player)
        {
            var dto = new PlayerDto()
            {
                Id = player.Id,
                Name = player.Name,
                DiscordId = player.DiscordId,
                Rank = player.Rank,
                TimesPlayed = player.Connections.Count,
                TimesWon = player.Connections.Count(connection => connection.HasWon == true),
                TimesBeenTraitor = player.Connections.Count(connection => connection.IsTraitor == true)
            };
            return dto;
        }

    }
}