using System.ComponentModel.DataAnnotations.Schema;

namespace UHC_API.Models;

[Table("Connections")]
public class Connection
{
    public int PlayerId { get; set; }
    [ForeignKey(nameof(PlayerId))] 
    public Player? Player { get; set; }
    
    public int SeasonId { get; set; }
    [ForeignKey(nameof(SeasonId))] 
    public Season? Season { get; set; }
    
    public Color? TeamColor { get; set; }

    public int? TeamNumber { get; set; }
    public int? Kills { get; set; }
    public bool? HasWon { get; set; }
    public bool? IsTraitor { get; set; }
    public int? Position { get; set; }
    public bool? IsIronMan { get; set; }
    public int? ApplesEaten { get; set; }
    public int? BlocksMined { get; set; }
    
    public TimeKilled? TimeKilled { get; set; }
    
    //KilledBy if player "{id}" if else then name
    public string? KilledBy { get; set; }
    
    public bool? KilledFirst { get; set; }
    public bool? KilledLast{ get; set; }
    
}