using System.Collections.Generic;
using Client.Actor;
using Client.Factory;
using Client.Init;
using UnityEngine;

namespace Client.Maze
{
    public class MazeCreator
    {
        public MazeCreator(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
            
            Initialize();
        }

        private readonly GameConfig _gameConfig;

        private BasicMazeGenerator _mazeGenerator = null;

        private Pool<MazeElement> _wallsPool;
        private Pool<MazeElement> _floorPool;
        private Pool<MazeElement> _columnsPool;
        private Pool<MazeElement> _itemsPool;
        
        private Transform _rootTransform;

        private IActor _mazeExit;

        public void Initialize()
        {
            _wallsPool = new Pool<MazeElement>(new PrefabFactory<MazeElement>(_gameConfig.WallPrefab));
            _floorPool = new Pool<MazeElement>(new PrefabFactory<MazeElement>(_gameConfig.FloorPrefab));
            _columnsPool = new Pool<MazeElement>(new PrefabFactory<MazeElement>(_gameConfig.ColumnPrefab));
            _itemsPool = new Pool<MazeElement>(new PrefabFactory<MazeElement>(_gameConfig.ItemPrefab));
            
            _rootTransform = new GameObject("MazeRoot").transform;
        }

        public void CreateNewMaze(int level, GameMode gameMode, out List<IActor> levelAttributes)
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
            levelAttributes = CreateMazeAttributes(gameMode, levelConfig);
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

                    MazeElement mazeElement = null;

                    mazeElement = _floorPool.Allocate();
                    mazeElement.SetPositionAndRotation(new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0));
                    mazeElement.SetParent(_rootTransform);

                    if (cell.WallRight)
                    {
                        mazeElement = _wallsPool.Allocate();
                        mazeElement.SetPositionAndRotation(
                            new Vector3(x + levelConfig.CellWidth * 0.5f, 0, z) + _gameConfig.WallPrefab.transform.position,
                            Quaternion.Euler(0, 90, 0));
                        mazeElement.SetParent(_rootTransform);
                    }

                    if (cell.WallFront)
                    {
                        mazeElement = _wallsPool.Allocate();
                        mazeElement.SetPositionAndRotation(
                            new Vector3(x, 0, z + levelConfig.CellHeight * 0.5f) + _gameConfig.WallPrefab.transform.position,
                            Quaternion.Euler(0, 0, 0));
                        mazeElement.SetParent(_rootTransform);
                    }

                    if (cell.WallLeft)
                    {
                        mazeElement = _wallsPool.Allocate();
                        mazeElement.SetPositionAndRotation(
                            new Vector3(x - levelConfig.CellWidth * 0.5f, 0, z) + _gameConfig.WallPrefab.transform.position,
                            Quaternion.Euler(0, 270, 0));
                        mazeElement.SetParent(_rootTransform);
                    }

                    if (cell.WallBack)
                    {
                        mazeElement = _wallsPool.Allocate();
                        mazeElement.SetPositionAndRotation(
                            new Vector3(x, 0, z - levelConfig.CellHeight * 0.5f) + _gameConfig.WallPrefab.transform.position,
                            Quaternion.Euler(0, 180, 0));
                        mazeElement.SetParent(_rootTransform);
                    }
                }
            }

            if (_gameConfig.ColumnPrefab != null)
            {
                for (var row = 0; row < levelConfig.RowsCount + 1; row++)
                {
                    for (var column = 0; column < levelConfig.ColumnsCount + 1; column++)
                    {
                        var x = column * levelConfig.CellWidth;
                        var z = row * levelConfig.CellHeight;

                        var mazeElement = _columnsPool.Allocate();
                        mazeElement.SetPositionAndRotation(
                            new Vector3(x - levelConfig.CellWidth * 0.5f, _gameConfig.ColumnPrefab.transform.position.y,
                                z - levelConfig.CellHeight * 0.5f), _gameConfig.ColumnPrefab.transform.rotation);
                        mazeElement.SetParent(_rootTransform);
                    }
                }
            }
        }

        private List<IActor> CreateMazeAttributes(GameMode mode, MazeLevelConfig levelConfig)
        {
            var mazeAttributes = new List<IActor>();

            switch (mode)
            {
                case GameMode.FindItems when _gameConfig.ItemPrefab != null:
                {
                    var randCells = _mazeGenerator.GetRandomMazeCells(levelConfig.ItemsCount);

                    foreach (var cell in randCells)
                    {
                        var x = cell.Key.X * levelConfig.CellWidth;
                        var z = cell.Key.Y * levelConfig.CellHeight;

                        var newItem = _itemsPool.Allocate();

                        newItem.SetPositionAndRotation(
                            new Vector3(x, _gameConfig.ItemPrefab.transform.position.y, z),
                            _gameConfig.ItemPrefab.transform.rotation);

                        newItem.SetParent(_rootTransform);

                        var actor = newItem.GetComponent<IActor>();

                        if (actor != null)
                        {
                            mazeAttributes.Add(actor);
                        }
                    }

                    break;
                }
                case GameMode.EscapeFromMaze:
                {
                    if (_mazeExit == null)
                    {
                        _mazeExit = Object.Instantiate(_gameConfig.MazeExitPrefab).GetComponent<IActor>();
                    }

                    if (_mazeExit != null)
                    {
                        mazeAttributes.Add(_mazeExit);
                    }

                    break;
                }
            }

            return mazeAttributes;
        }
    }

    public enum MazeGenerationAlgorithm
    {
        PureRecursive = 0
    }
}