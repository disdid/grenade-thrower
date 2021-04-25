using GrenadeThrower.Ballistics;

namespace GrenadeThrower.Grenades
{
    public interface IThrowable
    {
        public void SetTrajectory(BallisticCurve curve);
    }
}