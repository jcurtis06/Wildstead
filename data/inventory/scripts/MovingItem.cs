namespace Wildstead.data.inventory.scripts;

public class MovingItem
{
    public Inventory FromInventory { get; set; }
    public int FromSlot { get; set; }
    public Item Item { get; set; }
    
    public MovingItem(Inventory fromInventory, int fromSlot, Item item)
    {
        FromInventory = fromInventory;
        FromSlot = fromSlot;
        Item = item;
    }
}