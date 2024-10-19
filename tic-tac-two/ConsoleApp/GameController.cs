using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    
    private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryHardcoded();
    
    public static string MainLoop()
    {
        var chosenConfigShortcut = ChooseConfiguration();

        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            return chosenConfigShortcut;
        }

        var chosenConfig = ConfigRepository
            .GetConfigurationByName(ConfigRepository.GetConfigurationNames()[configNo]);
    
        var gameInstance = new TicTacTwoBrain(chosenConfig);

        do
        {
            
            Visualiser.DrawBoard(gameInstance);

            Console.Write("Write first word + required coordinates --- example: move 1,1 --- Allowed moves are:");
            Console.WriteLine();
            if (gameInstance._xPiecesPlaced >= chosenConfig.MaxGamePieces || gameInstance._oPiecesPlaced >= chosenConfig.MaxGamePieces)
            {
                Console.Write("Move: <fromX,fromY,toX,toY> | Shift Grid (middle square coordinates): <x,y> | Exit ---YOUR INPUT:");
            }
            else if (gameInstance._xPiecesPlaced == 0 || gameInstance._oPiecesPlaced == 0)
            {
                Console.Write("Place: <x,y> | Shift Grid (middle square coordinates): <x,y> | Exit ---YOUR INPUT:");
            } else
            {
                Console.Write("Place: <x,y> | Move: <fromX,fromY,toX,toY> | Shift Grid (middle square coordinates): <x,y> | Exit ---YOUR INPUT:");
            }
            
            var input = Console.ReadLine()!.ToLower();
            
            if (input == "exit") break;
            
            if (input.StartsWith("place"))
            {
                var coords = input.Split(' ')[1].Split(',');
                var x = int.Parse(coords[0]);
                var y = int.Parse(coords[1]);
                if (gameInstance.MakeAMove(x, y))
                {
                    Visualiser.DrawBoard(gameInstance);
                }
            }
            else if (input.StartsWith("move"))
            {
                var coords = input.Split(' ')[1].Split(',');
                var fromX = int.Parse(coords[0]);
                var fromY = int.Parse(coords[1]);
                var toX = int.Parse(coords[2]);
                var toY = int.Parse(coords[3]);
                if (gameInstance.MovePiece(fromX, fromY, toX, toY))
                {
                    Visualiser.DrawBoard(gameInstance);
                }
            }
            else if (input.StartsWith("shift"))
            {
                var coords = input.Split(' ')[1].Split(',');
                var offsetX = int.Parse(coords[0]);
                var offsetY = int.Parse(coords[1]);
                if (gameInstance.ShiftGrid(offsetX, offsetY))
                {
                    Visualiser.DrawBoard(gameInstance);
                }
            }
            else
            {
                Console.WriteLine("Invalid input");
            }

        } while (!gameInstance.CheckWin());
        
        Console.WriteLine($"{gameInstance._currentPlayer} won the game!");
        
        return "";
    }

    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = ConfigRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }

        var configMenu = new Menu(EMenuLevel.Secondary,
            "Tic-Tac-Two - Choose game config",
            configMenuItems
        );
    
        return configMenu.Run();
    }

}