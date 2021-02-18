using System;
using Cinemachine;

namespace Client.Managers
{
    public interface IGameManager
    {
        int CurrentLevel { get; }
        int ItemsFound { get; }
        Action<int> OnNewLevelStarted { get; set; }
        Action OnGameOver { get; set; }
        Action<int> OnItemFound { get; set; }

        void Initialize(ILoadingManager loadingManager, IGameSettingsManager gameSettings,
            CinemachineVirtualCamera mainCamera);

        void ItemFound();
        void StartNextLevel();
        void GameOver();
        void RestartGame();
        void SwitchCamera();
    }
}