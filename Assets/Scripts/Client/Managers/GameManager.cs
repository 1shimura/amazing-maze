using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Client.Abilities;
using Client.Actor;
using Client.Maze;
using Object = UnityEngine.Object;

namespace Client.Managers
{
    public class GameManager : IGameManager
    {
        public int CurrentLevel { get; private set; }
        public int ItemsFound { get; private set; }
        public Action<int> OnNewLevelStarted { get; set; }
        public Action OnGameOver { get; set; }
        public Action<int> OnItemFound { get; set; }

        private IGameSettingsManager _gameSettingsManager;
        private MazeCreator _mazeCreator;

        private int _currentLevelIndex;

        private IActor _player;

        private CinemachineVirtualCamera _mainCamera = null;
        private CinemachineVirtualCamera _fpvCamera = null;

        private List<IActor> _mazeAttributes = new List<IActor>();

        public void Initialize(IGameSettingsManager gameSettingsManager, CinemachineVirtualCamera mainCamera)
        {
            _gameSettingsManager = gameSettingsManager;
            var gameConfig = _gameSettingsManager.GameConfig;

            _player = Object.Instantiate(gameConfig.PlayerPrefab).GetComponent<IActor>();
            _mazeCreator = new MazeCreator(_gameSettingsManager.GameConfig);
            _player.Initialize(this);

            _mainCamera = mainCamera;
            _mainCamera.Follow = _player.Transform;

            var fpvCamera = _player.Abilities.Find(ability => ability is AbilityFPV) as AbilityFPV;
            if (fpvCamera != null)
            {
                _fpvCamera = fpvCamera.FpvCamera;
            }

            if (_player == null || !gameConfig.MazeLevelsConfigList.Any()) return;

            StartLevel(0);
        }

        public void ItemFound()
        {
            ItemsFound++;
            OnItemFound?.Invoke(ItemsFound);

            if (ItemsFound >= _gameSettingsManager.GameConfig.MazeLevelsConfigList[CurrentLevel].ItemsCount)
            {
                StartNextLevel();
            }
        }

        public void GameOver()
        {
            ResetActors();
            OnGameOver?.Invoke();
        }

        public void RestartGame()
        {
            StartLevel(0);
        }

        public void SwitchCamera()
        {
            if (_mainCamera == null || _fpvCamera == null) return;

            var mainCamPriority = _mainCamera.Priority;

            _mainCamera.Priority = _fpvCamera.Priority;
            _fpvCamera.Priority = mainCamPriority;
        }

        public void StartNextLevel()
        {
            ResetActors();

            if (_gameSettingsManager.GameConfig.MazeLevelsConfigList.Count - 1 > _currentLevelIndex)
            {
                StartLevel(_currentLevelIndex + 1);
            }
            else
            {
                GameOver();
            }
        }

        private void StartLevel(int level)
        {
            _currentLevelIndex = level;
            _mazeCreator.CreateNewMaze(level, _gameSettingsManager.CurrentGameMode, out _mazeAttributes);

            _player.PrepareForNewLevel(_gameSettingsManager.GameConfig.MazeLevelsConfigList[level].PlayerSpawnPoint);

            foreach (var mazeAttribute in _mazeAttributes)
            {
                mazeAttribute.Initialize(this);

                if (_gameSettingsManager.CurrentGameMode == GameMode.EscapeFromMaze)
                {
                    mazeAttribute.PrepareForNewLevel(_gameSettingsManager.GameConfig.MazeLevelsConfigList[level]
                        .MazeExitSpawnPoint);
                }
            }

            ItemsFound = 0;

            CurrentLevel = level;
            OnNewLevelStarted?.Invoke(CurrentLevel);
        }

        private void ResetActors()
        {
            (_player as IResettable)?.Reset();

            foreach (var mazeAttribute in _mazeAttributes)
            {
                (mazeAttribute as IResettable)?.Reset();
            }
        }
    }
}