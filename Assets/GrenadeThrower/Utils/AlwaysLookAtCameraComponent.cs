using UnityEngine;

namespace GrenadeThrower.Utils
{
    /// <summary>
    /// Orients object towards camera
    /// </summary>
    public class AlwaysLookAtCameraComponent : MonoBehaviour
    {
        private void LateUpdate()
        {
            var cam = Camera.main;
            if (cam == null)
            {
                return;
            }
            transform.rotation = cam.transform.rotation;
        }
    }
}