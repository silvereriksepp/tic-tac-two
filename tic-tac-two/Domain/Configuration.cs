using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Configuration
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }
    public int WindCondition { get; set; }
    public int MovePieceAfterNMoves { get; set; }
    public int InitialGridTopLeftX { get; set; }
    public int InitialGridTopLeftY { get; set; }
    public int MaxGamePieces { get; set; }
    public int GridSize { get; set; }
    
    public ICollection<SaveGame>? SaveGames { get; set; }

    public override string ToString()
    {
        return Id + " " + Name + " (" + BoardWidth + "x" + BoardHeight + ") Games: " 
               + (SaveGames?.Count.ToString() ?? "not joined");
    }
}