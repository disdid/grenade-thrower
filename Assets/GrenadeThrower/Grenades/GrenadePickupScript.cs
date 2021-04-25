using UnityEngine;

namespace GrenadeThrower.Grenades
{
    public class GrenadePickupScript : MonoBehaviour
    {
        [SerializeField]
        private Grenade grenade;

        [SerializeField]
        private MeshRenderer meshRenderer;

        private void Start()
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            meshRenderer.material.color = grenade.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            var inventory = other.gameObject.GetComponent<GrenadeInventoryScript>();

            if (inventory == null)
            {
                return;
            }
            
            inventory.AddToInventory(grenade);
            Destroy(gameObject);
        }
    }
}