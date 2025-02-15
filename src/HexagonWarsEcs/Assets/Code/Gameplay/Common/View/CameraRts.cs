using UnityEngine;

namespace Code.Gameplay.Common.View
{
    public class CameraRts : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 20f;
    
        private void Update() => 
            HandleMovement();

        private void HandleMovement()
        {
            if (Input.GetMouseButton(2))
            {
                float horizontal = Input.GetAxis("Mouse X");
                float vertical = Input.GetAxis("Mouse Y");

                Vector3 move = new Vector3(-horizontal, 0, -vertical) * moveSpeed * Time.deltaTime;
                transform.Translate(move, Space.World);
            }
        }
    }
}