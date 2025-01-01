using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;

    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    
    public List<SaveGame> GetGamesList()
    {
        // Fetch all saved game names (using CreatedAtDateTime as name if no other field exists)
        return _context.Savegames
            .ToList();
    }
    
    public void SaveGame(string jsonState, string name)
    {
        var configuration = _context.Configurations.
            FirstOrDefault(c => c.Name == name);
        
        if (configuration == null)
        {
            throw new Exception($"Configuration '{name}' not found in the database.");
        }
        
        // Create a new SaveGame entity
        var saveGameEntity = new SaveGame
        {
            CreatedAtDateTime = DateTime.Now.ToString("O").Replace(":", "-"), // Use the provided name as an identifier
            State = jsonState,
            ConfigurationId = configuration.Id
        };

        // Add a new game to the database
        _context.Savegames.Add(saveGameEntity);

        _context.SaveChanges();
        
    }
    
    private string SerializeGameState(GameState gameState)
    {
        // Serialize the GameState object to a JSON string
        return System.Text.Json.JsonSerializer.Serialize(gameState);
    }

    private GameState DeserializeGameState(string serializedGameState)
    {
        // Deserialize the JSON string to a GameState object
        return System.Text.Json.JsonSerializer.Deserialize<GameState>(serializedGameState)
               ?? throw new Exception("Failed to deserialize the game state.");
    }
}