using Client.Init;

namespace Client.Managers
{
    public class GameSettingsManager : IGameSettingsManager
    {
        public GameConfig GameConfig { get; set; }
        public GameMode CurrentGameMode { get; set; }
    }
}