namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece CurrentPlayer { get; set; } = EGamePiece.X;

    public GameConfiguration GameConfiguration { get; set; }
    
    public string XPass { get; set; } = "x";

    public string OPass { get; set; } = "o";

    public string Name { get; set; } = default!;

    public bool Ai { get; set; } = default!;

    public GameState(EGamePiece[][] gameBoard, GameConfiguration gameConfiguration)
    {
        GameBoard = gameBoard;
        GameConfiguration = gameConfiguration;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}