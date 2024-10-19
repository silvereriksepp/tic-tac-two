using GameBrain;

namespace ConsoleUI;

public static class Visualiser 
{
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        int middleStartX = gameInstance._currentGridCenter.x - 1;
        int middleEndX = gameInstance._currentGridCenter.x + 1;
        int middleStartY = gameInstance._currentGridCenter.y - 1;
        int middleEndY = gameInstance._currentGridCenter.y + 1;

        for (var y = 0; y < gameInstance.DimY; y++)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if (x >= middleStartX && x <= middleEndX && y >= middleStartY && y <= middleEndY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }
                
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x, y]) + " ");
                
                Console.ResetColor();
                
                if (x == gameInstance.DimX - 1) continue;
                
                // Color the vertical grid lines if part of the 3x3 grid
                if (x >= middleStartX && x < middleEndX && y >= middleStartY && y <= middleEndY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }
                
                Console.Write("|");
            }
            Console.WriteLine();
            if (y != gameInstance.DimY - 1)
            {
                for (var x = 0; x < gameInstance.DimX; x++)
                {
                    // Color the horizontal grid lines if part of the 3x3 grid
                    if (x >= middleStartX && x <= middleEndX && y >= middleStartY && y < middleEndY)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ResetColor();
                    }
                    
                    Console.Write("---");
                    
                    Console.ResetColor();
                    
                    if (x != gameInstance.DimX - 1)
                    {
                        // Color the intersection grid lines if part of the 3x3 grid
                        if (x >= middleStartX && x < middleEndX && y >= middleStartY && y < middleEndY)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ResetColor();
                        }
                        
                        Console.Write("+");
                    }
                }
                Console.WriteLine();
            }
        }
        Console.WriteLine($"Current player: {gameInstance._currentPlayer}");
    }
    
    private static string DrawGamePiece(EGamePiece piece)
    {
        return piece switch
        {
            EGamePiece.X => "X",
            EGamePiece.O => "O",
            _ => " "
        };
    }
}

