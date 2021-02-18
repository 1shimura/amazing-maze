using System;
using System.Collections.Generic;
using Client;
using Client.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadingManager : ILoadingManager
{
    private readonly Dictionary<AssetReference, List<GameObject>> _spawnedGameObjects =
        new Dictionary<AssetReference, List<GameObject>>();

    private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles =
        new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();

    private readonly Dictionary<int, AsyncOperationHandle<SceneInstance>> _loadedScenes =
        new Dictionary<int, AsyncOperationHandle<SceneInstance>>();

    public void LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
    {
        Addressables.LoadSceneAsync(sceneIndex, mode).Completed += handle =>
        {
            _loadedScenes.Add(sceneIndex, handle);
        };
    }

    public void UnloadSceneAsync(int sceneIndex)
    {
        if (_loadedScenes.ContainsKey(sceneIndex))
        {
            Addressables.UnloadSceneAsync(_loadedScenes[sceneIndex]).Completed += handle =>
            {
                _loadedScenes.Remove(sceneIndex);
            };
        }
    }

    public void SpawnPrefab(AssetReference assetReference, Action<GameObject> onComplete)
    {
        if (!assetReference.RuntimeKeyIsValid())
        {
            Debug.Log($"Invalid Key: {assetReference.RuntimeKey}");
            return;
        }

        if (_asyncOperationHandles.ContainsKey(assetReference))
        {
            SpawnGameObjectFromLoadedReference(assetReference, onComplete);
            return;
        }

        LoadAndSpawn(assetReference, onComplete);
    }

    private void LoadAndSpawn(AssetReference assetReference, Action<GameObject> onComplete)
    {
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);

        _asyncOperationHandles[assetReference] = op;

        op.Completed += operation => { SpawnGameObjectFromLoadedReference(assetReference, onComplete); };
    }

    private void SpawnGameObjectFromLoadedReference(AssetReference assetReference, Action<GameObject> onComplete)
    {
        assetReference.InstantiateAsync().Completed += asyncOperationHandles =>
        {
            if (!_spawnedGameObjects.ContainsKey(assetReference))
            {
                _spawnedGameObjects[assetReference] = new List<GameObject>();
            }

            _spawnedGameObjects[assetReference].Add(asyncOperationHandles.Result);
            var notify = asyncOperationHandles.Result.AddComponent<NotifyOnDestroy>();

            notify.Destroyed += Remove;
            notify.AssetReference = assetReference;

            onComplete?.Invoke(asyncOperationHandles.Result);
        };
    }

    private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
    {
        Addressables.ReleaseInstance(obj.gameObject);

        _spawnedGameObjects[assetReference].Remove(obj.gameObject);

        if (_spawnedGameObjects[assetReference].Count != 0) return;


        if (_asyncOperationHandles[assetReference].IsValid())
        {
            Addressables.ReleaseInstance(_asyncOperationHandles[assetReference]);
        }

        _asyncOperationHandles.Remove(assetReference);
    }
}