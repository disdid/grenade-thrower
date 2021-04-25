using UnityEngine;

namespace GrenadeThrower.Grenades
{
    [CreateAssetMenu(fileName = "New Grenade", menuName = "Grenade")]
    public class Grenade : ScriptableObject
    {
        public string displayedName = "Generic Grenade";
        public Color color = Color.white;
        public float explosionRadius = 3;
        public int damage = 5;
        public bool explodeOnTouch = true;
    }
}