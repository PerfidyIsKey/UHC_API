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
        public List<Player> GetAll()
        {
            var query = CreateQuery();
            return query.ToList();
        }
        
        [HttpGet]
        [Route("{id}")]
        public ObjectResult Get(int id)
        {
            var query = CreateQuery();
            var player = query.SingleOrDefault(player => player.Id == id);
            if (player == null) return NotFound(null);
            return Ok(player);
        }

        [HttpPost(Name = "PostPlayer")]
        [Route("")]
        public ObjectResult Post(Player player)
        {
            if (player.Name == null || player.DiscordId == null) return BadRequest(player);
            var newPlayer = CreateNewPlayer(player);

            _context.Players.AddAsync(newPlayer);
            _context.SaveChangesAsync();
            return Ok(newPlayer);
        }
        
        [HttpPost]
        [Route("many")]
        public ObjectResult PostMany(List<Player> players)
        {
            var newPlayers = players.Select(CreateNewPlayer).ToList();
            foreach (var newPlayer in newPlayers)
            {
                _context.Players.AddAsync(newPlayer);
            }
            _context.SaveChangesAsync();
            return Ok(newPlayers);
        }

        [HttpPost]
        [Route("update")]
        public ObjectResult Update(Player player)
        {
            var updatedPlayer = UpdatePlayer(player);
            if (updatedPlayer == null) return NotFound(null);
            _context.SaveChangesAsync();
            return Ok(updatedPlayer);
        }

        [HttpPost]
        [Route("updates")]
        public ObjectResult UpdateMany(List<Player> players)
        {
            foreach (var player in players)
            {
                UpdatePlayer(player);
                
            }
            _context.SaveChangesAsync();
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
                .ThenInclude(connection => connection.Season)
                .Include(player => player.Connections)
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
    }
}