using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GrenadeThrower.Inventory
{
    /// <summary>
    /// Simple circular inventory with single item selection
    /// Selection can be moved forward and backward
    /// </summary>
    /// <typeparam name="TItem">Stored item type</typeparam>
    public class CircularInventory<TItem> : IEnumerable<IReadOnlyItemStack<TItem>>
    {
        private readonly Dictionary<TItem, LinkedListNode<ItemStack<TItem>>> _nodeDictionary =
            new Dictionary<TItem, LinkedListNode<ItemStack<TItem>>>();

        private readonly LinkedList<ItemStack<TItem>> _inventory = new LinkedList<ItemStack<TItem>>();

        /// <summary>
        /// True if there are no items in the inventory
        /// </summary>
        public bool IsEmpty => _inventory.Count == 0;

        /// <summary>
        /// Currently selected item
        /// </summary>
        public TItem SelectedItem => IsEmpty ? default : _inventory.First.Value.Item;

        /// <summary>
        /// Adds item to the inventory. If added item already present in the inventory then increases its stack count
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="amount">Amount of items to add</param>
        /// <exception cref="ArgumentOutOfRangeException">Invalid amount of items specified</exception>
        /// <exception cref="ArgumentNullException">Invalid item specified</exception>
        public void AddItem(TItem item, int amount = 1)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount,
                    "Amount of added items to the inventory cannot be negative");
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Null item cannot be added to the inventory");
            }

            if (_nodeDictionary.TryGetValue(item, out var node))
            {
                node.Value.Count += amount;
            }
            else
            {
                var newNode = new LinkedListNode<ItemStack<TItem>>(new ItemStack<TItem>(item, amount));
                _inventory.AddLast(newNode);
                _nodeDictionary.Add(item, newNode);
            }
        }

        /// <summary>
        /// Removes single item from the inventory
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if item was found and removed from the inventory</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool RemoveItem(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Null item cannot be removed from the inventory");
            }

            if (_nodeDictionary.TryGetValue(item, out var node))
            {
                node.Value.Count -= 1;

                if (node.Value.Count <= 0)
                {
                    _nodeDictionary.Remove(item);
                    _inventory.Remove(node);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Moves selection to the next item
        /// </summary>
        public void MoveNext()
        {
            if (_inventory.Count < 2)
            {
                return;
            }

            var firstNode = _inventory.First;
            _inventory.RemoveFirst();
            _inventory.AddLast(firstNode);
        }

        /// <summary>
        /// Moves selection to the previous item
        /// </summary>
        public void MovePrev()
        {
            if (_inventory.Count < 2)
            {
                return;
            }

            var lastNode = _inventory.Last;
            _inventory.RemoveLast();
            _inventory.AddFirst(lastNode);
        }

        public IEnumerator<IReadOnlyItemStack<TItem>> GetEnumerator()
        {
            return _inventory.Cast<IReadOnlyItemStack<TItem>>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}