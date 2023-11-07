

namespace UHC_API.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Connection>()
                .HasKey(connection => new { connection.PlayerId, connection.SeasonId });

            builder.Entity<Connection>()
                .HasOne(connection => connection.Player)
                .WithMany(player => player.Connections)
                .HasForeignKey(connection => connection.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Connection>()
                .HasOne(connection => connection.Season)
                .WithMany(season => season.Connections)
                .HasForeignKey(connection => connection.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
