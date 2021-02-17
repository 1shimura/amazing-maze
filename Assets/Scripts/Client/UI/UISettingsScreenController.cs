using Client.Managers;
using UnityEngine.SceneManagement;

namespace Client.UI
{
    public class UISettingsScreenController
    {
        public UISettingsScreenController(UISettingsScreenView settingsScreenView, IGameManager gameManager, int prevSceneIndex)
        {
            settingsScreenView.CloseButton.onClick.AddListener(() => settingsScreenView.gameObject.SetActive(false));
            settingsScreenView.ExitButton.onClick.AddListener(() => SceneManager.LoadSceneAsync(prevSceneIndex));
            settingsScreenView.RestartButton.onClick.AddListener(gameManager.RestartGame);
            settingsScreenView.SwitchCameraButton.onClick.AddListener(gameManager.SwitchCamera);
        }
    }
}