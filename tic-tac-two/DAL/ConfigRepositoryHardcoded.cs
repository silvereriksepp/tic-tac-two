using GameBrain;

namespace DAL;

public class ConfigRepositoryHardcoded : IConfigRepository
{
    private List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration()
        {
            Name = "Classical",
            
        },
        new GameConfiguration()
        {
            Name = "Custom Rules",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            WinCondition = 3,
            MovePieceAfterNMoves = 2,
            InitialGridCenterX = 2,
            InitialGridCenterY = 2
        }
    };

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
}