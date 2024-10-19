using System.Text.Json;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{

    private readonly string _basePath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                        + Path.DirectorySeparatorChar + "tic-tac-toe" + Path.DirectorySeparatorChar;
    
    
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        
        var res = new List<string>();
        
        foreach (var fullFileName in System.IO.Directory.GetFiles(_basePath, "*.config.json").ToList())
        {
            var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            var primaryName = System.IO.Path.GetFileNameWithoutExtension(fileNameParts);
            res.Add(primaryName);
        }

        return res;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(_basePath + name + ".config.json");
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
        
        var data = System.IO.Directory.GetFiles(_basePath, "*.config.json").ToList();

        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var optionNames = hardcodedRepo.GetConfigurationNames();
            foreach (var optionName in optionNames)
            {
                var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = JsonSerializer.Serialize(gameOption);
                File.WriteAllText(_basePath + gameOption.Name + ".config.json", optionJsonStr);
            }
        }
    }
}