using Client.Init;
using Client.Managers;

namespace Client.UI
{
    public class UITimerController
    {
        public UITimerController(GameConfig gameConfig, UITimerView timerView, IGameManager gameManager)
        {
            StartTimer(gameConfig, 0, timerView);
            
            gameManager.OnNewLevelStarted += levelIndex => StartTimer(gameConfig, levelIndex, timerView);
            timerView.TimeIsUp += gameManager.GameOver;
        }
        
        public void StartTimer(GameConfig gameConfig, int level, UITimerView timerView)
        {
            var configList = gameConfig.MazeLevelsConfigList;
            
            if (configList.Count - 1 < level) return;

            var currentConfig = configList[level];
            timerView.StartTimer(currentConfig.Timer);
        }
    }
}