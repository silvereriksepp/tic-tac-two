using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class ConfigRepositoryDb : IConfigRepository
{
    private readonly AppDbContext _context;
    
    public ConfigRepositoryDb(AppDbContext context)
    {
        _context = context;
    }
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        return _context.Configurations.Select(x => x.Name).ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        // Retrieve the configuration from the database by name
        var configEntity = _context.Configurations
            .FirstOrDefault(c => c.Name == name);

        if (configEntity == null)
        {
            throw new Exception($"Configuration with name {name} could not be found!");
        }

        // Convert the database entity to GameConfiguration and return
        return new GameConfiguration
        {
            Name = configEntity.Name,
            BoardSizeWidth = configEntity.BoardWidth,
            BoardSizeHeight = configEntity.BoardHeight,
            WinCondition = configEntity.WindCondition,
            MovePieceAfterNMoves = configEntity.MovePieceAfterNMoves,
            InitialGridTopLeftX = configEntity.InitialGridTopLeftX,
            InitialGridTopLeftY = configEntity.InitialGridTopLeftY,
            MaxGamePieces = configEntity.MaxGamePieces,
            GridSize = configEntity.GridSize
        };
    }

    private void CheckAndCreateInitialConfig()
    {
        if (!_context.Configurations.Any())
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var initialConfigs = hardcodedRepo.GetConfigurationNames()
                .Select(name => hardcodedRepo.GetConfigurationByName(name))
                .ToList();

            // Convert hardcoded configurations to database entities
            foreach (var config in initialConfigs)
            {
                var configEntity = new Configuration
                {
                    Name = config.Name,
                    BoardWidth = config.BoardSizeWidth,
                    BoardHeight = config.BoardSizeHeight
                    // Add other properties here if needed, 
                };

                _context.Configurations.Add(configEntity);
            }

            _context.SaveChanges();
        }
    }
    
    public void SaveCustomConfiguration(GameConfiguration configuration)
    {
        CheckAndCreateInitialConfig();

        // Check if a configuration with the same name already exists in the database
        if (_context.Configurations.Any(c => c.Name == configuration.Name))
        {
            Console.WriteLine("Configuration with this name already exists.");
            return;
        }

        // Create a new Configuration entity and populate it with the properties from GameConfiguration
        var configEntity = new Configuration
        {
            Name = configuration.Name,
            BoardWidth = configuration.BoardSizeWidth,
            BoardHeight = configuration.BoardSizeHeight,
            WindCondition = configuration.WinCondition,
            MovePieceAfterNMoves = configuration.MovePieceAfterNMoves,
            InitialGridTopLeftX = configuration.InitialGridTopLeftX,
            InitialGridTopLeftY = configuration.InitialGridTopLeftY,
            MaxGamePieces = configuration.MaxGamePieces,
            GridSize = configuration.GridSize,
        };

        // Add the new configuration to the database
        _context.Configurations.Add(configEntity);
        _context.SaveChanges();

        Console.WriteLine("Custom configuration saved successfully to the database.");
    }
}