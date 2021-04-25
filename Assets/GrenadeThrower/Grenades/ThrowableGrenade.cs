using System.Linq;
using GrenadeThrower.Ballistics;
using GrenadeThrower.Damagable;
using UnityEngine;

namespace GrenadeThrower.Grenades
{
    [RequireComponent(typeof(Rigidbody))]
    public class ThrowableGrenade : MonoBehaviour, IThrowable
    {
        [SerializeField]
        private float maxLifetime = 3;

        [SerializeField]
        private GameObject explosionPrefab;

        [SerializeField]
        private Grenade grenade;

        private Rigidbody _rigidbody;

        private float _spawnTime;

        private BallisticCurve _curve;

        private void Awake()
        {
            _spawnTime = Time.fixedTime;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            var lifetime = Time.fixedTime - _spawnTime;

            if (lifetime > maxLifetime)
            {
                Explode();
            }
        }

        private void OnCollisionEnter()
        {
            if (grenade.explodeOnTouch)
            {
                Explode();
            }
        }

        public void SetTrajectory(BallisticCurve curve)
        {
            _curve = curve;

            if (_rigidbody == null)
            {
                return;
            }
            
            _rigidbody.position = _curve.StartingPosition;
            _rigidbody.velocity = _curve.InitialSpeed;
        }

        public void SetGrenade(Grenade g)
        {
            grenade = g;
        }

        private void SpawnExplosion()
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            var ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var mainModule = ps.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(grenade.color);
            }
        }

        private void DamageObjects()
        {
            var objects = Physics.OverlapSphere(transform.position, grenade.explosionRadius);
            foreach (var damagable in objects.SelectMany(c => c.gameObject.GetComponents<IDamagable>()))
            {
                damagable.Damage(grenade.damage);
            }
        }
        
        private void Explode()
        {
            if (grenade == null)
            {
                Debug.Log("Grenade is not specified", this);
                return;
            }
            SpawnExplosion();
            DamageObjects();
            Destroy(gameObject);
        }
    }
}