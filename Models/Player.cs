


namespace UHC_API.Models
{
    public class Player
    {
        public Player()
        {
            Connections = new HashSet<Connection>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? DiscordId { get; set; }

        public int? Rank { get; set; }

        public ICollection<Connection> Connections { get; set; }
    }
}
