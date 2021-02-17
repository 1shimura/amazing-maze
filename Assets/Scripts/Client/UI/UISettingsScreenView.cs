using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class UISettingsScreenView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _switchCameraButton;
        [SerializeField] private Button _exitButton;

        public Button CloseButton => _closeButton;
        public Button RestartButton => _restartButton;
        public Button SwitchCameraButton => _switchCameraButton;
        public Button ExitButton => _exitButton;

        public void SetScreenVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void OnDestroy()
        {
            CloseButton.onClick.RemoveAllListeners();
            RestartButton.onClick.RemoveAllListeners();
            SwitchCameraButton.onClick.RemoveAllListeners();
            ExitButton.onClick.RemoveAllListeners();
        }
    }
}