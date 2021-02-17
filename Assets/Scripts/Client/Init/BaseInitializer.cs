using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client.Init
{
    public class BaseInitializer : MonoBehaviour
    {
        [SerializeField] private int _preloaderSceneIndex;
        private void Awake()
        {
            SceneManager.LoadSceneAsync(_preloaderSceneIndex, LoadSceneMode.Additive);
        }
    }
}