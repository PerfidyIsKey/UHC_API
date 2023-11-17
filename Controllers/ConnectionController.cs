using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UHC_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConnectionController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("")]
        public List<Connection> Get()
        {
            var query = _context.Connections
                .Include(connection => connection.Player)
                .Include(connection => connection.Season)
                .AsQueryable();
            return query.ToList();
        }
        
        [HttpGet]
        [Route("{seasonId:int}/{playerId:int}")]
        public List<Connection> GetByIDs(int? seasonId, int? playerId)
        {
            var query = _context.Connections
                .AsQueryable();
            if (seasonId != null)
            {
                query = query.Where(connection => connection.SeasonId == seasonId)
                    .Include(connection => connection.Player);
            } 
            if (playerId != null)
            {
                query = query.Where(connection => connection.PlayerId == playerId)
                    .Include(connection => connection.Season);
            }

            return query.ToList();
        }
        
        [HttpPost]
        [Route("")]
        public ObjectResult Post(Connection connection)
        {
            if (FindConnectionByIds(connection.SeasonId, connection.PlayerId) != null)
            {
                return BadRequest("Connection already exists.");
            }

            var newConnection = CreateNewConnection(connection);
            _context.Connections.AddAsync(newConnection);
            _context.SaveChangesAsync();
            return Ok(newConnection);
        }
        
        [HttpPost]
        [Route("many")]
        public ObjectResult PostMany(List<Connection> connections)
        {
            var newConnections = connections.Select(CreateNewConnection).ToList();
            foreach (var newConnection in newConnections)
            {
                _context.Connections.AddAsync(newConnection);
            }
            _context.SaveChangesAsync();
            return Ok(newConnections);
        }

        private Connection? FindConnectionByIds(int seasonId, int playerId)
        {
            return _context.Connections.Where(connection => connection.SeasonId == seasonId)
                .SingleOrDefault(connection => connection.PlayerId == playerId);
        }

        private static Connection CreateNewConnection(Connection connection)
        {
            var newConnection = new Connection
            {
                PlayerId = connection.PlayerId,
                SeasonId = connection.SeasonId,
                TeamColor = connection.TeamColor,
                TeamNumber = connection.TeamNumber,
                Kills = connection.Kills,
                HasWon = connection.HasWon,
                IsTraitor = connection.IsTraitor,
                Position = connection.Position,
                IsIronMan = connection.IsIronMan,
                ApplesEaten = connection.ApplesEaten,
                BlocksMined = connection.BlocksMined,
                TimeKilled = connection.TimeKilled,
                KilledBy = connection.KilledBy,
                KilledFirst = connection.KilledFirst,
                KilledLast = connection.KilledLast
            };
            return newConnection;
        }
    }
}
