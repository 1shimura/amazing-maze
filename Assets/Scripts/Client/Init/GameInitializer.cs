using Cinemachine;
using Client.Managers;
using Client.UI;
using UnityEngine;
using Zenject;

namespace Client.Init
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private UIHUDScreenView _hudScreenView;
        [SerializeField] private UISettingsScreenView _settingsScreenView;
        [SerializeField] private UITimerView _timerView;
        [SerializeField] private UIRestartScreenView _restartScreenView;
        [Space]
        [SerializeField] private int _firstSceneIndex;
        [Space]
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        [Inject] private IGameSettingsManager _gameSettingsManager;
        [Inject] private IGameManager _gameManager;
        [Inject] private ILoadingManager _loadingManager;

        public void Start()
        {
            _gameManager.Initialize(_loadingManager, _gameSettingsManager, _virtualCamera);
            InitializeUI(_gameSettingsManager.GameConfig);
        }

        private void InitializeUI(GameConfig gameConfig)
        {
            new UIHUDScreenController(_gameSettingsManager, _hudScreenView, _gameManager);
            new UISettingsScreenController(_settingsScreenView, _loadingManager, _gameManager, _firstSceneIndex);
            new UITimerController(gameConfig, _timerView, _gameManager);
            new UIRestartScreenController(_restartScreenView, _loadingManager, _gameManager, _firstSceneIndex);
        }
    }
}