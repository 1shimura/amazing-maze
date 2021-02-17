using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class UIRestartScreenView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        public Button RestartButton => _restartButton;
        public Button ExitButton => _exitButton;

        public void SetScreenVisibility(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void OnDestroy()
        {
            _restartButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}