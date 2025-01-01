using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    
    public void SaveGame(string jsonStateString, string gameConfigName)
    {   
        var filename = FileHelper.BasePath + 
                       gameConfigName + " " + DateTime.Now.ToString("O").Replace(":", "-") + 
                       FileHelper.GameExtension;
        
        File.WriteAllText(filename, jsonStateString);
    }
}