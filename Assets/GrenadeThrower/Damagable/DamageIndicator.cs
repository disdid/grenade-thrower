using TMPro;
using UnityEngine;

namespace GrenadeThrower.Damagable
{
    public static class DamageIndicator
    {
        private static readonly Vector3 PositionJitter = new Vector3(0.5f, 0.5f, 0.5f);

        private static readonly GameObject DamageIndicatorPrefab;

        static DamageIndicator()
        {
            DamageIndicatorPrefab = Resources.Load<GameObject>("Damage Indicator");
            if (DamageIndicatorPrefab == null)
            {
                Debug.LogWarning("Damage Indicator prefab not found!");
            }
        }
        
        /// <summary>
        /// Displays damage number
        /// </summary>
        /// <param name="position">World position of the number</param>
        /// <param name="value">Damage value</param>
        public static void Display(Vector3 position, int value)
        {
            if (DamageIndicatorPrefab == null)
            {
                return;
            }

            var indicatorPosition = position + new Vector3(
                Random.Range(-1.0f, 1.0f) * PositionJitter.x,
                Random.Range(-1.0f, 1.0f) * PositionJitter.y,
                Random.Range(-1.0f, 1.0f) * PositionJitter.z);
            var damageIndicator = Object.Instantiate(DamageIndicatorPrefab, indicatorPosition, Quaternion.identity);
            damageIndicator.GetComponent<TMP_Text>().text = value.ToString();
            Object.Destroy(damageIndicator, 2.0f);
        }
    }
}