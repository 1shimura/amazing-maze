using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class UIStartScreenView : MonoBehaviour
    {
        [SerializeField] private Button _playEscapeFromMazeButton;
        [SerializeField] private Button _playFindItemsButton;

        public Button PlayEscapeFromMazeButton => _playEscapeFromMazeButton;
        public Button PlayFindItemsButton => _playFindItemsButton;

        private void OnDestroy()
        {
            _playEscapeFromMazeButton.onClick.RemoveAllListeners();
            _playFindItemsButton.onClick.RemoveAllListeners();
        }
    }
}