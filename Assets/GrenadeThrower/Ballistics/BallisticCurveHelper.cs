using Unity.Collections;
using UnityEngine;

namespace GrenadeThrower.Ballistics
{
    public static class BallisticCurveHelper
    {
        /// <summary>
        /// Traces curve
        /// </summary>
        /// <param name="curve">Curve to trace</param>
        /// <param name="points">Array of points. Will be filled until trace will collide</param>
        /// <param name="timeStep">Defines the distance between points</param>
        /// <param name="hitTestLayerMask">Layer mask of collision tests</param>
        /// <param name="collisionPoint">Equals to the position of trace hit</param>
        /// <returns>Amount of generated points</returns>
        public static int TraceCurve(this BallisticCurve curve, NativeArray<Vector3> points, float timeStep, LayerMask hitTestLayerMask, out Vector3? collisionPoint)
        {
            if (points.Length == 0)
            {
                collisionPoint = null;
                return 0;
            }

            points[0] = curve.StartingPosition;
            
            for (var i = 1; i < points.Length; i++)
            {
                var time = i * timeStep;
                var prevPoint = points[i - 1];
                var nextPoint = curve.GetPosition(time);
                if (Physics.Linecast(prevPoint, nextPoint, out var hit, hitTestLayerMask))
                {
                    points[i] = hit.point;
                    collisionPoint = hit.point;
                    return i + 1;
                }
                points[i] = nextPoint;
            }

            collisionPoint = null;
            return points.Length;
        }
    }
}