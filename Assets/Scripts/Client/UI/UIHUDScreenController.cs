using Client.Managers;

namespace Client.UI
{
    public class UIHUDScreenController
    {
        public UIHUDScreenController(IGameSettingsManager gameSettingsManager, UIHUDScreenView hudView,
            IGameManager gameManager)
        {
            hudView.SettingsButton.onClick.AddListener(() =>
                hudView.ScreenSettings.SetScreenVisibility(!hudView.ScreenSettings.gameObject.activeSelf));

            hudView.SetCurrentLevelIndex(gameManager.CurrentLevel + 1);

            gameManager.OnNewLevelStarted += i => hudView.SetCurrentLevelIndex(i + 1);

            hudView.SetItemsFoundTextVisibility(gameSettingsManager.CurrentGameMode == GameMode.FindItems);
            if (gameSettingsManager.CurrentGameMode != GameMode.FindItems) return;

            hudView.MaxItemsCount =
                gameSettingsManager.GameConfig.MazeLevelsConfigList[gameManager.CurrentLevel].ItemsCount;
            hudView.SetItemsCount(gameManager.ItemsFound);

            gameManager.OnItemFound += hudView.SetItemsCount;

            gameManager.OnNewLevelStarted += level =>
            {
                hudView.MaxItemsCount = gameSettingsManager.GameConfig.MazeLevelsConfigList[gameManager.CurrentLevel]
                    .ItemsCount;
                hudView.SetItemsCount(gameManager.ItemsFound);
            };
        }
    }
}