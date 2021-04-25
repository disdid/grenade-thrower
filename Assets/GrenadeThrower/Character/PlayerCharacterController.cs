using UnityEngine;

namespace GrenadeThrower.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [SerializeField]
        private Transform characterCamera;

        [SerializeField]
        private float walkingSpeed = 10.0f;
        
        private CharacterController _controller;
        private PlayerInputHandler _playerInputHandler;

        /// <summary>
        /// Camera position, where x is yaw and y is pitch
        /// </summary>
        private Vector2 _cameraPosition;
        
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            UpdateCameraPosition();
            UpdateCharacterPosition();
        }

        private void UpdateCameraPosition()
        {
            var cameraInput = _playerInputHandler.GetCameraInput();

            _cameraPosition += cameraInput;

            _cameraPosition = new Vector2(
                _cameraPosition.x % 360.0f,
                Mathf.Clamp(_cameraPosition.y, -90.0f, 90.0f));
            
            characterCamera.transform.rotation = transform.rotation * Quaternion.Euler(-_cameraPosition.y, _cameraPosition.x, 0.0f);
        }

        private void UpdateCharacterPosition()
        {
            var movementInput = _playerInputHandler.GetMovementInput();
            var movementInput3D = new Vector3(movementInput.y, 0.0f, movementInput.x);
            var movementRotation = Quaternion.Euler(0.0f, _cameraPosition.x, 0.0f);
            var movementSpeed = movementRotation * movementInput3D * walkingSpeed;
            _controller.SimpleMove(movementSpeed);
        }
    }
}
