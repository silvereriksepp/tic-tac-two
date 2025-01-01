namespace MenuSystem;

public class Menu
{
    public string MenuHeader { get; set; }
    private static readonly string MenuDivider = "==================";
    private List<MenuItem> MenuItems { get; set; }
    
    private MenuItem _menuItemExit = new MenuItem()
    {
        Shortcut = "E",
        Title = "Exit"
    };
    
    private MenuItem _menuItemReturn = new MenuItem()
    {
        Shortcut = "R",
        Title = "Return"
    };
    
    private MenuItem _menuItemReturnMain = new MenuItem()
    {
        Shortcut = "M",
        Title = "return to Main menu"
    };
    
    private EMenuLevel MenuLevel { get; set; }

    public void SetMenuItemAction(string shortCut, Func<string> action)
    {
        var menuItem = MenuItems.Single(m => m.Shortcut == shortCut);
        menuItem.MenuItemAction = action;
    }

    public Menu(EMenuLevel menuLevel, string menuHeader, List<MenuItem> menuItems)
    {
        if (string.IsNullOrWhiteSpace(menuHeader))
        {
            throw new ArgumentException("Menu header cannot be null or empty.");
        }
 
        MenuHeader = menuHeader;

        if (menuItems == null || menuItems.Count == 0)
        {
            throw new ArgumentException("Menu items cannot be null or empty.");
        }

        MenuItems = menuItems;
        MenuLevel = menuLevel;
        
        if (MenuLevel != EMenuLevel.Main)
        {
            menuItems.Add(_menuItemReturn);
        }
        if (MenuLevel == EMenuLevel.Deep)
        {
            menuItems.Add(_menuItemReturnMain);
        }
        menuItems.Add(_menuItemExit);
    }

    public string Run()
    {
        do
        {
            var menuItem = DisplayMenuGetUserChoice();
            var menuReturnValue = "";

            if (menuItem.MenuItemAction != null)
            {
                menuReturnValue = menuItem.MenuItemAction();
            }

            if (menuItem.Shortcut == _menuItemReturn.Shortcut)
            {
                return string.Empty;
            }
            if (menuItem.Shortcut == _menuItemExit.Shortcut || menuReturnValue == _menuItemExit.Shortcut)
            {
                return _menuItemExit.Shortcut;
            }
            if ((menuItem.Shortcut == _menuItemReturnMain.Shortcut || menuReturnValue == _menuItemReturnMain.Shortcut) && MenuLevel != EMenuLevel.Main)
            {
                return menuItem.Shortcut;
            }

            if (!string.IsNullOrWhiteSpace(menuReturnValue))
            {
                return menuReturnValue;
            }
        } while (true);
    }

    private MenuItem DisplayMenuGetUserChoice()
    {
        var userInput = "";
        do
        {
            DrawMenu();
            
            userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Please enter a valid input.");
                Console.WriteLine();
            }
            else
            {
                userInput = userInput.ToUpper();

                foreach (var menuItem in MenuItems)
                {
                    if (menuItem.Shortcut.ToUpper() != userInput) continue;
                    
                    return menuItem;
                }
                Console.WriteLine("Please enter something from the menu.");
                Console.WriteLine();
            }
        } while (true);

    }

    private void DrawMenu()
    {
        Console.WriteLine(MenuHeader);
        Console.WriteLine(MenuDivider);
        foreach (var item in MenuItems)
        {
            Console.WriteLine(item);
        }
        
        Console.WriteLine();

        Console.Write(">");
    }
}