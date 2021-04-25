using System;
using UnityEngine;

namespace GrenadeThrower.Character
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private const string HorizontalMouseInputName = "Mouse X";
        private const string VerticalMouseInputName = "Mouse Y";
        private const string ForwardMovementInputName = "Vertical";
        private const string LateralMovementInputName = "Horizontal";
        private const string FireButtonInputName = "Fire1";

        [SerializeField]
        private float cameraSensitivity = 1.0f;

        private bool _fireButtonPressedLastFrame;

        public bool IsProcessingInput => Cursor.lockState == CursorLockMode.Locked;

        [NonSerialized]
        public bool EnableMovementInput = true;


        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            UpdateFireInput();
        }

        private void UpdateFireInput()
        {
            _fireButtonPressedLastFrame = GetFireButtonPressed();
        }

        public Vector2 GetCameraInput()
        {
            if (!IsProcessingInput)
            {
                return Vector2.zero;
            }

            var horizontalInput = Input.GetAxisRaw(HorizontalMouseInputName);
            var verticalInput = Input.GetAxisRaw(VerticalMouseInputName);

            var cameraInput = new Vector2(horizontalInput, verticalInput);
            cameraInput *= cameraSensitivity;

            return cameraInput;
        }

        public Vector2 GetMovementInput()
        {
            if (!IsProcessingInput || !EnableMovementInput)
            {
                return Vector2.zero;
            }

            var forwardInput = Input.GetAxisRaw(ForwardMovementInputName);
            var lateralInput = Input.GetAxisRaw(LateralMovementInputName);

            var movementInput = new Vector2(forwardInput, lateralInput);
            movementInput = Vector2.ClampMagnitude(movementInput, 1);

            return movementInput;
        }

        public bool GetFireButtonPressed()
        {
            return IsProcessingInput && Input.GetAxisRaw(FireButtonInputName) > 0.0f;
        }

        public bool GetFireButtonDown()
        {
            return !_fireButtonPressedLastFrame && GetFireButtonPressed();
        }

        public bool GetFireButtonUp()
        {
            return _fireButtonPressedLastFrame && !GetFireButtonPressed();
        }
    }
}
