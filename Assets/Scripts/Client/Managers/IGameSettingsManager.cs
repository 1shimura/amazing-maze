using Client.Init;

namespace Client
{
    public interface IGameSettingsManager
    {
        GameConfig GameConfig { get; set; }
        public GameMode CurrentGameMode { get; set; }
    }
    
    public enum GameMode
    {
        EscapeFromMaze = 0,
        FindItems = 1
    }
}