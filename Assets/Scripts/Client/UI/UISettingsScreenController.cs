using Client.Managers;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Client.UI
{
    public class UISettingsScreenController
    {
        public UISettingsScreenController(UISettingsScreenView settingsScreenView, ILoadingManager loadingManager, IGameManager gameManager, int prevSceneIndex)
        {
            settingsScreenView.CloseButton.onClick.AddListener(() => settingsScreenView.gameObject.SetActive(false));
            settingsScreenView.ExitButton.onClick.AddListener(() => loadingManager.LoadSceneAsync(prevSceneIndex));
            settingsScreenView.RestartButton.onClick.AddListener(gameManager.RestartGame);
            settingsScreenView.SwitchCameraButton.onClick.AddListener(gameManager.SwitchCamera);
        }
    }
}