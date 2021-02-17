using Client.Managers;
using UnityEngine.SceneManagement;

namespace Client.UI
{
    public class UIRestartScreenController
    {
        public UIRestartScreenController(UIRestartScreenView restartScreenView, IGameManager gameManager,
            int prevSceneIndex)
        {
            restartScreenView.ExitButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(prevSceneIndex));
            restartScreenView.RestartButton.onClick.AddListener(gameManager.RestartGame);

            gameManager.OnGameOver += () => restartScreenView.SetScreenVisibility(true);
            gameManager.OnNewLevelStarted += levelIndex => restartScreenView.SetScreenVisibility(false);
        }
    }
}