using ConsoleApp;
using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class AddConfig : PageModel
{
    private readonly ConfigRepositoryDb _configRepository;

    public AddConfig(AppDbContext context)
    {
        _configRepository = new ConfigRepositoryDb(context);
    }

    [BindProperty]
    public string Name { get; set; } = default!;
    [BindProperty]
    public int BoardWidth { get; set; } = 5;
    [BindProperty]
    public int BoardHeight { get; set; } = 5;
    [BindProperty]
    public int GridSize { get; set; } = 3;
    [BindProperty]
    public int MaxGamePieces { get; set; } = 5;
    [BindProperty]
    public int WindCondition { get; set; } = 3;
    [BindProperty]
    public int MovePieceAfterNMoves { get; set; } = 1;
    [BindProperty] 
    public int InitialGridTopLeftX { get; set; } = 0;
    [BindProperty] 
    public int InitialGridTopLeftY { get; set; } = 0;
    public List<string> Messages { get; set; } = new List<string>();

    public void OnGet()
    {
    }

    public void OnPost()
    {
        if (string.IsNullOrEmpty(Name))
        {
            Messages.Add("Name can't be empty!");
        }

        if (BoardWidth < 3 || BoardHeight < 3)
        {
            Messages.Add("Minimal board size is 3 x 3");
        }

        if (GridSize < 3 || GridSize < 3 || GridSize > BoardWidth || GridSize > BoardHeight)
        {
            Messages.Add($"Minimal grid size is 3 x 3 and maximal size is {BoardWidth} x {BoardHeight}");
        }

        if (WindCondition < 3)
        {
            Messages.Add("Minimal game winning line size is 3.");
        }

        if (MovePieceAfterNMoves < 0)
        {
            Messages.Add("Move Piece After N Moves can't be negative");
        }

        if (MaxGamePieces < WindCondition)
        {
            Messages.Add($"Minimal piece count is {WindCondition}");
        }

        if (Messages.Count != 0) return;
        Messages.Add("Configuration Added");
        var configuration = new GameConfiguration
        {
            Name = Name,
            BoardSizeWidth = BoardWidth,
            BoardSizeHeight = BoardHeight,
            WinCondition = WindCondition,
            MovePieceAfterNMoves = MovePieceAfterNMoves,
            InitialGridTopLeftX = InitialGridTopLeftX,
            InitialGridTopLeftY = InitialGridTopLeftY,
            MaxGamePieces = MaxGamePieces,
            GridSize = GridSize
        };
        _configRepository.SaveCustomConfiguration(configuration);
    }
}