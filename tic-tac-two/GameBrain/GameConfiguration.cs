namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    
    //how many pieces in straight line to win game
    public int WinCondition { get; set; } = 3;
    
    //0 disabled
    public int MovePieceAfterNMoves { get; set; } = 0;
    public int InitialGridTopLeftX { get; set; } = 1;
    public int InitialGridTopLeftY { get; set; } = 1;
    public int MaxGamePieces { get; set; } = 4;
    public int GridSize { get; set; } = 3;


    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, to win: {WinCondition}, can move pieces after: {MovePieceAfterNMoves}," +
        $" Initial grid center: ({InitialGridTopLeftX}, {InitialGridTopLeftY})";
}