namespace GrenadeThrower.Inventory
{
    public interface IReadOnlyItemStack<out T>
    {
        public T Item { get; }
        public int Count { get; }
    }
}