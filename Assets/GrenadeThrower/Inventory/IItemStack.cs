using System;

namespace GrenadeThrower.Inventory
{
    public class ItemStack<T> : IReadOnlyItemStack<T>
    {
        public T Item { get; }
        public int Count { get; set; }

        public ItemStack(T item, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count,
                    "ItemStack cannot have negative item count");
            }
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item stack cannot have null item");
            }

            Item = item;
            Count = count;
        }
    }
}