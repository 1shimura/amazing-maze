using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.UI
{
    public class UIHUDScreenView : MonoBehaviour
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private TextMeshProUGUI _currentLevelIndex;
        [SerializeField] private TextMeshProUGUI _foundItemsCount;
        [SerializeField] private UISettingsScreenView _screenSettings;
        
        public Button SettingsButton => _settingsButton;
        public UISettingsScreenView ScreenSettings => _screenSettings;
        public int MaxItemsCount { get; set; }

        public void SetItemsFoundTextVisibility(bool isVisible)
        {
            _foundItemsCount.gameObject.SetActive(isVisible);
        }

        public void SetItemsCount(int itemsCount)
        {
            _foundItemsCount.SetText($"{itemsCount}/{MaxItemsCount}");
        }

        public void SetCurrentLevelIndex(int level)
        {
            _currentLevelIndex.SetText(level.ToString());
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveAllListeners();
        }
    }
}