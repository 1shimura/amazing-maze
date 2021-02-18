using System.Collections.Generic;
using Client.Maze;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client.Init
{
    [CreateAssetMenu(menuName = "Game Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private AssetReference _floorAssetRef = null;
        [SerializeField] private AssetReference _wallAssetRef = null;
        [SerializeField] private AssetReference _columnAssetRef = null;
        [Space] 
        [SerializeField] private AssetReference _playerAssetRef;
        [SerializeField] private AssetReference _itemAssetRef;
        [SerializeField] private AssetReference _mazeExitAssetRef;
        
        [Header("Generation")]
        [Header("Maze Settings")]
        [SerializeField] private MazeGenerationAlgorithm _algorithm = MazeGenerationAlgorithm.PureRecursive;
        [SerializeField] private bool _fullRandom = false;
        [SerializeField] private int _randomSeed = 12345;

        [SerializeField] private List<MazeLevelConfig> _mazeLevelsConfigList = new List<MazeLevelConfig>();
        
        public AssetReference ItemAssetRef => _itemAssetRef;
        public AssetReference MazeExitAssetRef => _mazeExitAssetRef;
        public AssetReference PlayerAssetRef => _playerAssetRef;
        public MazeGenerationAlgorithm Algorithm => _algorithm;
        public bool FullRandom => _fullRandom;
        public int RandomSeed => _randomSeed;

        public AssetReference FloorAssetRef => _floorAssetRef;
        public AssetReference WallAssetRef => _wallAssetRef;
        public AssetReference ColumnAssetRef => _columnAssetRef;
        public List<MazeLevelConfig> MazeLevelsConfigList => _mazeLevelsConfigList;
    }
}