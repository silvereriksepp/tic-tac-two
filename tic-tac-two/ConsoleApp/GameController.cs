using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public static class GameController
{
    static string connectionString = $"Data Source={FileHelper.BasePath}app.db";

    static DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
    
        static AppDbContext ctx = new(contextOptions);


        private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryDb(ctx);//new ConfigRepositoryJson(); //new ConfigRepositoryDb(ctx); //change between db and json ConfigRepositoryJson()
        private static readonly IGameRepository GameRepository = new GameRepositoryDb(ctx);//new GameRepositoryJson(); //new GameRepositoryDb(ctx); //change between db and json GameRepositoryJson()
    
    public static string MainLoop(TicTacTwoBrain? brain = null)
    {
        TicTacTwoBrain gameInstance;
        GameConfiguration chosenConfig;
        
        if (brain == null)
        {
            var chosenConfigShortcut = ChooseConfiguration();

            if (!int.TryParse(chosenConfigShortcut, out var configNo))
            {
                return chosenConfigShortcut;
            }

            chosenConfig = ConfigRepository
                .GetConfigurationByName(ConfigRepository.GetConfigurationNames()[configNo]);
    
            gameInstance = new TicTacTwoBrain(chosenConfig); 
        }
        else
        {
            chosenConfig = brain.GameConfiguration;
            gameInstance = brain;
        }


        do
        {
            
            Visualiser.DrawBoard(gameInstance);

            Console.Write("Write first word + required coordinates --- example: place 1,1 --- Allowed moves are:");
            Console.WriteLine();
            if (gameInstance._xPiecesPlaced >= chosenConfig.MaxGamePieces || gameInstance._oPiecesPlaced >= chosenConfig.MaxGamePieces)
            {
                Console.Write("Move: <fromX,fromY,toX,toY> | Shift Grid (top-left square coordinates): <x,y> | Save | Exit");
                Console.WriteLine();
                Console.Write("YOUR INPUT:");
            }
            else if (gameInstance._xPiecesPlaced == 0 || gameInstance._oPiecesPlaced == 0)
            {
                Console.Write("Place: <x,y> | Shift Grid (top-left square coordinates): <x,y> | Save | Exit");
                Console.WriteLine();
                Console.Write("YOUR INPUT:");
            } else
            {
                Console.Write("Place: <x,y> | Move: <fromX,fromY,toX,toY> | Shift Grid (top-left square coordinates): <x,y> | Save | Exit");
                Console.WriteLine();
                Console.Write("YOUR INPUT:");
            }
            
            var input = Console.ReadLine()!.ToLower();
            
            if (input == "exit") break;

            if (input == "save")
            {
                GameRepository.SaveGame(gameInstance.GetGameStateJson(), gameInstance.GetConfigName());
            }

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

        if (gameInstance.CheckWin())
        {
            Console.WriteLine($"{gameInstance._gameState.CurrentPlayer} won the game!");   
        }
        
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
    
    
    public static string LoadGame(string filePath)
    {
        try
        {
            var json = File.ReadAllText(filePath);
            var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(json);

            if (gameState != null)
            {
                // Pass the loaded gameState to initialize TicTacTwoBrain and proceed with the game
                var brain = new TicTacTwoBrain(gameState.GameConfiguration)
                {
                    GameBoard = gameState.GameBoard
                };

                Console.WriteLine("Game loaded successfully!");
                MainLoop(brain);
            }

            Console.WriteLine("Failed to load game state.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the game: {ex.Message}");
        }

        return string.Empty;
    }
    
    public static string LoadGameDb(string state)
    {
        var jsonList = ctx.Savegames.Select(save => save.State).ToList();
        string jsonText = "";

        foreach (var json in jsonList)
        {
            if (json == state)
            {
                jsonText = json;
            }
        }

        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(jsonText);

        if (gameState != null)
        {
            // Pass the loaded gameState to initialize TicTacTwoBrain and proceed with the game
            var brain = new TicTacTwoBrain(gameState.GameConfiguration)
            {
                GameBoard = gameState.GameBoard
            };

            Console.WriteLine("Game loaded successfully!");
            MainLoop(brain);
        }
        
        return string.Empty;
    }

    public static void CreateNewGameConfiguration()
    {
        // Create a new instance of GameConfiguration
        var newConfig = new GameConfiguration();

        Console.WriteLine("Create New Game Configuration:");
        Console.WriteLine("==============================");

        Console.Write("Enter configuration name: ");
        newConfig.Name = Console.ReadLine() ?? "DefaultConfig";

        Console.Write("Enter board size (board is always a square): ");
        newConfig.BoardSizeWidth = newConfig.BoardSizeHeight = int.Parse(Console.ReadLine() ?? "5");

        Console.Write("Enter win condition (number of pieces in a line): ");
        newConfig.WinCondition = int.Parse(Console.ReadLine() ?? "3");
        
        Console.Write("Enter after how many moves can players move pieces on board: ");
        newConfig.MovePieceAfterNMoves = int.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter grid size (grid is always a square): ");
        newConfig.GridSize = int.Parse(Console.ReadLine() ?? "3");
        
        Console.Write("Enter X coordinates for Initial grid placement (top-left corner): ");
        newConfig.InitialGridTopLeftX = int.Parse(Console.ReadLine() ?? "0");
        
        Console.Write("Enter Y coordinates for Initial grid placement (top-left corner): ");
        newConfig.InitialGridTopLeftY = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter max game pieces: ");
        newConfig.MaxGamePieces = int.Parse(Console.ReadLine() ?? "10");
        
        //var configRepo = new ConfigRepositoryJson();
        //configRepo.SaveCustomConfiguration(newConfig);

        var configRepo = new ConfigRepositoryDb(ctx);
        configRepo.SaveCustomConfiguration(newConfig);

        Console.WriteLine("Configuration saved successfully!");
    }

}