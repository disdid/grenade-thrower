using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GrenadeThrower.LevelObjects
{
    /// <summary>
    /// Simple prefab spawner script
    /// Spawns on start or after <see cref="spawnDelay"/> after previous spawned prefab destruction
    /// Randomly selects what to spawn from list <see cref="prefabs"/>
    /// </summary>
    public class SpawnerScript : MonoBehaviour
    {
        [Tooltip("Spawn pool of prefabs")]
        [SerializeField]
        private List<GameObject> prefabs;

        [Tooltip("Prefab respawn delay")]
        [SerializeField]
        private float spawnDelay = 5;
        
        [Tooltip("Already spawned instance of prefab")]
        [SerializeField]
        private GameObject spawnedObject;

        private float? _spawnTime;

        private void Start()
        {
            if (prefabs.Count == 0)
            {
                return;
            }

            if (spawnedObject == null)
            {
                Spawn();
            }
        }

        private void Update()
        {
            if (prefabs.Count == 0)
            {
                return;
            }

            if (!_spawnTime.HasValue)
            {
                if (spawnedObject == null)
                {
                    _spawnTime = Time.time + spawnDelay;
                }
            }

            if (_spawnTime.HasValue)
            {
                if (Time.time >= _spawnTime)
                {
                    Spawn();
                    _spawnTime = null;
                }
            }
        }

        private void Spawn()
        {
            var spawnTransform = transform;
            spawnedObject = Instantiate(prefabs[Random.Range(0, prefabs.Count)], spawnTransform.position, spawnTransform.rotation);
        }
    }
}
