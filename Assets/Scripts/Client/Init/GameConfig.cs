using System.Collections.Generic;
using Client.Maze;
using UnityEngine;

namespace Client.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _floorPrefab = null;
        [SerializeField] private GameObject _wallPrefab = null;
        [SerializeField] private GameObject _columnPrefab = null;
        [Space]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private GameObject _mazeExitPrefab;
        
        [Header("Generation")]
        [Header("Maze Settings")]
        [SerializeField] private MazeGenerationAlgorithm _algorithm = MazeGenerationAlgorithm.PureRecursive;
        [SerializeField] private bool _fullRandom = false;
        [SerializeField] private int _randomSeed = 12345;

        [SerializeField] private List<MazeLevelConfig> _mazeLevelsConfigList = new List<MazeLevelConfig>();
        
        public GameObject ItemPrefab => _itemPrefab;
        public GameObject MazeExitPrefab => _mazeExitPrefab;
        public GameObject PlayerPrefab => _playerPrefab;
        public MazeGenerationAlgorithm Algorithm => _algorithm;
        public bool FullRandom => _fullRandom;
        public int RandomSeed => _randomSeed;

        public GameObject FloorPrefab => _floorPrefab;
        public GameObject WallPrefab => _wallPrefab;
        public GameObject ColumnPrefab => _columnPrefab;
        public List<MazeLevelConfig> MazeLevelsConfigList => _mazeLevelsConfigList;
    }
}