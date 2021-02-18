using Client.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Client.Init
{
    public class BaseInitializer : MonoBehaviour
    {
        [SerializeField] private int _preloaderSceneIndex;

        [Inject] private ILoadingManager _loadingManager;
        
        private void Awake()
        {
            _loadingManager.LoadSceneAsync(_preloaderSceneIndex, LoadSceneMode.Additive);
        }
    }
}