using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Client.Managers
{
    public interface ILoadingManager
    {
        void LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single);
        void UnloadSceneAsync(int sceneIndex);
        void SpawnPrefab(AssetReference assetReference, Action<GameObject> onComplete);
    }
}