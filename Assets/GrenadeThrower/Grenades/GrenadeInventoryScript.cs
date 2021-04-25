using System.Collections.Generic;
using GrenadeThrower.Inventory;
using UnityEngine;

namespace GrenadeThrower.Grenades
{
    public class GrenadeInventoryScript : MonoBehaviour
    {
        [SerializeField]
        private ThrowerScript throwerScript;

        private CircularInventory<Grenade> _inventory = new CircularInventory<Grenade>();

        public IEnumerable<IReadOnlyItemStack<Grenade>> Inventory => _inventory;

        public Grenade SelectedGrenade => _inventory.SelectedItem;
        
        private void Start()
        {
            throwerScript.ObjectThrownEvent += OnObjectThrownEvent;
            UpdateThrower();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _inventory.MovePrev();
                UpdateThrower();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _inventory.MoveNext();
                UpdateThrower();
            }
        }

        private void UpdateThrower()
        {
            throwerScript.enabled = !_inventory.IsEmpty;
        }

        private void OnObjectThrownEvent(GameObject obj)
        {
            var thrownGrenade = SelectedGrenade;
            if (thrownGrenade == null)
            {
                return;
            }

            _inventory.RemoveItem(thrownGrenade);
            obj.GetComponent<ThrowableGrenade>()?.SetGrenade(thrownGrenade);
            UpdateThrower();
        }

        public void AddToInventory(Grenade newGrenade)
        {
            _inventory.AddItem(newGrenade);
            UpdateThrower();
        }
    }
}