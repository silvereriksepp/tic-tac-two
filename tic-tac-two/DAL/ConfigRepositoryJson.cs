using System.Text.Json;
using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    
    
    public List<string> GetConfigurationNames()
    {
        CheckAndCreateInitialConfig();

        
        var res = new List<string>();
        
        foreach (var fullFileName in System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList())
        {
            var fileNameParts = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            var primaryName = System.IO.Path.GetFileNameWithoutExtension(fileNameParts);
            res.Add(primaryName);
        }

        return res;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(FileHelper.BasePath + name + FileHelper.ConfigExtension);
        var config = JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    private void CheckAndCreateInitialConfig()
    {
        if (!Directory.Exists(FileHelper.BasePath))
        {
            Directory.CreateDirectory(FileHelper.BasePath);
        }
        
        var data = System.IO.Directory.GetFiles(FileHelper.BasePath, "*" + FileHelper.ConfigExtension).ToList();

        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var optionNames = hardcodedRepo.GetConfigurationNames();
            foreach (var optionName in optionNames)
            {
                var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = JsonSerializer.Serialize(gameOption);
                File.WriteAllText(FileHelper.BasePath + gameOption.Name + FileHelper.ConfigExtension, optionJsonStr);
            }
        }
    }
    
    public void SaveCustomConfiguration(GameConfiguration configuration)
    {
        CheckAndCreateInitialConfig();

        var filePath = FileHelper.BasePath + configuration.Name + FileHelper.ConfigExtension;
        
        if (File.Exists(filePath))
        {
            Console.WriteLine("Configuration with this name already exists.");
            return;
        }

        var configJsonStr = JsonSerializer.Serialize(configuration, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, configJsonStr);
        Console.WriteLine("Custom configuration saved successfully.");
    }
}