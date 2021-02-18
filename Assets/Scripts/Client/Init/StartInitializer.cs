using Client.Managers;
using Client.UI;
using UnityEngine;
using Zenject;

namespace Client.Init
{
    public class StartInitializer : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private UIStartScreenView _startScreenView;
        [Space] 
        [SerializeField] private int _currentSceneIndex;
        [SerializeField] private int _nextSceneIndex;

        [Inject] private IGameSettingsManager _gameSettingsManager;
        [Inject] private ILoadingManager _loadingManager;

        public void Awake()
        {
            _gameSettingsManager.GameConfig = _gameConfig;
            new UIStartScreenController(_gameSettingsManager, _startScreenView, _loadingManager, _currentSceneIndex,
                _nextSceneIndex);
        }
    }
}