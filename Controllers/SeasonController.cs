namespace UHC_API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeasonController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SeasonController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet(Name = "GetSeasons")]
    public List<Season> Get()
    {
        var query = _context.Seasons.Include(season => season.Connections)
            .ThenInclude(connection => connection.Player)
            .Include(season => season.Connections)
            .AsQueryable();
        return query.ToList();
    }
        
    [HttpPost(Name = "PostSeason")]
    [Route("")]
    public ObjectResult Post(Season season)
    {
        if (season.Name == null) return BadRequest(season);
        var newSeason = CreateNewSeason(season);
        
        _context.Seasons.AddAsync(newSeason);
        _context.SaveChangesAsync();
        return Ok(newSeason);
    }
    
    [HttpPost]
    [Route("many")]
    public ObjectResult PostMany(List<Season> seasons)
    {
        var newSeasons = seasons.Select(CreateNewSeason).ToList();
        foreach (var newSeason in newSeasons)
        {
            _context.Seasons.AddAsync(newSeason);
        }
        _context.SaveChangesAsync();
        return Ok(newSeasons);
    }

    private static Season CreateNewSeason(Season season)
    {
        var newSeason = new Season
        {
            Name = season.Name,
            DateHeld = season.DateHeld,
            VictoryType = season.VictoryType
        };
        return newSeason;
    }
}