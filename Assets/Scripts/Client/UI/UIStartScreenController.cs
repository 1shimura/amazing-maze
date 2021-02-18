using Client.Managers;
using UnityEngine.SceneManagement;

namespace Client.UI
{
    public class UIStartScreenController
    {
        public UIStartScreenController(IGameSettingsManager gameSettingsManager, UIStartScreenView startScreenView, ILoadingManager loadingManager, int currentSceneIndex, int nextSceneIndex)
        {
            startScreenView.PlayEscapeFromMazeButton.onClick.AddListener(() =>
            {
                gameSettingsManager.CurrentGameMode = GameMode.EscapeFromMaze;
                LoadNextScene();
            });
            
            
            startScreenView.PlayFindItemsButton.onClick.AddListener(() =>
            {
                gameSettingsManager.CurrentGameMode = GameMode.FindItems;
                LoadNextScene();
            });

            void LoadNextScene()
            {
                loadingManager.LoadSceneAsync(nextSceneIndex, LoadSceneMode.Additive);
                loadingManager.UnloadSceneAsync(currentSceneIndex);
            }
        }
    }
}