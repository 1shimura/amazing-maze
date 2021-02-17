using UnityEngine;

namespace Client.Maze
{
    public class MazeElement : MonoBehaviour, IResettable
    {
        private Transform _transform;
        private Vector3 _defaultPosition;
        private Quaternion _defaultRotation;

        private void Awake()
        {
            _transform = GetComponent<Transform>();

            _defaultPosition = _transform.position;
            _defaultRotation = _transform.rotation;
        }

        public void SetPositionAndRotation(Vector3 newPosition, Quaternion newRotation)
        {
            _transform.SetPositionAndRotation(newPosition, newRotation);
        }

        public void SetParent(Transform parentTransform)
        {
            _transform.SetParent(parentTransform);
        }
        
        public void PrewarmSetup()
        {
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            SetPositionAndRotation(_defaultPosition, _defaultRotation);
        }
    }
}