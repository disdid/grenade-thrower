using UnityEngine;

namespace GrenadeThrower.Ballistics
{
    public readonly struct BallisticCurve
    {
        private readonly Vector3 _startingPosition;
        private readonly Vector3 _initialSpeed;

        public Vector3 StartingPosition => _startingPosition;
        public Vector3 InitialSpeed => _initialSpeed;
        
        public BallisticCurve(Transform launchTransform, float launchSpeed)
        {
            _startingPosition = launchTransform.position;
            _initialSpeed = launchTransform.forward * launchSpeed;
        }

        public Vector3 GetPosition(float time)
        {
            return _startingPosition +
                   _initialSpeed * time +
                   Physics.gravity * (time * time * 0.5f);
        }
    }
}