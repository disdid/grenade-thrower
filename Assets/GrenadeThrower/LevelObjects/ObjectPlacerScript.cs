using UnityEngine;
using Random = UnityEngine.Random;

namespace GrenadeThrower.LevelObjects
{
    /// <summary>
    /// Spawns prefab at random position inside volume
    /// </summary>
    public class ObjectPlacerScript : MonoBehaviour
    {
        [SerializeField]
        private int objectCount;

        [SerializeField]
        private int maxSpawnAttempts;

        [SerializeField]
        private GameObject objectPrefab;

        [SerializeField]
        private float collisionTestSphereRadius;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        }

        private void Start()
        {
            SpawnObjects();
        }

        private bool CheckPosition(Vector3 position, out float spawnHeight)
        {
            var ray = new Ray(position, Vector3.down);
            bool isHit = Physics.SphereCast(ray, collisionTestSphereRadius, out var hit, transform.lossyScale.y);
            spawnHeight = ray.origin.y - hit.distance;
            if (!isHit)
            {
                return false;
            }
            var hitOffset = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(hit.point.x, hit.point.z));
            return hitOffset < collisionTestSphereRadius * 0.1f;
        }

        private void SpawnObjects()
        {
            var spawnerTransform = transform;
            var spawnerPosition = spawnerTransform.position;
            var spawnerScale = spawnerTransform.lossyScale;

            var spawnAttempts = 0;
            var spawnedObjects = 0;

            while (spawnedObjects < objectCount && spawnAttempts < maxSpawnAttempts)
            {
                spawnAttempts++;

                var randomPosition = new Vector3(
                    spawnerPosition.x + spawnerScale.x * Random.Range(-0.5f, 0.5f),
                    spawnerPosition.y + spawnerScale.y * 0.5f,
                    spawnerPosition.z + spawnerScale.z * Random.Range(-0.5f, 0.5f));

                if (CheckPosition(randomPosition, out var height))
                {
                    randomPosition.y = height;
                    Instantiate(objectPrefab, randomPosition, Quaternion.identity);
                    spawnedObjects++;
                }
            }
        }
    }
}
