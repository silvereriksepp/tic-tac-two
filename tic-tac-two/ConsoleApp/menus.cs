using MenuSystem;

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
                Shortcut = "O",
                Title = "Options",
                MenuItemAction = OptionsMenu.Run
            },

            new MenuItem()
            {
                Shortcut = "N",
                Title = "New Game",
                MenuItemAction = GameController.MainLoop
            }
        ]);
    
    public static string DummyMethod()
    {
        Console.Write("Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine();
        return "foobar";
    }
    
    public static Menu CustomRulesMenu = new Menu(
        EMenuLevel.Deep,
        "Tic-Tac-Two Custom Rules Menu", [
            new MenuItem()
            {
                Shortcut = "1",
                Title = "Board Size",
                MenuItemAction = OptionsMenu.Run
            },

            new MenuItem()
            {
                Shortcut = "N",
                Title = "New Game",
                MenuItemAction = GameController.MainLoop
            }
        ]);
}