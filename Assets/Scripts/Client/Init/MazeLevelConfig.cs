using System;
using UnityEngine;

namespace Client.Init
{
    [Serializable]
    public class MazeLevelConfig
    {
        [Header("Level Parameters")]
        [SerializeField] private int _rowsCount = 5;
        [SerializeField] private int _columnsCount = 5;
        [Space]
        [SerializeField] private float _cellWidth = 4;
        [SerializeField] private float _cellHeight = 4;
        [Space]
        [SerializeField] private Vector3 _playerSpawnPoint;
        [SerializeField] private Vector3 _mazeExitSpawnPoint;
        [Space] 
        [SerializeField] private int _timer;
        [Space] 
        [SerializeField] private int _itemsCount;

        public int RowsCount => _rowsCount;
        public int ColumnsCount => _columnsCount;
        public Vector3 PlayerSpawnPoint => _playerSpawnPoint;
        public float CellWidth => _cellWidth;
        public float CellHeight => _cellHeight;
        public Vector3 MazeExitSpawnPoint => _mazeExitSpawnPoint;
        public int Timer => _timer;
        public int ItemsCount => _itemsCount;
    }
}