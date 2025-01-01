using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(string jsonStateName, string gameConfigName);
}