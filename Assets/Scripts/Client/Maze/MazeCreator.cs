using System;
using System.Collections.Generic;
using Client.Actor;
using Client.Factory;
using Client.Init;
using Client.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Maze
{
    public class MazeCreator
    {
        private readonly GameConfig _gameConfig;

        private BasicMazeGenerator _mazeGenerator = null;

        private Pool<MazeElement> _wallsPool;
        private Pool<MazeElement> _floorPool;
        private Pool<MazeElement> _columnsPool;
        private Pool<MazeElement> _itemsPool;

        private Transform _rootTransform;
        private IActor _mazeExit;
        private ILoadingManager _loadingManager;

        public MazeCreator(ILoadingManager loadingManager, GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _loadingManager = loadingManager;

            Initialize();
        }

        public void Initialize()
        {
            _wallsPool =
                new Pool<MazeElement>(new PrefabFactory<MazeElement>(_loadingManager, _gameConfig.WallAssetRef));

            _floorPool =
                new Pool<MazeElement>(new PrefabFactory<MazeElement>(_loadingManager, _gameConfig.FloorAssetRef));

            _columnsPool =
                new Pool<MazeElement>(new PrefabFactory<MazeElement>(_loadingManager, _gameConfig.ColumnAssetRef));

            _itemsPool =
                new Pool<MazeElement>(new PrefabFactory<MazeElement>(_loadingManager, _gameConfig.ItemAssetRef));

            _rootTransform = new GameObject("MazeRoot").transform;
        }

        public void CreateNewMaze(int level, GameMode gameMode, Action<List<IActor>> mazeAttributes)
        {
            _wallsPool.ReleaseAll();
            _floorPool.ReleaseAll();
            _columnsPool.ReleaseAll();
            _itemsPool.ReleaseAll();

            if (!_gameConfig.FullRandom)
            {
                Random.InitState(_gameConfig.RandomSeed);
            }

            var levelConfig = _gameConfig.MazeLevelsConfigList[level];

            switch (_gameConfig.Algorithm)
            {
                case MazeGenerationAlgorithm.PureRecursive:
                    _mazeGenerator = new RecursiveMazeAlgorithm(levelConfig.RowsCount, levelConfig.ColumnsCount);
                    break;
            }

            _mazeGenerator.GenerateMaze();

            BuildMazeCore(levelConfig);
            CreateMazeAttributes(gameMode, levelConfig, attributes => mazeAttributes?.Invoke(attributes));
        }

        private void BuildMazeCore(MazeLevelConfig levelConfig)
        {
            for (var row = 0; row < levelConfig.RowsCount; row++)
            {
                for (var column = 0; column < levelConfig.ColumnsCount; column++)
                {
                    var x = column * levelConfig.CellWidth;
                    var z = row * levelConfig.CellHeight;
                    var cell = _mazeGenerator.GetMazeCell(row, column);

                    _floorPool.Allocate(element =>
                    {
                        element.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
                        element.SetParent(_rootTransform);
                    });

                    if (cell.WallRight)
                    {
                        _wallsPool.Allocate(element =>
                        {
                            element.SetPositionAndRotation(
                                new Vector3(x + levelConfig.CellWidth * 0.5f, 0, z) + element.transform.position,
                                Quaternion.Euler(0, 90, 0));
                            element.SetParent(_rootTransform);
                        });
                    }

                    if (cell.WallFront)
                    {
                        _wallsPool.Allocate(element =>
                        {
                            element.SetPositionAndRotation(
                                new Vector3(x, 0, z + levelConfig.CellHeight * 0.5f) + element.transform.position,
                                Quaternion.Euler(0, 0, 0));
                            element.SetParent(_rootTransform);
                        });
                    }

                    if (cell.WallLeft)
                    {
                        _wallsPool.Allocate(element =>
                        {
                            element.SetPositionAndRotation(
                                new Vector3(x - levelConfig.CellWidth * 0.5f, 0, z) + element.transform.position,
                                Quaternion.Euler(0, 270, 0));
                            element.SetParent(_rootTransform);
                        });
                    }

                    if (cell.WallBack)
                    {
                        _wallsPool.Allocate(element =>
                        {
                            element.SetPositionAndRotation(
                                new Vector3(x, 0, z - levelConfig.CellHeight * 0.5f) + element.transform.position,
                                Quaternion.Euler(0, 180, 0));
                            element.SetParent(_rootTransform);
                        });
                    }
                }
            }

            if (_gameConfig.ColumnAssetRef == null) return;

            for (var row = 0; row < levelConfig.RowsCount + 1; row++)
            {
                for (var column = 0; column < levelConfig.ColumnsCount + 1; column++)
                {
                    var x = column * levelConfig.CellWidth;
                    var z = row * levelConfig.CellHeight;

                    _columnsPool.Allocate(element =>
                    {
                        element.SetPositionAndRotation(
                            new Vector3(x - levelConfig.CellWidth * 0.5f, element.transform.position.y,
                                z - levelConfig.CellHeight * 0.5f), element.transform.rotation);
                        element.SetParent(_rootTransform);
                    });
                }
            }
        }

        private void CreateMazeAttributes(GameMode mode, MazeLevelConfig levelConfig, Action<List<IActor>> onComplete)
        {
            var mazeAttributes = new List<IActor>();

            if (mode == GameMode.FindItems && _gameConfig.ItemAssetRef != null)
            {
                var randCells = _mazeGenerator.GetRandomMazeCells(levelConfig.ItemsCount);

                foreach (var cell in randCells)
                {
                    var x = cell.Key.X * levelConfig.CellWidth;
                    var z = cell.Key.Y * levelConfig.CellHeight;

                    _itemsPool.Allocate(element =>
                    {
                        element.SetPositionAndRotation(
                            new Vector3(x, element.transform.position.y, z),
                            element.transform.rotation);

                        element.SetParent(_rootTransform);

                        var actor = element.GetComponent<IActor>();

                        if (actor != null)
                        {
                            mazeAttributes.Add(actor);
                        }

                        onComplete?.Invoke(mazeAttributes);
                    });
                }
            }
            else if (mode == GameMode.EscapeFromMaze)
            {
                if (_mazeExit == null)
                {
                    _loadingManager.SpawnPrefab(_gameConfig.MazeExitAssetRef,
                        loadedPlayer =>
                        {
                            _mazeExit = loadedPlayer.GetComponent<IActor>();

                            if (_mazeExit != null)
                            {
                                mazeAttributes.Add(_mazeExit);
                            }

                            onComplete?.Invoke(mazeAttributes);
                        });
                }
                else
                {
                    mazeAttributes.Add(_mazeExit);
                    onComplete?.Invoke(mazeAttributes);
                }
            }
        }
    }

    public enum MazeGenerationAlgorithm
    {
        PureRecursive = 0
    }
}