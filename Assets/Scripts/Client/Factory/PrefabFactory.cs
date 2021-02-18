using System;
using Client.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Client.Factory
{
    public class PrefabFactory<T> : IFactory<T>
    {
        private GameObject _prefab;

        private AssetReference _prefabAssetReference;
        private ILoadingManager _loadingManager;

        public PrefabFactory(ILoadingManager loadingManager, AssetReference prefabAssetRef)
        {
            _loadingManager = loadingManager;
            _prefabAssetReference = prefabAssetRef;
        }

        public void Create(Action<T> onComplete)
        {
            _loadingManager.SpawnPrefab(_prefabAssetReference, go =>
            {
                var objectOfType = go.GetComponent<T>();
                onComplete?.Invoke(objectOfType);
            });
        }
    }
}