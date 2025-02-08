using UnityEngine;

namespace Code.Gameplay.Common.View
{
    public class FaceToCamera : MonoBehaviour
    {
        private Camera _targetCamera;

        private void Start()
        {
            if (_targetCamera == null)
                _targetCamera = Camera.main;
        }

        private void Update()
        {
            if (_targetCamera == null)
                return;
            
            transform.LookAt(transform.position + _targetCamera.transform.rotation * Vector3.forward,
                _targetCamera.transform.rotation * Vector3.up);
        }
    }
}