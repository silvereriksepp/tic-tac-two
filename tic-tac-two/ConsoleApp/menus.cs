using DAL;
using Domain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public static class menus
{
    public static readonly Menu OptionsMenu =
        new Menu(
            EMenuLevel.Secondary,
            "Tic-Tac-Two options", menuItems: [
                new MenuItem()
                {
                    Shortcut = "X",
                    Title = "X starts",
                    MenuItemAction = DummyMethod
                },
    
                new MenuItem()
                {
                    Shortcut = "O",
                    Title = "O starts",
                    MenuItemAction = DummyMethod
                },
            ]);

    

    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "Tic-Tac-Two", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "New Game",
                MenuItemAction = () => GameController.MainLoop()
            },
            new MenuItem()
            {
                Shortcut = "L",
                Title = "Load Game",
                MenuItemAction = LoadGameMenu.Run
            },
            new MenuItem()
            {
                Shortcut = "C",
                Title = "Create New Configuration",
                MenuItemAction = () => {
                    GameController.CreateNewGameConfiguration();
                    return string.Empty;
                }
            }
        ]);
    
    public static Menu LoadGameMenu
    {
        
        get
        {
            var connectionString = $"Data Source={FileHelper.BasePath}app.db";

            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .Options;

            using var ctx = new AppDbContext(contextOptions);
            
            var dbSaves = new GameRepositoryDb(ctx).GetGamesList();
            
            
            var saveFiles = Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.GameExtension).
                ToList();
            
            if (dbSaves.Count == 0 && saveFiles.Count == 0)
            {
                return new Menu(EMenuLevel.Secondary, "No Saved games Found",
                    new List<MenuItem> { new MenuItem { Title = "no saves", Shortcut = " " } });
            }

            var index = 0;
            var menuItems = saveFiles.Select((file, index) => new MenuItem
            {
                Shortcut = index.ToString(),
                Title = Path.GetFileName(file),
                MenuItemAction = () => GameController.LoadGame(file)
            }).ToList();

            foreach (var game in dbSaves)
            {
                menuItems.Add(new MenuItem
                {
                    Shortcut = (menuItems.Count + index).ToString(),
                    Title = game.CreatedAtDateTime,
                    MenuItemAction = () => GameController.LoadGameDb(game.State)
                });
            }

            return new Menu(
                EMenuLevel.Secondary,
                "Load Game",
                menuItems
            );
        }
    }
    
    public static string DummyMethod()
    {
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine();
        return "foobar";
    }
}