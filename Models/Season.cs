
namespace UHC_API.Models
{
    public class Season
    {
        public Season()
        {
            Connections = new HashSet<Connection>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }

        public DateTime DateHeld { get; set; }
        
        public VictoryType? VictoryType { get; set; }

        public ICollection<Connection> Connections { get; set; }
        
        
    }
}
